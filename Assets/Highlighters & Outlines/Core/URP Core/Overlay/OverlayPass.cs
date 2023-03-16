using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

namespace Highlighters
{
    public class OverlayPass : ScriptableRenderPass
    {
        private readonly Material material;
        private RenderTargetIdentifier cameraColorTarget;

        private string profilingName;
        private RenderTargetIdentifier objectsInfoIdentifier;
        private RenderTargetIdentifier meshOutlineIdentifier;
        private bool useMeshOutline = false;

        public OverlayPass(RenderPassEvent renderPassEvent, HighlighterSettings highlighterSettings,string profilingName)
        {
            this.renderPassEvent = renderPassEvent;
            this.profilingName = profilingName;

            material = new Material(Shader.Find("Highlighters/Overlay"));
            highlighterSettings.SetOverlayMaterialProperties(material);
        }

        public void SetupMeshOutlineTarget(RenderTargetIdentifier meshOutlineIdentifier)
        {
            useMeshOutline = true;
            this.meshOutlineIdentifier = meshOutlineIdentifier;
        }

        public void SetupObjectsTarget(RenderTargetIdentifier objectsInfoIdentifier)
        {
            this.objectsInfoIdentifier = objectsInfoIdentifier;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!material) return;

			var renderer = renderingData.cameraData.renderer;
            cameraColorTarget = renderer.cameraColorTarget;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(
                profilingName)))
            {
                cmd.SetGlobalTexture("_ObjectsInfo", objectsInfoIdentifier);
                if(useMeshOutline) cmd.SetGlobalTexture("_MeshOutlineObjects", meshOutlineIdentifier);

                Blit(cmd, cameraColorTarget, cameraColorTarget, material, 0);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }


        public override void FrameCleanup(CommandBuffer cmd)
        {
        }
    }
}