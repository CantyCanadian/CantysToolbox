Shader "Custom/PostProcess/ColorOffset"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}

		_RedOffsetX ("Red Offset X", Float) = 0.0
		_RedOffsetY ("Red Offset Y", Float) = 0.0

		_GreenOffsetX ("Green Offset X", Float) = 0.0
		_GreenOffsetY ("Green Offset Y", Float) = 0.0

		_BlueOffsetX ("Blue Offset X", Float) = 0.0
		_BlueOffsetY ("Blue Offset Y", Float) = 0.0

		[Enum(Clamp, 0, Repeat, 1, Mirror, 2)] _Tiling ("Offset Tiling", Int) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "../../CGIncludes/SquareMath.cginc"

			struct appdata
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			float _RedOffsetX;
			float _RedOffsetY;
			float _RedZoomX;
			float _RedZoomY;
			float _GreenOffsetX;
			float _GreenOffsetY;
			float _GreenZoomX;
			float _GreenZoomY;
			float _BlueOffsetX;
			float _BlueOffsetY;
			float _BlueZoomX;
			float _BlueZoomY;

			int _Tiling;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				float2 rPositiveOffset = float2(-_RedOffsetX, -_RedOffsetY);
				float2 gPositiveOffset = float2(-_GreenOffsetX, -_GreenOffsetY);
				float2 bPositiveOffset = float2(-_BlueOffsetX, -_BlueOffsetY);

				float2 newUVr = i.uv + rPositiveOffset;
				float2 newUVg = i.uv + gPositiveOffset;
				float2 newUVb = i.uv + bPositiveOffset;

				switch(_Tiling)
				{
					case 0:
						newUVr = clamp(newUVr, 0, 1);
						newUVg = clamp(newUVg, 0, 1);
						newUVb = clamp(newUVb, 0, 1);
					break;

					case 1:
						newUVr = RepeatUV(newUVr);
						newUVg = RepeatUV(newUVg);
						newUVb = RepeatUV(newUVb);
					break;

					case 2:
						newUVr = MirrorUV(newUVr);
						newUVg = MirrorUV(newUVg);
						newUVb = MirrorUV(newUVb);
					break;
				}

				float4 r = tex2D(_MainTex, newUVr);
				float4 g = tex2D(_MainTex, newUVg);
				float4 b = tex2D(_MainTex, newUVb);

				return float4(r.r, g.g, b.b, 1.0f);
			}
			ENDCG
		}
	}
}
