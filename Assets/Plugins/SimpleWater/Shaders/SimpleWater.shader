Shader "Custom/SimpleWater"
{
    Properties
    {
        _WaterColor ("Water Color", Color) = (1,1,1,1)
        _WaterTex ("Water Texture", 2D) = "white" {}
        _FlowDirectionX ("Flow Direction X", float) = 0
        _FlowDirectionY ("Flow Direction Y", float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float fogCoord : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            half4 _WaterColor;
            float _FlowDirectionX;
            float _FlowDirectionY;

            TEXTURE2D(_WaterTex);
            SAMPLER(sampler_WaterTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _WaterTex_ST;
            CBUFFER_END

            float repeat(float v, float length)
            {
                int a = v / length;
                if (v < 0)
                    a += 1;
                return v - a * length;
            }

            v2f vert(appdata IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = vertexInput.positionCS;
                OUT.positionWS = vertexInput.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _WaterTex);
                OUT.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);

                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                IN.uv.x = repeat(IN.uv.x + _Time.x * _FlowDirectionX, 1);
                IN.uv.y = repeat(IN.uv.y + _Time.x * _FlowDirectionY, 1);

                half4 color = SAMPLE_TEXTURE2D(_WaterTex, sampler_WaterTex, IN.uv);
                color = lerp(_WaterColor, half4(0.3, 0.3, 0.3, 0.01), color.r);

                VertexPositionInputs vertexInput = (VertexPositionInputs)0;
                vertexInput.positionWS = IN.positionWS;

                //float4 shadowCoord = GetShadowCoord(vertexInput);
                //half shadowAttenuation = MainLightRealtimeShadow(shadowCoord);
                //color = lerp(half4(0, 0, 0, 1), color, shadowAttenuation);

                color.rgb = MixFogColor(color.rgb, half3(1, 1, 1), IN.fogCoord);
                return color;
            }

            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
