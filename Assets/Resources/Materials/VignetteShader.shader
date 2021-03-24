Shader "RectDev/Vignette"
{
    Properties
    {
        overallAlpha("Overall Alpha", Range(0, 1)) = 1
        alphaMax("Maximum Alpha", Range(0, 1)) = 1
        alphaMin("Minimum Alpha", Range(0, 1)) = 0
        speed("Speed", Range(0.5, 10)) = 1
        [Toggle] insideOut("Inside Out", Float) = 0
        vignetteColor("Warning Color", Color) = (1,1,1,1)
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

            fixed4 vignetteColor;

            fixed alphaMax;
            fixed alphaMin;
            fixed overallAlpha;
            fixed speed;
            bool insideOut;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float centerDistX = i.uv.x - 0.5;
                float centerDistY = i.uv.y - 0.5;

                float distSqrd = centerDistX * centerDistX + centerDistY * centerDistY;
                
                float alpha = pow(distSqrd, speed);
                if (insideOut) alpha = 1 - alpha;
                alpha = alpha * (alphaMax - alphaMin) + alphaMin;

                vignetteColor.a = alpha * overallAlpha;

                return vignetteColor;
            }
            ENDCG
        }
    }
}
