///====================================================================================================
///
///     CustomTransition by
///     - CantyCanadian
///		- ellioman
///
///		Make sure to use the TransitionPostProcessShader script instead of the regular PostProcessShader script.
///
///====================================================================================================
Shader "Custom/PostProcess/Transition/Custom"
{
	Properties
	{
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
		_TransitionPattern ("Transition Pattern", 2D) = "white" {}
		_TransitionTex ("Transition Texture", 2D) = "white" {}
		_Color ("Transition Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_TextureColor ("Texture-Color Amount", Range(0.0, 1.0)) = 0.0
		_Blur ("Texture Blur", Range(0.0, 1.0)) = 0.05
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
			sampler2D _TransitionPattern;

			float4 _Color;

			float _TransitionValue;
			float _TextureColor;
			float _Blur;

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

				float transitionPattern = tex2D(_TransitionPattern, i.uv).r;
				float value = 0.0f;
				float alpha = 1.0f;

				float transitionValue = 1.0f - _TransitionValue;

				if (transitionPattern >= transitionValue + _Blur)
				{
					value = 1.0f;
				}
				else if (transitionPattern >= transitionValue)
				{
					float blurLevel = min(_Blur, transitionValue);

					if (blurLevel > 0.0f)
					{
						float percent = (transitionPattern - transitionValue) / blurLevel;

						value = smoothstep(0.0f, 1.0f, percent);
					}
					else
					{
						value = 1.0f;
					}
				}

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
