// Make sure to use the CrissCrossTransitionPostProcessShader script instead of the regular PostProcessShader script.
Shader "Custom/PostProcess/Transition/CrissCross"
{
	Properties
	{
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
		_TransitionTex ("Transition Texture", 2D) = "white" {}
		_Color ("Transition Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_TextureColor ("Texture-Color Amount", Range(0.0, 1.0)) = 0.0
	}
	SubShader
	{
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#pragma glsl

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

			float _TransitionValue;
			float _TextureColor;
			float _Angle;

			int _BarCount;
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

				float barSize = 1.0f / max(_BarCount, 1);

				float value = (i.uv.y / barSize) % 2 == 0 ? 1.0f : 0.0f;
				
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
