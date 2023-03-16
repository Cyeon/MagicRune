using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Highlighters
{
    public class ObjectsPass : ScriptableRenderPass
    {
        private readonly string profilingName;
        public readonly RenderTargetHandle objectsInfo;

        private List<HighlighterRenderer> renderersToDraw;
        private List<Material> materialsToDraw;
        private List<int> materialsPassIndexes;
        private HighlighterSettings highlighterSettings;

        private RenderTargetIdentifier sceneDepthMaskIdentifier;
        private bool useSceneDepth = false;

        /// <summary>
        /// Vector4(MinX, MinY, MaxX, MaxY) in viewport space
        /// </summary>
        private Vector4 renderingBounds;

        public ObjectsPass(RenderPassEvent renderPassEvent, HighlighterSettings highlighterSettings, int ID, string profilingName, List<HighlighterRenderer> renderers)
        {
            this.renderPassEvent = renderPassEvent;
            this.profilingName = profilingName;
            this.highlighterSettings = highlighterSettings;

            objectsInfo.Init("_HighlightedObjects_" + ID);

            renderersToDraw = renderers;
            UpdateMaterialsToDraw();
        }

        public void SetupSceneDepthTarget(RenderTargetIdentifier sceneDepthMaskIdentifier)
        {
            useSceneDepth = true;
            this.sceneDepthMaskIdentifier = sceneDepthMaskIdentifier;
        }

        private void UpdateMaterialsToDraw()
        {
            materialsToDraw = new List<Material>();
            materialsPassIndexes = new List<int>();

            bool useDepth = true;
            if (highlighterSettings.DepthMask == DepthMask.Disable) useDepth = false;

            foreach (var item in renderersToDraw)
            {
                if (item.useCutout)
                {
                    var materialCutout = new Material(Shader.Find("Highlighters/ObjectsInfo"));
                    materialCutout.SetTexture("_MainTex", item.GetClipTexture());
                    materialCutout.SetFloat("_Cutoff", item.clippingThreshold);
                    materialCutout.SetInt("useDepth", useDepth ? 1 : 0);
                    materialsToDraw.Add(materialCutout);
                    materialsPassIndexes.Add(((int)item.cullMode));

                }
                else
                {
                    var material = new Material(Shader.Find("Highlighters/ObjectsInfo"));
                    material.SetInt("useDepth", useDepth ? 1 : 0);
                    materialsToDraw.Add(material);
                    //materialsPassIndexes.Add(((int)item.cullMode));
                    materialsPassIndexes.Add(((int)item.cullMode));
                }
            }
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor textureDescriptor = cameraTextureDescriptor;
            textureDescriptor.colorFormat = RenderTextureFormat.ARGBFloat; // could be: RGB565
            textureDescriptor.msaaSamples = 1;
            textureDescriptor.width = Mathf.FloorToInt(textureDescriptor.width * highlighterSettings.InfoRenderScale);
            textureDescriptor.height = Mathf.FloorToInt(textureDescriptor.height * highlighterSettings.InfoRenderScale);

            cmd.GetTemporaryRT(objectsInfo.id, textureDescriptor, FilterMode.Bilinear);
            ConfigureTarget(objectsInfo.Identifier());
            ConfigureClear(ClearFlag.All, new Color(0, 0, 0, 0));
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(
                profilingName)))
            {
                if (useSceneDepth) cmd.SetGlobalTexture("_SceneDepthMask", sceneDepthMaskIdentifier);

                if (renderersToDraw.Count == materialsToDraw.Count)
                {
                    renderingBounds = new Vector4(10, 10, -10, -10);
                    Vector3 rendererCenter = Vector3.zero;

                    var camera = renderingData.cameraData.camera;
                    if (camera != null)
                    {
                        for (int i = 0; i < renderersToDraw.Count; i++)
                        {
                            var item = renderersToDraw[i];
                            if (item.renderer == null || item.renderer.enabled == false) continue;

                            for (int submeshIndex = 0; submeshIndex < item.submeshIndexes.Count; submeshIndex++)
                            {
                                cmd.DrawRenderer(item.renderer, materialsToDraw[i], item.submeshIndexes[submeshIndex], materialsPassIndexes[i]);
                            }

                            
                            var bounds = item.renderer.bounds;
                            var center = bounds.center;
                            var extents = bounds.extents;
                            rendererCenter = center;

                            if (highlighterSettings.RenderingBoundsDistanceFix)
                            {
                                if (RenderingBounds.CloseEnughToRenderFullScreen(camera, center, highlighterSettings.RenderingBoundsMaxDistanceFix, highlighterSettings.RenderingBoundsMinDistanceFix))
                                {
                                    renderingBounds = new Vector4(0, 0, 1, 1);
                                    highlighterSettings.SetRenderBoundsValues(renderingBounds);
                                    continue;
                                }
                            }

                            renderingBounds = RenderingBounds.CalculateBounds(camera, extents, center, renderingBounds, highlighterSettings.RenderingBoundsSizeIncrease);
                        }
                    }

                    float cameraObjectDist = (rendererCenter - camera.transform.position).magnitude;
                    highlighterSettings.CameraObjectDistace = cameraObjectDist;

                    highlighterSettings.SetRenderBoundsValues(renderingBounds);
                }
                else
                {
                    UpdateMaterialsToDraw();
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(objectsInfo.id);
        }
    }
}