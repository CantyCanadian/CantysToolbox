///====================================================================================================
///
///     CompleteRadialBar by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/Unlit/CompleteRadialBar"
{
	Properties
	{
		[Header(Bar Front)]
		_BarTex ("Texture", 2D) = "white" {}
		_BarAlpha ("Texture Alpha", Range(0.0, 1.0)) = 1.0
		[HDR] _BarColor ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BarBalance ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

		[Header(Bar Back)]
		_BarBackTex ("Texture", 2D) = "white" {}
		_BarBackAlpha ("Texture Alpha", Range(0.0, 1.0)) = 1.0
		[HDR] _BarBackColor ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BarBackBalance ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

		[Header(Bar Data)]
		_BarProgress ("Progress", Range(0.0, 1.0)) = 0.0
		_BarAngle ("Angle", Range(0.0, 180.0)) = 45.0
		_BarWidth ("Width", Range(0.0, 1.0)) = 0.3
	}
	SubShader
	{
		Tags 
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane" 
		}

		Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGBA

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "../../CGIncludes/AngleMath.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD;
				float2 frontuv : TEXCOORD1;
				float2 backuv : TEXCOORD2;
			};

			sampler2D _BarTex;
			sampler2D _BarBackTex;

			float4 _BarTex_ST;
			float4 _BarBackTex_ST;
			float4 _BarColor;
			float4 _BarBackColor;

			float _BarAlpha;
			float _BarBackAlpha;
			float _BarBalance;
			float _BarBackBalance;
			float _BarProgress;
			float _BarAngle;
			float _BarWidth;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.frontuv = TRANSFORM_TEX(v.uv, _BarTex);
				o.backuv = TRANSFORM_TEX(v.uv, _BarBackTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 newUV = (i.uv * 2.0f) - 1.0f;
				float uvDist = sqrt(newUV.x * newUV.x + newUV.y * newUV.y);

				float4 col = lerp(tex2D(_BarTex, i.frontuv), _BarColor, _BarBalance);
				col.a *= _BarAlpha;

				float4 backCol = lerp(tex2D(_BarBackTex, i.backuv), _BarBackColor, _BarBackBalance);
				backCol.a *= _BarBackAlpha;

				float angle = AngleBetween(float2(0.0f, -1.0f), newUV);

				if (uvDist > 1.0f || uvDist < 1.0f - _BarWidth || angle < _BarAngle)
				{
					discard;
				}

				angle = sign(newUV.x) == 1.0f ? 360.0f - angle : angle;
				float progression = (angle - _BarAngle) / (360.0f - _BarAngle - _BarAngle);

				return progression > _BarProgress ? backCol : col;
			}
			ENDCG
		}
	}
}
