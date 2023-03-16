using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

namespace Highlighters
{
    public class PerObjectBlurPass : ScriptableRenderPass
    {
        private readonly Material blurMaterial;
        private RenderTargetIdentifier cameraColorTarget;

        private RenderTargetHandle blurredObjectsBuffer;
        private string profilingName;
        private HighlighterSettings highlighterSettings;

        // Accurate Alpha Blending for two sides depth mask properties
        private RenderTargetHandle blurredObjectsBothBuffer;
        private readonly Material alphaBlitMaterial;

        private RenderTargetIdentifier objectsInfoIdentifier;

        #region GaussianBlur

        private int MaxWidth = 50;
        private float[] gaussSamples;

        private float[] GetGaussSamples(int width, float[] samples)
        {
            var stdDev = width * 0.5f;

            if (samples is null)
            {
                samples = new float[MaxWidth];
            }

            for (var i = 0; i < width; i++)
            {
                samples[i] = Gauss(i, stdDev);
            }

            return samples;
        }
        private float Gauss(float x, float stdDev)
        {
            var stdDev2 = stdDev * stdDev * 2;
            var a = 1 / Mathf.Sqrt(Mathf.PI * stdDev2);
            var gauss = a * Mathf.Pow((float)Math.E, -x * x / stdDev2);

            return gauss;
        }
        #endregion

        public PerObjectBlurPass(RenderPassEvent renderPassEvent, HighlighterSettings blurOutlineSettings, int ID, string profilingName)
        {
            this.renderPassEvent = renderPassEvent;
            this.profilingName = profilingName;
            this.highlighterSettings = blurOutlineSettings;

            blurMaterial = new Material(Shader.Find("Highlighters/Blur"));
            blurOutlineSettings.SetBlurMaterialProperties(blurMaterial);
            blurMaterial.EnableKeyword("_Variation_" + ID.ToString());

            gaussSamples = GetGaussSamples(50, gaussSamples);
            blurMaterial.SetFloatArray("_GaussSamples", gaussSamples);

            alphaBlitMaterial = new Material(Shader.Find("Highlighters/AlphaBlit"));
            alphaBlitMaterial.EnableKeyword("_Variation_" + ID.ToString());
            blurOutlineSettings.SetAlphaBlitMaterialProperties(alphaBlitMaterial);

            blurredObjectsBuffer.Init("_BluerredObjects_" + ID.ToString());
            blurredObjectsBothBuffer.Init("_BlurredObjectsBoth_" + ID.ToString());
        }

        public void SetupObjectsTarget(RenderTargetIdentifier objectsInfoIdentifier)
        {
            this.objectsInfoIdentifier = objectsInfoIdentifier;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor textureDescriptor = cameraTextureDescriptor;
            textureDescriptor.colorFormat = RenderTextureFormat.RGFloat; // was: RG16  ARGBFloat
            textureDescriptor.msaaSamples = 1;
            textureDescriptor.width = Mathf.FloorToInt(textureDescriptor.width * highlighterSettings.BlurRenderScale);
            textureDescriptor.height = Mathf.FloorToInt(textureDescriptor.height * highlighterSettings.BlurRenderScale);
            
            cmd.GetTemporaryRT(blurredObjectsBuffer.id, textureDescriptor, FilterMode.Bilinear);
            cmd.GetTemporaryRT(blurredObjectsBothBuffer.id, textureDescriptor, FilterMode.Bilinear);

            cmd.SetGlobalTexture("_ObjectsInfo", objectsInfoIdentifier);
            cmd.SetGlobalTexture("_BlurredObjects", blurredObjectsBuffer.Identifier());
            cmd.SetGlobalTexture("_BlurredObjectsBoth", blurredObjectsBothBuffer.Identifier());

            //ConfigureTarget(blurredObjectsBuffer.Identifier());
            //ConfigureClear(ClearFlag.Color, new Color(0, 0, 0, 0));

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!blurMaterial) return;

            // Set Source / Destination
            var renderer = renderingData.cameraData.renderer;

            cameraColorTarget = renderer.cameraColorTarget;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(
                profilingName)))
            {
                //RenderTextureDescriptor textureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                //textureDescriptor.colorFormat = RenderTextureFormat.RGFloat; // was: RG16  ARGBFloat
                //textureDescriptor.msaaSamples = 1;
                //textureDescriptor.width = Mathf.FloorToInt(textureDescriptor.width * highlighterSettings.BlurRenderScale);
                //textureDescriptor.height = Mathf.FloorToInt(textureDescriptor.height * highlighterSettings.BlurRenderScale);
                //
                //cmd.GetTemporaryRT(blurredObjectsBuffer.id, textureDescriptor, FilterMode.Bilinear);
                //cmd.GetTemporaryRT(blurredObjectsBothBuffer.id, textureDescriptor, FilterMode.Bilinear);


                // Horizontal blur
                Blit(cmd, cameraColorTarget, blurredObjectsBuffer.Identifier(), blurMaterial, 0);

                //cmd.SetRenderTarget(cameraColorTarget);
                //Blit(cmd, cameraColorTarget, cameraColorTarget, blurMaterial, 2);
                //Blit(cmd, blurredObjectsBuffer.Identifier(), cameraColorTarget);


                if (highlighterSettings.BlendingType == HighlighterSettings.BlendType.Alpha)
                {
                    // Use additional buffer for right alpha blending
                    if(highlighterSettings.DepthMask == DepthMask.Both && !highlighterSettings.UseSingleOuterGlow)
                    {
                        // Use VPassAlphaBoth shader Pass
                        Blit(cmd, blurredObjectsBuffer.Identifier(), blurredObjectsBothBuffer.Identifier(), blurMaterial, 3);
                
                        Blit(cmd,blurredObjectsBothBuffer.Identifier(), cameraColorTarget, alphaBlitMaterial, 0); // Front Blit
                        Blit(cmd,blurredObjectsBothBuffer.Identifier(), cameraColorTarget, alphaBlitMaterial, 1); // Back Blit
                    }
                    else
                    {
                        Blit(cmd, blurredObjectsBuffer.Identifier(), cameraColorTarget, blurMaterial, 1);
                    }
                }
                else // Don't do anything fancy when BlendType is additive 
                {
                    Blit(cmd, blurredObjectsBuffer.Identifier(), cameraColorTarget, blurMaterial,2); 
                }

            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }


        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(blurredObjectsBuffer.id);
            cmd.ReleaseTemporaryRT(blurredObjectsBothBuffer.id);
        }
    }
}