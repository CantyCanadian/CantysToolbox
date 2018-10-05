///====================================================================================================
///
///     CrissCrissTransition by
///     - CantyCanadian
///		- ellioman
///
///		Make sure to use the TransitionPostProcessShader script instead of the regular PostProcessShader script.
///
///====================================================================================================
Shader "Custom/PostProcess/Transition/CrissCross"
{
	Properties
	{
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
		_TransitionTex ("Transition Texture", 2D) = "white" {}
		_Color ("Transition Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_TextureColor ("Texture-Color Amount", Range(0.0, 1.0)) = 0.0
		_BarCount ("Bar Count", int) = 100
		_Angle ("Angle", Range(0.0, 180.0)) = 0.0
		[Toggle] _PushImage("Push Background Image", int) = 0
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
			#include "../../../CGIncludes/AngleMath.cginc"
			#include "../../../CGIncludes/SquareMath.cginc"

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
			int _PushImage;

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
				float radAngle = radians(_Angle);
				float2 newUV = i.uv - float2(0.5f, 0.5f);
				float2 newPoint = RotateAroundCenter(newUV, _Angle) + 0.5f;

				float barSize = 2.0f / max(1, _BarCount);
				
				if (newPoint.y < 0.0f)
				{
					newPoint.y = abs(newPoint.y) + (barSize / 2.0f);
				}

				int side = step(newPoint.y % barSize, barSize / 2.0f);

				float endDistance = ImperfectSquareLength(_Angle);
				float newTransitionValue = lerp(-(endDistance - 1.0f), endDistance, _TransitionValue);
				float value = side ? newPoint.x >= newTransitionValue : newPoint.x <= 1.0f - newTransitionValue;

				float sideAngle = side ? _Angle : _Angle + 180.0f;
				float2 direction = float2(-cos(radians(sideAngle)), sin(radians(sideAngle)));
				float2 pushUV = i.uv + (lerp(0.0f, 1.0f + endDistance / 2.0f, _TransitionValue) * direction);

				float4 tex = tex2D(_MainTex, _PushImage ? pushUV : i.uv);
				float4 transitionTex = tex2D(_TransitionTex, i.uv);
				float4 final = lerp(transitionTex, _Color, _TextureColor);
				
				return lerp(final, tex, value);
			}
			ENDCG
		}
	}
}
