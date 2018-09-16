// Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
// Make sure to use the TransitionPostProcessShader script instead of the regular PostProcessShader script.
Shader "Custom/PostProcess/Transition/Bars"
{
	Properties
	{
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
		_TransitionTex ("Transition Texture", 2D) = "white" {}
		_Color ("Transition Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_TextureColor ("Texture-Color Amount", Range(0.0, 1.0)) = 0.0
		[Toggle]_LockUV ("Lock Texture UV", int) = 0
		_Angle ("Angle", Range(0.0, 180.0)) = 0.0
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

			float4 _ScreenResolution;
			float4 _Color;

			float _TransitionValue;
			float _PositionX;
			float _PositionY;
			float _TextureColor;
			float _Angle;

			int _LockUV;

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
				float2 center = float2(_PositionX * _ScreenResolution.x, _PositionY * _ScreenResolution.y);

				float2 newUV = (i.uv * 2.0f) - float2(1.0f, 1.0f);
				float angle = 1.0f - abs((_Angle - 90.0f) / 90.0f);

				float newTransitionValue = 1.0f - _TransitionValue;
				float cutoff = _Angle <= 90.0f ? lerp(newUV.x, newUV.y, angle) : lerp(-newUV.x, newUV.y, angle);
				float value = cutoff < newTransitionValue && cutoff > -newTransitionValue ? 1.0f : 0.0f;

				float2 unitVector = UnitVector(_Angle);
				float2 direction = unitVector / 2.0f;
				float2 pushUV = i.uv + (newTransitionValue * SquareLength(_Angle)) * (AngleBetween(unitVector, newUV) < 0.0f ? direction : -direction);

				float4 tex = tex2D(_MainTex, i.uv);
				float4 transitionTex = tex2D(_TransitionTex, _LockUV ? pushUV : i.uv);
				float4 final = lerp(transitionTex, _Color, _TextureColor);

				return lerp(final, tex, value);
			}
			ENDCG
		}
	}
}
