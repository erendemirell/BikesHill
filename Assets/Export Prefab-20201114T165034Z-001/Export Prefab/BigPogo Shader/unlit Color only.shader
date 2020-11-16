Shader "Surface/Deneme Unlit"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
	}

	Subshader
	{
		Pass
		{
			Tags{"Lightmode" = "ForwardBase"}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform half4 _Color;
			
			// structs
			struct vertexInput
			{
				half4 vertex : POSITION;
			};
		
			struct vertexOutput
			{
				half4 pos : SV_POSITION;
			};

			//  function
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(vertexOutput i) : COLOR
			{
				return _Color;
			}

			ENDCG
		}
	}
}
