Shader "Seraph/Outline Shader"
{
	Properties
	{
		_OutlineColor("Outline color", Color) = (1,0.5,0,1)
		_OutlineWidth("Outlines width", Range(0.0, 0.2)) = 0.06
	}

		CGINCLUDE
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;

	ENDCG

		SubShader
	{
		Tags{ "Queue" = "Transparent+1" "IgnoreProjector" = "True" }

		Pass
		{
			ZWrite Off
			Cull Front

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v)
			{


				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);
				o.pos.xy += offset * o.pos.z * _OutlineWidth;

				return o;

			}

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}


	}
		Fallback "Diffuse"
}