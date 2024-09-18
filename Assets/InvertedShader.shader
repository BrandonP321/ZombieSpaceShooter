Shader "Custom/InvertedShader"
{
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Cull Front   // Inverts the normals by rendering the inside of the object
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha  // Enables transparency
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Semi-transparent gray color (RGBA: 0.5, 0.5, 0.5, 0.5)
                return fixed4(0.5, 0.5, 0.5, 0.5);  // Gray color with 50% transparency
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}