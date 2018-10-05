///====================================================================================================
///
///     BlendModeScreen by
///     - CantyCanadian
///		- ellioman
///		- AlanZucconi
///
///====================================================================================================
Shader "Custom/PostProcess/BlendMode/Screen"
{
	Properties
	{
		[HideInInspector]_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlendTex ("Blend Texture (RGB)", 2D) = "white" {}
		_Opacity ("Blend Opacity", Range(0.0, 1.0)) = 1.0
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
			sampler2D _BlendTex;

			float _Opacity;

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
				fixed4 blendTex = tex2D(_BlendTex, i.uv);
					
				fixed4 blendedScreen = (1.0f - ((1.0f - renderTex) * (1.0f - blendTex)));
					
				renderTex = lerp(renderTex, blendedScreen, _Opacity);
				return renderTex;
			}
			ENDCG
		}
	}
}
