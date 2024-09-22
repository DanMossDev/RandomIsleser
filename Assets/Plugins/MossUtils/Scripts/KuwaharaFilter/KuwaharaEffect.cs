using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Serialization;

namespace MossUtils
{
    public class KuwaharaEffect : ScriptableRendererFeature
    {
        private static readonly LayerMask _allLayers = ~0;
        private const int _filterKernalSize = 32;
        
        public Settings Settings;

        private DepthOnlyPass _depthOnlyPass;
        private KuwaharaEffectPass _renderPass;

        private RTHandle _depthTexture;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData data)
        {
            if (!Settings.ShowInEditor && Application.isEditor && (data.cameraData.camera.name == "SceneCamera" || data.cameraData.camera.name == "Preview Scene Camera")) 
            {
                return;
            }

            _renderPass.Setup(Settings);
            renderer.EnqueuePass(_renderPass);
        }

        public override void Create()
        {
            var structureTensorMat = CoreUtils.CreateEngineMaterial("Hidden/MossUtils/BlurredStructureTensor");
            var kuwaharaFilterMaterial = CoreUtils.CreateEngineMaterial("Hidden/MossUtils/AnisotropicKuwaharaFilter");
            var lineIntegralConvolutionMaterial = CoreUtils.CreateEngineMaterial("Hidden/MossUtils/LineIntegralConvolution");
            var compositorMaterial = CoreUtils.CreateEngineMaterial("Hidden/MossUtils/Compositor");
            
            _renderPass = new KuwaharaEffectPass(structureTensorMat, kuwaharaFilterMaterial, lineIntegralConvolutionMaterial, compositorMaterial)
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
            };

            var tex = new Texture2D(_filterKernalSize, _filterKernalSize, TextureFormat.RFloat, true);
            InitializeFilterKernelTexture(tex,
                _filterKernalSize,
                Settings.AnisotropicKuwaharaFilterSettings.filterKernelSectors,
                Settings.AnisotropicKuwaharaFilterSettings.filterKernelSmoothness);

            Settings.AnisotropicKuwaharaFilterSettings.filterKernelTexture = tex;
        }
        private static void InitializeFilterKernelTexture(Texture2D texture, int kernelSize, int sectorCount, float smoothing)
        {
            for (int j = 0; j < texture.height; j++)
            {
                for (int i = 0; i < texture.width; i++)
                {
                    float x = i - 0.5f * texture.width + 0.5f;
                    float y = j - 0.5f * texture.height + 0.5f;
                    float r = Mathf.Sqrt(x * x + y * y);

                    float a = 0.5f * Mathf.Atan2(y, x) / Mathf.PI;

                    if (a > 0.5f)
                    {
                        a -= 1f;
                    }
                    if (a < -0.5f)
                    {
                        a += 1f;
                    }

                    if ((Mathf.Abs(a) <= 0.5f / sectorCount) && (r < 0.5f * kernelSize))
                    {
                        texture.SetPixel(i, j, Color.red);
                    }
                    else
                    {
                        texture.SetPixel(i, j, Color.black);
                    }
                }
            }

            float sigma = 0.25f * (kernelSize - 1);

            GaussianBlur(texture, sigma * smoothing);

            float maxValue = 0f;
            for (int j = 0; j < texture.height; j++)
            {
                for (int i = 0; i < texture.width; i++)
                {
                    var x = i - 0.5f * texture.width + 0.5f;
                    var y = j - 0.5f * texture.height + 0.5f;
                    var r = Mathf.Sqrt(x * x + y * y);

                    var color = texture.GetPixel(i, j);
                    color *= Mathf.Exp(-0.5f * r * r / sigma / sigma);
                    texture.SetPixel(i, j, color);

                    if (color.r > maxValue)
                    {
                        maxValue = color.r;
                    }
                }
            }

            for (int j = 0; j < texture.height; j++)
            {
                for (int i = 0; i < texture.width; i++)
                {
                    var color = texture.GetPixel(i, j);
                    color /= maxValue;
                    texture.SetPixel(i, j, color);
                }
            }

            texture.Apply(true, true);
        }

        private static void GaussianBlur(Texture2D texture, float sigma)
        {
            float twiceSigmaSq = 2.0f * sigma * sigma;
            int halfWidth = Mathf.CeilToInt(2 * sigma);

            var colors = new Color[texture.width * texture.height];

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    int index = y * texture.width + x;

                    float norm = 0;
                    for (int i = -halfWidth; i <= halfWidth; i++)
                    {
                        int xi = x + i;
                        if (xi < 0 || xi >= texture.width) continue;

                        for (int j = -halfWidth; j <= halfWidth; j++)
                        {
                            int yj = y + j;
                            if (yj < 0 || yj >= texture.height) continue;

                            float distance = Mathf.Sqrt(i * i + j * j);
                            float k = Mathf.Exp(-distance * distance / twiceSigmaSq);

                            colors[index] += texture.GetPixel(xi, yj) * k;
                            norm += k;
                        }
                    }

                    colors[index] /= norm;
                }
            }

            texture.SetPixels(colors);
        }
    } 


    [Serializable]
    public class Settings
    {
        public bool ShowInEditor = false;
        
        public AnisotropicKuwaharaFilterSettings AnisotropicKuwaharaFilterSettings;
        public EdgeFlowSettings EdgeFlowSettings;
        public CompositorSettings CompositorSettings;
    }
    
    [Serializable]
    public class AnisotropicKuwaharaFilterSettings
    {
        [Range(3, 8)]
        public int filterKernelSectors = 8;
        
        [Range(0f, 1f)]
        public float filterKernelSmoothness = 0.33f;
        
        [NonSerialized]
        public Texture2D filterKernelTexture;
    
        [Range(2f, 12f)]
        public float filterRadius = 4f;
        [Range(2f, 16f)]
        public float filterSharpness = 8f;
        [Range(0.125f, 8f)]
        public float eccentricity = 1f;
    
        [Range(1, 4)]
        public int iterations = 1;
    }
    
    [Serializable]
    public class EdgeFlowSettings
    {
        public Texture2D noiseTexture;
    
         [Range(1, 64)]
         public int streamLineLength = 10;
         [Range(0f, 2f)]
         public float streamKernelStrength = 0.5f;
    }
    
    [Serializable]
    public class CompositorSettings
    {
        [Range(0f, 4f)]
        public float edgeContribution = 1f;
        [Range(0f, 4f)]
        public float flowContribution = 1f;

        [Range(0.25f, 1f)]
        public float bumpPower = 0.8f;
        [Range(0f, 1f)]
        public float bumpIntensity = 0.4f;
    }
}
