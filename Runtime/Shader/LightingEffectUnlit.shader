Shader "Custom/LightingEffectUnlit"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _LightWidth ("Light Width", Range(0, 1)) = 0.1
        _LightIntensity ("Light Intensity", Range(0, 5)) = 1
        _Progress ("Progress", Range(-1, 2)) = 0
        _LightAngle ("Light Angle", Range(-89, 89)) = 0
        _ParentRect ("Parent Rect", Vector) = (100,100,0,0)
        _ParentPosition ("Parent Position", Vector) = (0,0,0,0)
        _UseScreenCoordinates ("Use Screen Coordinates", Float) = 0
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline" = "UniversalPipeline"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        Pass
        {
            Name "Universal2D"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #pragma vertex vert
            #pragma fragment frag

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
                float2 localPos : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _LightColor;
                float _LightWidth;
                float _LightIntensity;
                float _Progress;
                float _LightAngle;
                float4 _ParentRect;
                float4 _ParentPosition;
                float _UseScreenCoordinates;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                // 변환: 오브젝트 공간 -> 월드 공간 -> HClip
                // TODO: HClip을 사용하면 렌더링이 깨지는 경우가 있음
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.positionOS).xyz;
                OUT.localPos = IN.uv;  // 기존 UV (필요시 참고용)
                OUT.color = IN.color * _Color;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // 기본 텍스쳐 색상
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.localPos) * IN.color;
                
                float2 position;
                if (_UseScreenCoordinates > 0.5)
                {
                    // Overlay 모드: 스크린 좌표계 사용
                    position = IN.positionHCS.xy;
                }
                else
                {
                    // Camera 모드: 월드 좌표계 사용
                    position = IN.worldPos.xy;
                }
                
                // 부모 기준 UV 계산
                float2 parentUV = (position - _ParentPosition.xy) / _ParentRect.xy;
                
                // 조명 계산
                float angle = radians(_LightAngle);
                float2 lightDir = float2(cos(angle), sin(angle));
                float projection = dot(parentUV, lightDir);
                
                float lightEffect = 1 - saturate(abs(projection - _Progress) / _LightWidth);
                float4 lightColor = _LightColor * lightEffect * _LightIntensity;
                
                // 최종 색상 계산
                color.rgb += lightColor.rgb * color.a;
                
                return color;
            }
            ENDHLSL
        }
    }
}