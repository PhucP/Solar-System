Shader "Custom/LightShader"
{
     Properties {
        _Color ("Light Color", Color) = (1, 1, 1, 1)
        _Radius ("Light Radius", Range(0.1, 50)) = 5.0
    }
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };
            struct v2f {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float _Radius;
            float4 _Color;

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                float3 worldPos = mul(unity_ObjectToWorld, i.pos).xyz;
                float distance = length(worldPos - _WorldSpaceCameraPos);
                float attenuation = 1.0 / (1.0 + 0.25 * distance * distance); // Adjust this formula as needed
                return i.color * attenuation;
            }
            ENDCG
        }
    }
}
