
Shader "Surface/Lambert Texture"
{
	Properties
	{
		_Color ("Color" , Color) = (0,0,0,0)
		_Ambient("Ambient intensity",Range(0,4)) = 1
		_Directional("Directional Light",Range(0,4)) = 1 
		_MainTex("Main Texture" , 2D) = "white" {}
	}

	Subshader
	{

		Pass
		{
			Lighting Off

			Tags{"RenderType" = "Opaque"}
			LOD 100

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag Lambert
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			#pragma target 2.0
			
			#include "UnityCG.cginc"

			// user defined
			uniform sampler2D _MainTex;
			uniform half4 _Color;
			uniform half _Ambient;
			uniform half _Directional;

			// unity defined
			uniform float4 _LightColor0;
			uniform half4 _MainTex_ST;
			
			// structs
			struct vertexInput
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half2 texCoord0 : TEXCOORD0;
				half2 texCoord1 : TEXCOORD1;
			};
		
			struct vertexOutput
			{
				half4 pos : SV_POSITION;
				half4 col : COLOR;
				half2 uv0 :TEXCOORD0;
				half2 uv1 :TEXCOORD1;
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				half3 normalDirection = normalize(mul(half4(v.normal,0), unity_WorldToObject).xyz);
				half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz)*_Directional;
				half3 difuseReflection = _LightColor0 * max(0,dot(normalDirection, lightDirection));
				half3 lightFinal = difuseReflection + UNITY_LIGHTMODEL_AMBIENT.xyz * _Ambient;
				   
				o.uv0 = TRANSFORM_TEX(v.texCoord0,_MainTex); 
				o.uv1 = v.texCoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				
				o.col = half4(lightFinal * _Color.rgb ,1);
				o.pos = UnityObjectToClipPos(v.vertex);
	
				return o;
			}

			float4 frag(vertexOutput i) : COLOR
			{
				#ifdef LIGHTMAP_OFF
				half4 tex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1.xy) * tex2D(_MainTex, i.uv0) * i.col;
				#else
				half4 tex = tex2D(_MainTex, i.uv0) * i.col;
				#endif
				return tex;
			}
	
			ENDCG
		}

		// GÖLGE
		Pass
		{
			Tags {"LightMode" = "ShadowCaster"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}

	}
}