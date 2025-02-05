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
        _LightAngle ("Light Angle", Range(-35, 35)) = 0
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
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.positionOS).xyz;
                OUT.localPos = IN.uv;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.localPos) * IN.color;
                
                float2 position;
                float angle = radians(_LightAngle);
                // 음수/양수 각도에 따른 방향 벡터 계산
                float2 lightDir = float2(
                    cos(angle),
                    sin(angle)
                );
                float2 parentUV;
                
                if (_UseScreenCoordinates > 0.5)
                {
                    position = IN.positionHCS.xy;
                    float2 relativePos = (position - _ParentPosition.xy);
                    parentUV = relativePos / _ParentRect.xy;
                    parentUV.y = 1.0 - parentUV.y;
                }
                else
                {
                    position = IN.worldPos.xy;
                    parentUV = (position - _ParentPosition.xy) / _ParentRect.xy;
                }
                
                // 각도의 방향성을 유지하면서 투영 계산
                float projection = dot(parentUV, lightDir);
                float lightEffect = 1 - saturate(abs(projection - _Progress) / _LightWidth);
                float4 lightColor = _LightColor * lightEffect * _LightIntensity;
                
                color.rgb += lightColor.rgb * color.a;
                
                return color;
            }
            ENDHLSL
        }
    }
}