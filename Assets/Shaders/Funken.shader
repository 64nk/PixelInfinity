Shader "PixelInfinity/Funken"
{
    Properties
    {
        [HDR] _Farbe ("Farbe (HDR fuer Bloom)", Color) = (4, 1.5, 0.3, 1)
        _Weichheit ("Weichheit des Randes", Range(0.01, 1)) = 0.6
        _KernHelligkeit ("Helligkeit des Kerns", Range(0, 5)) = 2
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha One
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _Farbe;
                half  _Weichheit;
                half  _KernHelligkeit;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {

                float abstand = distance(IN.uv, float2(0.5, 0.5)) * 2.0;


                float kreis = 1.0 - smoothstep(1.0 - _Weichheit, 1.0, abstand);


                float kern = 1.0 - smoothstep(0.0, 0.3, abstand);

                half4 farbe = _Farbe * IN.color;
                farbe.rgb += kern * _KernHelligkeit;
                farbe.a = kreis * IN.color.a;
                return farbe;
            }
            ENDHLSL
        }
    }
}
