///====================================================================================================
///
///     CircleTransition by
///     - CantyCanadian
///		- ellioman
///
///		Make sure to use the TransitionPostProcessShader script instead of the regular PostProcessShader script.
///
///====================================================================================================
Shader "Custom/PostProcess/Transition/Circle"
{
	Properties
	{
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
		_TransitionTex ("Transition Texture", 2D) = "white" {}
		_Color ("Transition Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_TextureColor ("Texture-Color Amount", Range(0.0, 1.0)) = 0.0
		_CenterX ("Center Point X", Range(0.0, 1.0)) = 0.5
		_CenterY ("Center Point Y", Range(0.0, 1.0)) = 0.5
		[Toggle] _Reverse ("Reverse Effect", int) = 0
	}
	SubShader
	{
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 color : COLOR;
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _TransitionTex;

			float4 _Color;

			float2 _ScreenResolution;

			float _CenterX;
			float _CenterY;
			float _TransitionValue;
			float _TextureColor;

			int _Reverse;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float4 tex = tex2D(_MainTex, i.uv);
				float4 transitionTex = tex2D(_TransitionTex, i.uv);
				float4 final = lerp(transitionTex, _Color, _TextureColor);

				float2 center = float2(_CenterX, _CenterY) * _ScreenResolution;
				
				float width = _ScreenResolution.x + (abs(_CenterX * 2.0f - 1.0f) * _ScreenResolution.x);
				float height = _ScreenResolution.y + (abs(_CenterY * 2.0f - 1.0f) * _ScreenResolution.y);
				float radius = 0.5f * sqrt(width * width + height * height);

				float newTransitionValue = _Reverse ? _TransitionValue : 1.0f - _TransitionValue;

				float value = clamp(distance(center.xy, (i.uv.xy * _ScreenResolution.xy)) - (radius * newTransitionValue) + 1, 0.0, 1.0);

				if (_Reverse == 0)
				{
					value = 1.0f - value;
				}

				return lerp(final, tex, value);
			}
			ENDCG
		}
	}
}
