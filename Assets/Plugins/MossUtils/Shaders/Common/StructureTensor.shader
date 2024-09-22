Shader "Hidden/MossUtils/StructureTensor"
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

            half4 frag(Varyings i) : SV_Target
            {
                return half4(StructureTensor(i.uv), 0);
            }

            #pragma vertex vert
            #pragma fragment frag
            
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}
