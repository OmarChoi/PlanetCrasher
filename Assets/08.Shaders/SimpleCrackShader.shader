Shader "Custom/SimpleCrackShader"
{
    Properties
    {
        _BaseImage ("Base Image", 2D) = "white" {}
        _NoiseImage ("Noise Image", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _CrackColor ("Crack Color", Color) = (0, 0, 0, 1)
        _Progress ("Crack Progress", Range(0,1)) = 0
        _CrackStart ("Crack Start", Range(0,1)) = 0.1
        _CrackSoftness ("Crack Softness", Range(0,0.5)) = 0.05
        _HeatColor ("Heat Color", Color) = (0.9, 0.1, 0, 0.5)
        _HeatStart ("Heat Start", Range(0,1)) = 0.85
        _HeatPulseSpeed ("Heat Pulse Speed", Range(0,30)) = 10
        _HeatPulseStrength ("Heat Pulse Strength", Range(0,1)) = 0.3
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
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_BaseImage);
            SAMPLER(sampler_BaseImage);
            TEXTURE2D(_NoiseImage);
            SAMPLER(sampler_NoiseImage);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _CrackColor;
                float _Progress;
                float _CrackStart;
                float _CrackSoftness;
                float4 _HeatColor;
                float _HeatStart;
                float _HeatPulseSpeed;
                float _HeatPulseStrength;
            CBUFFER_END


            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = posInputs.positionCS;
                output.uv = input.uv;
                output.color = input.color;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 baseTexture = SAMPLE_TEXTURE2D(_BaseImage, sampler_BaseImage, input.uv);
                half noiseValue = SAMPLE_TEXTURE2D(_NoiseImage, sampler_NoiseImage, input.uv).r;

                half4 baseColor = baseTexture * _BaseColor * input.color;

                half progressRange = max(1.0 - _CrackStart, 1e-4);
                half effectiveProgress = saturate((_Progress - _CrackStart) / progressRange);

                half softness = max(_CrackSoftness, 1e-4);
                // threshold를 softness만큼 오버슈트시켜 progress 0 → 균열 0, progress 1 → 완전 균열 보장
                half threshold = lerp(-softness, 1.0 + softness, effectiveProgress);
                half crackMask = smoothstep(noiseValue - softness, noiseValue + softness, threshold);

                half4 finalColor = lerp(baseColor, _CrackColor, crackMask);

                // 폭발 직전 점점 빨갛게 달아오르는 연출
                // _HeatColor.a를 전체 강도(master strength)로 사용 → 0이면 효과 없음, 1이면 최대
                half heat = saturate((_Progress - _HeatStart) / max(1.0 - _HeatStart, 1e-4)) * _HeatColor.a;
                half pulse = heat * heat * (0.5 + 0.5 * sin(_Time.y * _HeatPulseSpeed));

                // 어두운 행성에서 빨강이 과포화되지 않도록 베이스 밝기에 비례해 발광 적용
                // (밝은 행성: 베이스와 섞여 주황/흰빛, 어두운 행성: 거의 안 달아오름)
                half baseLum = dot(baseColor.rgb, half3(0.299, 0.587, 0.114));
                finalColor.rgb += _HeatColor.rgb * (heat + pulse * _HeatPulseStrength) * baseLum;

                finalColor.a = baseColor.a;

                return finalColor;
            }

            ENDHLSL
        }
    }
}
