///====================================================================================================
///
///     AlphaOverlay by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/PostProcess/AlphaOverlay"
{
	Properties
	{
		[HideInInspector]_MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Alpha Mask", 2D) = "white" {}
		_MaskStrength ("Alpha Mask Strength", Range(0, 1)) = 1.0
		_MaskTex ("Mask Texture", 2D) = "white" {}
		_MaskCol ("Mask Color", Color) = (1, 1, 1, 1)
		_MaskBalance ("Mask Color-Texture Balance", Range(0, 1)) = 1.0
		[Toggle] _Invert ("Invert Mask", Int) = 0.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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
			sampler2D _MaskTex;
			sampler2D _Mask;

			float4 _MaskCol;

			float _MaskBalance;
			float _MaskStrength;

			int _Invert;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed4 maskTex = tex2D(_MaskTex, i.uv);
				fixed4 mask = tex2D(_Mask, i.uv);

				return lerp(lerp(_MaskCol, maskTex, _MaskBalance), renderTex, clamp(_Invert ? 1.0f - (mask.r + _MaskStrength) : (mask.r + _MaskStrength), 0, 1));
			}
			ENDCG
		}
	}
}
