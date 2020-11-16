
Shader "Surface/Rim"
{
	Properties
	{
		[Header(Lightings)] [Space]
		_Color("Color",Color) = (0,0,0,0)
		_SpecColor("Specular Color",Color) = (1,1,1,1)
		_Shininess("Shininess" , float) = 10
		_Ambient("Ambient",Range(0.5,2.5)) = 2
		_Atten("atten",Range(0,2)) = 1
		_Directional("Directional Light",Range(0,2)) = 1 
		[Header(Rim)] [Space]
		_RimPower("RimPower",Range(0,10)) = 1
		_RimColor("RimColor",Color) = (0,0,0,0)
		
	}

	SubShader
	{
		pass
		{
			Tags{"Lightmode" = "ForwardBase"}

			CGPROGRAM

			// pragmas
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			// user defined
			uniform float4 _Color;	
			uniform float4 _RimColor;
			uniform float4 _SpecColor;
			uniform float _Shininess;
			uniform float _Ambient;
			uniform float _Atten;
			uniform float _Directional;
			uniform float _RimPower;
			
			// unity defined
			uniform float4 _LightColor0;

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

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.pos = UnityObjectToClipPos(i.vertex);
				// vectors
				float4 posWorld = mul(unity_ObjectToWorld,i.vertex);
				float3 normalDir = normalize(mul(float4(i.normal,0),unity_WorldToObject).xyz) * _Directional;
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - o.pos.xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				// lighting
			    float3 difuseReflection = _Atten * _LightColor0 * _Color.xyz * saturate(dot(normalDir,lightDirection));
				float3 specularReflection = _Atten * _LightColor0.xyz * _SpecColor.xyz * saturate(dot(normalDir,lightDirection)) * pow(saturate(dot(reflect(-lightDirection,normalDir),viewDirection)),_Shininess);
				//Rim Lighting
				float rim = 1 - saturate(dot(normalize(viewDirection),normalDir));
				float3 rimLighting = _Atten * _LightColor0.xyz * _RimColor * saturate(dot(normalDir,lightDirection)) * pow(rim,_RimPower);
				float3 lightFinal = rimLighting + difuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT.xyz * _Ambient;

				o.col = float4(lightFinal * _Color , 1);

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
