Shader "Custom/RevealTransparent"
{
    Properties
    {
        _RevealMask("Reveal Mask", 2D) = "black" {}
        _Threshold("Threshold", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _RevealMask;
            float4 _RevealMask_ST;
            float _Threshold;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _RevealMask);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float mask = tex2D(_RevealMask, i.uv).r;

                // Reveal = 1 → transparent; not revealed = 0 → black
                float alpha = 1 - step(_Threshold, mask); // Invert logic

                return float4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}