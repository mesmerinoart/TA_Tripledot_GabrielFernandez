Shader "UI/BlurPanel"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 1.8)) = 2.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        GrabPass { "_GrabTexture" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _BlurSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = o.pos.xy / o.pos.w;
                o.uv = o.uv * 0.5f + 0.5f;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
        {
            float2 uv = float2(i.uv.x, 1.0 - i.uv.y);
            float2 texel = _GrabTexture_TexelSize.xy * _BlurSize;

            fixed4 col = tex2D(_GrabTexture, uv) * 0.227027f;

         col += tex2D(_GrabTexture, uv + texel * 1.384615f) * 0.316216f;
         col += tex2D(_GrabTexture, uv - texel * 1.384615f) * 0.316216f;

            col += tex2D(_GrabTexture, uv + texel * 3.230769f) * 0.070270f;
            col += tex2D(_GrabTexture, uv - texel * 3.230769f) * 0.070270f;

    return col * 0.6;
        }
            ENDCG
        }
    }
}
