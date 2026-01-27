Shader "Custom/SimpleCrackShader"
{
    Properties
    {
        _BaseImage ("Base Image", 2D) = "white" {}
        _NoiseImage ("Noise Image", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _CrackColor ("Crack Color", Color) = (0, 0, 0, 1)
        _Progress ("Crack Progress", Range(0,1)) = 0
    }
    
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            TEXTURE2D(_BaseImage);
            SAMPLER(sampler_BaseImage);
            TEXTURE2D(_NoiseImage);
            SAMPLER(sampler_NoiseImage);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _CrackColor;
                float _Progress;
            CBUFFER_END
            
            
            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = posInputs.positionCS;
                output.uv = input.uv;

                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                half4 baseTexture = SAMPLE_TEXTURE2D(_BaseImage, sampler_BaseImage, input.uv);
                float noiseValue = SAMPLE_TEXTURE2D(_NoiseImage, sampler_NoiseImage, input.uv).r;

                half4 baseColor = baseTexture * _BaseColor;

                float effectiveProgress = saturate((_Progress - 0.1) / 0.9);
                float crackMask = effectiveProgress > noiseValue ? 1.0 : 0.0;
                half4 finalColor = lerp(baseColor, _CrackColor, crackMask);

                return finalColor;
            }
            
            ENDHLSL
        }
    }
}