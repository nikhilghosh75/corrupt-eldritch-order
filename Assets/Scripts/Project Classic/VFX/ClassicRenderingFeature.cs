using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClassicRenderingFeature : ScriptableRendererFeature
{
    class ClassicRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RTHandle renderTarget;

        public ClassicRenderPass(Material material)
        {
            this.material = material;
            renderTarget = RTHandles.Alloc("_TempTexture", "_TempTexture");
        }

        // Setup method
        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        // Execute method
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("CustomPostProcessing");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(Shader.PropertyToID(renderTarget.name), opaqueDesc);
            Blit(cmd, source, renderTarget.nameID, material, 0);
            Blit(cmd, renderTarget.nameID, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        // Cleanup method
        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(renderTarget.name));
        }
    }

    [System.Serializable]
    public class CustomSettings
    {
        public Material material = null;
    }

    public CustomSettings settings = new CustomSettings();
    ClassicRenderPass customRenderPass;

    // Create method
    public override void Create()
    {
        customRenderPass = new ClassicRenderPass(settings.material)
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
        };
    }

    // AddRenderPasses method
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material != null)
        {
            customRenderPass.Setup(renderer.cameraColorTargetHandle);
            renderer.EnqueuePass(customRenderPass);
        }
    }
}
