// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Texture-World-Space"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {           
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag               
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.position);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                   
                float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;
    float aspect = _ScreenParams.x / _ScreenParams.y;
    textureCoordinate.x = textureCoordinate.x * aspect;
    textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
    fixed4 col = tex2D(_MainTex, textureCoordinate);
                return col;
            }
            ENDCG
        }
    }
}