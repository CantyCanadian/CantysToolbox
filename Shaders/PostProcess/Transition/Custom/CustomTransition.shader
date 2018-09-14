// Make sure to use the CustomTransitionPostProcessShader script instead of the regular PostProcessShader script.
// Packaged transition graphics are made by Makin' Stuff Look Good on Youtube : https://www.youtube.com/channel/UCEklP9iLcpExB8vp_fWQseg
// Minus the Diamonds texture made by stellarNull on Twitter :  https://twitter.com/stellarNull 
Shader "Custom/PostProcess/Transition/Custom"
{
	Properties
	{
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
		_TransitionTex ("Transition Texture", 2D) = "white" {}
		_Color ("Transition Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_TextureColor ("Texture-Color Amount", Range(0.0, 1.0)) = 0.0
		_Blur ("Texture Blur", Range(0.0, 1.0)) = 0.05
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
				float2 uv1 : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _TransitionTex;
			sampler2D _TransitionPattern;

			float4 _MainTex_TexelSize;
			float4 _Color;

			float _Transition;
			float _TextureColor;
			float _Blur;

			int _Reverse;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.uv1 = v.uv;
                o.color = v.color;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
				{
					o.uv1.y = 1 - o.uv1.y;
				}
				#endif

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

				if (transitionPattern >= _Transition + _Blur)
				{
					value = 1.0f;
				}
				else if (transitionPattern >= _Transition)
				{
					float blurLevel = min(_Blur, _Transition);

					if (blurLevel > 0.0f)
					{
						float percent = (transitionPattern - _Transition) / blurLevel;

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
