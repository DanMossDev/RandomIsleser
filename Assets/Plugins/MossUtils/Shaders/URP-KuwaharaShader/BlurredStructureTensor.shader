Shader "Hidden/MossUtils/BlurredStructureTensor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        
        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            #define PIXEL_X (_ScreenParams.z - 1)
            #define PIXEL_Y (_ScreenParams.w - 1)

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Varyings vert(Attributes i)
            {
                Varyings o = (Varyings)0;

                VertexPositionInputs vertInput = GetVertexPositionInputs(i.positionOS.xyz);
                o.vertex = vertInput.positionCS;
                o.uv = i.uv;

                return o;
            }

            float3 SampleMain(float2 uv)
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).rgb;
            }

            float3 SobelU(float2 uv)
            {
                return (
                    -1.0f * SampleMain(uv + float2(-PIXEL_X, -PIXEL_Y)) +
                    -2.0f * SampleMain(uv + float2(-PIXEL_X, 0)) +
                    -1.0f * SampleMain(uv + float2(-PIXEL_X, PIXEL_Y)) +

                    1.0f * SampleMain(uv + float2(PIXEL_X, -PIXEL_Y)) +
                    2.0f * SampleMain(uv + float2(PIXEL_X, 0)) +
                    1.0f * SampleMain(uv + float2(PIXEL_X, PIXEL_Y))
                ) / 4.0;
            }

            float3 SobelV(float2 uv)
            {
                return (
                    -1.0f * SampleMain(uv + float2(-PIXEL_X, -PIXEL_Y)) +
                    -2.0f * SampleMain(uv + float2(0, -PIXEL_Y)) +
                    -1.0f * SampleMain(uv + float2(PIXEL_X, -PIXEL_Y)) +

                    1.0f * SampleMain(uv + float2(-PIXEL_X, PIXEL_Y)) +
                    2.0f * SampleMain(uv + float2(0, PIXEL_Y)) +
                    1.0f * SampleMain(uv + float2(PIXEL_X, PIXEL_Y))
               ) / 4.0;   
            }

            float3 StructureTensor(float2 uv)
            {
                float3 u = SobelU(uv);
                float3 v = SobelV(uv);

                return float3(dot(u,u), dot(v,v), dot(u,v));
            }

            float3 BlurredStructureTensor(float2 uv, float sigma)
            {
                float doubleSigmaSquared = 2.0f * sigma * sigma;
                int halfWidth = ceil(2 * sigma);

                float3 col = float3(0,0,0);
                float norm = 0;
                for (int i = -halfWidth; i <= halfWidth; i++)
                    for (int j = -halfWidth; j <= halfWidth; j++)
                    {
                        float distance = sqrt(i*i + j*j);
                        float k = exp(-distance * distance / doubleSigmaSquared);

                        col += StructureTensor(uv + float2(i * PIXEL_X, j * PIXEL_Y)) * k;
                        norm += k;
                    }

                return col / norm;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 t = BlurredStructureTensor(i.uv, 2.0f);
                
                float lambda1 = 0.5f * (t.x + t.y + sqrt((t.x - t.y) * (t.x - t.y) + 4.0f * t.z * t.z));
                float lambda2 = 0.5f * (t.x + t.y - sqrt((t.x - t.y) * (t.x - t.y) + 4.0f * t.z * t.z));
                
                float2 direction = float2(lambda1 - t.x, -t.z);
                direction = (length(direction) > 0.0) ? normalize(direction) : float2(0, 1);
                
                float angle = atan2(direction.y, direction.x);
                
                float anisotropy = (lambda1 + lambda2 <= 0.0) ? 0.0 : (lambda1 - lambda2) / (lambda1 + lambda2);
                
                return half4(direction, angle, anisotropy);
            }

            #pragma vertex vert
            #pragma fragment frag
            
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}
