Shader "Custom/PostProcess/Offset"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_OffsetX ("Offset X", Float) = 0.0
		_OffsetY ("Offset Y", Float) = 0.0
		_ZoomX ("Zoom X", Float) = 1.0
		_ZoomY ("Zoom Y", Float) = 1.0
		_Rotation ("Rotation", Range(0.0, 180.0)) = 0.0
		[Toggle]_RotateAfterOffset ("Rotate after Offset", int) = 0
		[Enum(Clamp, 0, Solid, 1, Repeat, 2, Mirror, 3)] _Tiling ("Offset Tiling", Int) = 0
		_Color ("Solid Color", Color) = (0.0, 0.0, 0.0, 0.0)
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
			#include "../../CGIncludes/AngleMath.cginc"

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

			float4 _Color;

			float _OffsetX;
			float _OffsetY;
			float _ZoomX;
			float _ZoomY;
			float _Rotation;

			int _RotateAfterOffset;
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
				float2 positiveOffset = abs(float2(_OffsetX, _OffsetY));
				float2 positiveZoom = abs(float2(_ZoomX, _ZoomY));

				float2 newUV = (i.uv + positiveOffset) / positiveZoom;

				if (_RotateAfterOffset == 0)
				{
					newUV = (newUV * 2.0f) - 1.0f;
					newUV = RotateAroundCenter(newUV, _Rotation);
					newUV = (newUV + 1.0f) / 2.0f;
				}

				switch(_Tiling)
				{
					case 0:
						newUV = clamp(newUV, 0, 1);
					break;

					case 1:
						if (IsOutside11Range(newUV))
						{
							return _Color;
						}
					break;

					case 2:
						newUV = RepeatUV(newUV);
					break;

					case 3:
						newUV = MirrorUV(newUV);
					break;
				}

				if (_RotateAfterOffset == 1)
				{
					newUV = (newUV * 2.0f) - 1.0f;
					newUV = RotateAroundCenter(newUV, _Rotation);
					newUV = (newUV + 1.0f) / 2.0f;
				}

				return tex2D(_MainTex, newUV);
			}
			ENDCG
		}
	}
}
