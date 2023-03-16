using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Highlighters
{
    public class DepthMaskPass : ScriptableRenderPass
    {
        private readonly string profilingName;
        public readonly RenderTargetHandle sceneDepthMask;

        private readonly Material maskMaterial;
        private readonly List<ShaderTagId> shaderTagIdList;

        private FilteringSettings filteringSettings;

        public DepthMaskPass(RenderPassEvent renderPassEvent, int layerMask, string profilingName) 
        {
            this.profilingName = profilingName;
            this.renderPassEvent = renderPassEvent;

            filteringSettings = new FilteringSettings(RenderQueueRange.all, layerMask);

            maskMaterial = new Material(Shader.Find("Highlighters/SceneDepthShader"));

            shaderTagIdList = new List<ShaderTagId>()
            {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefoultUnlit"),
                new ShaderTagId("DepthOnly"),
                new ShaderTagId("UniversalGBuffer"),
                new ShaderTagId("DepthNormalsOnly"),
                new ShaderTagId("Universal2D"),
                new ShaderTagId("SRPDefaultUnlit"),
            };

            sceneDepthMask.Init("_SceneDepthMask");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor textureDescriptor = cameraTextureDescriptor;
            textureDescriptor.colorFormat = RenderTextureFormat.RFloat;
            textureDescriptor.msaaSamples = 1;

            cmd.GetTemporaryRT(sceneDepthMask.id, textureDescriptor, FilterMode.Point);
            ConfigureTarget(sceneDepthMask.Identifier());
            ConfigureClear(ClearFlag.All, Color.black);
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!maskMaterial)
                return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(
                profilingName)))
            {
                //context.ExecuteCommandBuffer(cmd);
                //cmd.Clear();

                DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                drawingSettings.overrideMaterial = maskMaterial;
                drawingSettings.enableDynamicBatching = true;

                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);//, ref m_RenderStateBlock
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(sceneDepthMask.id);
        }
    }
}