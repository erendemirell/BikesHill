Shader "Surface/Specular fast"
{
	Properties
	{
		_Color("Color" , Color) = (1,1,1,1)
		_SpecColor("Specular Color",Color) = (1,1,1,1)
		_Shininess("Shininess" , float) = 10
		_Ambient("Ambient",Range(0.5,2.5)) = 2
		_Atten("atten",Range(0,2)) = 1
		_Directional("Directional Light",Range(0,2)) = 0 
	}

	Subshader
	{
		Tags {"LightMode" = "ForwardBase"}
		
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			
			// user defined
			uniform float4 _Color;
			uniform float4 _SpecColor;
			uniform float _Shininess;
			uniform float _Ambient;
			uniform float _Atten;
			uniform float _Directional;
			// unity defined
			uniform float4 _LightColor0;

			// Structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};

			// verts

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;

				// vectors
				float3 normalDirection = normalize(mul(float4(v.normal,0),unity_WorldToObject).xyz) * _Directional;
				float3 viewDirection = normalize(float3(float4(_WorldSpaceCameraPos.xyz,1) - mul(unity_ObjectToWorld,v.vertex).xyz));
				float3 lightDirection;
				
				// Lighting
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 difuseReflection = _LightColor0.xyz * max(0,dot(normalDirection,lightDirection));
				float3 specularReflection =_Atten * _SpecColor.rgb *  pow(max(0 ,dot( reflect(-lightDirection,normalDirection),viewDirection)),_Shininess);								
				float3 lightFinal = difuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT * _Ambient;

				o.col = float4(lightFinal * _Color.rgb,1);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(vertexOutput o) : COLOR
			{
				return o.col; 
			}


			ENDCG
		}
	}

}