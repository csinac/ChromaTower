Shader "RectDev/FadeToTransparent"
{
    Properties
    {
        alphaMax("Maximum Alpha", Range(0, 1)) = 1
        alphaMin("Minimum Alpha", Range(0, 1)) = 0
        speed("Speed", Range(1, 10)) = 1
        [Toggle] bottomUp("Bottom Up", Float) = 0
        gradientColor("Gradient Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 gradientColor;

            fixed alphaMax;
            fixed alphaMin;
            fixed speed;
            bool bottomUp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float gradientPos = bottomUp ? 1 - i.uv.y : i.uv.y;
                gradientPos = pow(gradientPos, speed);
                float alpha = alphaMin + (alphaMax - alphaMin) * gradientPos;
    
                gradientColor.a = alpha;

                return gradientColor;
            }
            ENDCG
        }
    }
}
