// Original shader found here : https://github.com/JPBotelho/Unity-Camera-cutout, modified by @CantyCanadian on Twitter.
Shader "Camera/Canty/Cutout"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_Mask ("Cutout", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Alpha ("Alpha", Range(0.0, 1.0)) = 1.0
	}
	SubShader
	{
		Cull Off 
		ZWrite Off 
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _Mask;

			float4 _Color;

			float _Alpha;

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				float4 mask = tex2D(_Mask, i.uv);

				float4 colorNew = lerp(color, _Color, _Alpha);

				color = (mask == float4(0,0,0,0)) ? colorNew : color;

				return color;
			}

			ENDCG
		}
	}
}
