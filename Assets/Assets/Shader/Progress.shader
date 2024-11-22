Shader "Custom/RadialFill"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 1.0
        _Center ("Fill Center", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Name "RadialFillPass"
            Tags { "LightMode"="UniversalForward" }
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION; // Object Space Position
                float2 uv : TEXCOORD0;       // UV Coordinates
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // Homogeneous Clip Space Position
                float2 uv : TEXCOORD0;           // Interpolated UV
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _FillAmount;
            float2 _Center;

            // Định nghĩa giá trị PI
            #define UNITY_PI 3.14159265359

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // Tính toán góc từ UV tới tâm
                float2 dir = uv - _Center;
                float angle = atan2(dir.y, dir.x) / UNITY_PI; // Góc trong khoảng [-1, 1]
                angle = (angle + 1.0) * 0.5;                  // Chuẩn hóa về [0, 1]

                // Kiểm tra Fill Amount
                if (angle > _FillAmount)
                    discard;

                // Trả về màu từ texture
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            }
            ENDHLSL
        }
    }
}
