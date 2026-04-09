Shader "Unlit/TracerShader"
{
     Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            ZWrite On
            Cull Back
            Lighting Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5 // required for StructuredBuffer
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;

            StructuredBuffer<float4x4> _Matrices;

            struct appdata
            {
                float3 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(appdata v, uint id : SV_InstanceID)
            {
                v2f o;
                float4x4 m = _Matrices[id];
                float4 worldPos = mul(m, float4(v.vertex, 1.0));
                o.pos = UnityObjectToClipPos(worldPos);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv) * _Color;
            }
            ENDHLSL
        }
    }
}
