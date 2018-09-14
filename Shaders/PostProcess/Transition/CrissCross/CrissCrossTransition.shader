// Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
// Make sure to use the BarsTransitionPostProcessShader script instead of the regular PostProcessShader script.
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

				float radAngle = radians(_Angle);
				float2 newUV = i.uv - float2(0.5f, 0.5f);
				float2 newPoint = float2(newUV.x * cos(radAngle) - newUV.y * sin(radAngle) + 0.5f, newUV.x * sin(radAngle) + newUV.y * cos(radAngle) + 0.5f);

				float barSize = 2.0f / max(1, _BarCount);
				
				if (newPoint.y < 0.0f)
				{
					newPoint.y = abs(newPoint.y) + (barSize / 2.0f);
				}

				int side = step(newPoint.y % barSize, barSize / 2.0f);

				float endDistance = lerp(0.1f, 0.4142f, 0.5f * cos(4.0f * 3.1416f * (_Angle / 180.0f) + 3.1416f) + 0.5f);
				float newTransitionValue = lerp(-endDistance, 1.0f + endDistance, _TransitionValue);
				float value = side ? newPoint.x >= newTransitionValue : newPoint.x <= 1.0f - newTransitionValue;

				if (_Reverse == 1)
				{
					value = 1.0f - value;
				}
				
				return lerp(final, tex, value);
			}
			ENDCG
		}
	}
}
