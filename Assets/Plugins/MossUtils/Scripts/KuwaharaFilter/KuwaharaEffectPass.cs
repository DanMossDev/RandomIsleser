using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MossUtils
{
    public class KuwaharaEffectPass : ScriptableRenderPass
    {
        private RTHandle _source;
        private RTHandle _destination;

        private RenderTexture _structureTensorTex;
        private RenderTexture _kuwaharaFilterTex;
        private RenderTexture _edgeFlowTex;

        private RTHandle _tensorHandle;
        private RTHandle _kuwaharaHandle;
        private RTHandle _edgeFlowHandle;
        
        private readonly Material _structureTensorMat;
        private readonly Material _kuwaharaFilterMat;
        private readonly Material _lineIntegralConvolutionMat;
        private readonly Material _compositorMat;

        private int _kuwaharaFilterIterations = 1;

        public KuwaharaEffectPass(Material structureTensorMat, Material kuwaharaFilterMat, Material lineIntegralConvolutionMat, Material compositorMat)
        {
            _structureTensorMat = structureTensorMat;
            _kuwaharaFilterMat = kuwaharaFilterMat;
            _lineIntegralConvolutionMat = lineIntegralConvolutionMat;
            _compositorMat = compositorMat;
        }

        public void Setup(MossUtils.Settings settings)
        {
            SetupKuwaharaFilter(settings.AnisotropicKuwaharaFilterSettings);
            SetupLineIntegralConvolution(settings.EdgeFlowSettings);
            SetupCompositor(settings.CompositorSettings);
        }
        
        private void SetupKuwaharaFilter(AnisotropicKuwaharaFilterSettings kuwaharaFilterSettings)
        {
            _kuwaharaFilterMat.SetInt("_FilterKernelSectors", kuwaharaFilterSettings.filterKernelSectors);
            _kuwaharaFilterMat.SetTexture("_FilterKernelTex", kuwaharaFilterSettings.filterKernelTexture);
            _kuwaharaFilterMat.SetFloat("_FilterRadius", kuwaharaFilterSettings.filterRadius);
            _kuwaharaFilterMat.SetFloat("_FilterSharpness", kuwaharaFilterSettings.filterSharpness);
            _kuwaharaFilterMat.SetFloat("_Eccentricity", kuwaharaFilterSettings.eccentricity);
            _kuwaharaFilterIterations = kuwaharaFilterSettings.iterations;
        }

        private void SetupLineIntegralConvolution(EdgeFlowSettings edgeFlowSettings)
        {
            _lineIntegralConvolutionMat.SetTexture("_NoiseTex", edgeFlowSettings.noiseTexture);
            _lineIntegralConvolutionMat.SetInt("_StreamLineLength", edgeFlowSettings.streamLineLength);
            _lineIntegralConvolutionMat.SetFloat("_StreamKernelStrength", edgeFlowSettings.streamKernelStrength);
        }
        
        private void SetupCompositor(CompositorSettings compositorSettings)
        {
            _compositorMat.SetFloat("_EdgeContribution", compositorSettings.edgeContribution);
            _compositorMat.SetFloat("_FlowContribution", compositorSettings.flowContribution);
            _compositorMat.SetFloat("_BumpPower", compositorSettings.bumpPower);
            _compositorMat.SetFloat("_BumpIntensity", compositorSettings.bumpIntensity);
        }
    
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData data)
        {
            RenderTextureDescriptor blitTargetDescriptor = data.cameraData.cameraTargetDescriptor;
            blitTargetDescriptor.depthBufferBits = 0;
    
            var renderer = data.cameraData.renderer;
    
            _source = renderer.cameraColorTargetHandle;
            _destination = renderer.cameraColorTargetHandle;

            _structureTensorTex = RenderTexture.GetTemporary(blitTargetDescriptor.width, blitTargetDescriptor.height, 0, RenderTextureFormat.ARGBFloat);
            _tensorHandle = RTHandles.Alloc(_structureTensorTex);
            _kuwaharaFilterTex = RenderTexture.GetTemporary(blitTargetDescriptor);
            _kuwaharaHandle = RTHandles.Alloc(_kuwaharaFilterTex);
            _edgeFlowTex = RenderTexture.GetTemporary(blitTargetDescriptor.width, blitTargetDescriptor.height, 0, RenderTextureFormat.RFloat);
            _edgeFlowHandle = RTHandles.Alloc(_edgeFlowTex);
        }
    
        public override void Execute(ScriptableRenderContext context, ref RenderingData data)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Kuwahara Effect");
            
            cmd.Blit(_source, _tensorHandle, _structureTensorMat, -1);

            _kuwaharaFilterMat.SetTexture("_StructureTensorTex", _structureTensorTex);

            cmd.Blit( _source, _kuwaharaFilterTex, _kuwaharaFilterMat, -1);
            for (int i = 0; i < _kuwaharaFilterIterations - 1; i++)
            {
                cmd.Blit(_kuwaharaFilterTex, _source, _kuwaharaFilterMat, -1);
                cmd.Blit( _source, _kuwaharaFilterTex, _kuwaharaFilterMat, -1);
            }
            cmd.Blit(_structureTensorTex, _edgeFlowTex, _lineIntegralConvolutionMat, -1);
            
            _compositorMat.SetTexture("_EdgeFlowTex", _edgeFlowTex);
            cmd.Blit(_kuwaharaFilterTex, _destination, _compositorMat, -1);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    
        public override void FrameCleanup(CommandBuffer cmd)
        {
            RenderTexture.ReleaseTemporary(_structureTensorTex);
            RenderTexture.ReleaseTemporary(_kuwaharaFilterTex);
            RenderTexture.ReleaseTemporary(_edgeFlowTex);
        }
    }
}
