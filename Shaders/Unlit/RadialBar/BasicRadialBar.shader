﻿///====================================================================================================
///
///     BasicRadialBar by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/Unlit/RadialBar/Basic"
{
	Properties
	{
		[Header(Bar Under Progress Side)]
		_BarTex ("Texture", 2D) = "white" {}
		_BarAlpha ("Texture Alpha", Range(0.0, 1.0)) = 1.0
		[HDR] _BarColor ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BarBalance ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

		[Header(Bar Over Progress Side)]
		_BarBackTex ("Texture", 2D) = "white" {}
		_BarBackAlpha ("Texture Alpha", Range(0.0, 1.0)) = 1.0
		[HDR] _BarBackColor ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BarBackBalance ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

		[Header(Bar Data)]
        _BarAlphaMask ("Alpha Mask", 2D) = "white" {}
		_BarProgress ("Progress", Range(0.0, 1.0)) = 0.0
		_BarAngle ("Angle", Range(0.0, 180.0)) = 45.0
        _BarRadius("Radius", Range(0.0, 1.0)) = 1.0
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
                float2 uv : TEXCOORD0;
                float2 uvalpha : TEXCOORD1;
                float2 uvtex : TEXCOORD2;
                float2 uvtexback : TEXCOORD3;
			};

			sampler2D _BarTex;
			sampler2D _BarBackTex;
            sampler2D _BarAlphaMask;

            float4 _BarTex_ST;
            float4 _BarBackTex_ST;
            float4 _BarAlphaMask_ST;

			float4 _BarColor;
			float4 _BarBackColor;

			float _BarAlpha;
			float _BarBackAlpha;

			float _BarBalance;
			float _BarBackBalance;

			float _BarProgress;
			float _BarAngle;
            float _BarRadius;
			float _BarWidth;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.uvalpha = TRANSFORM_TEX(v.uv, _BarAlphaMask);
                o.uvtex = TRANSFORM_TEX(v.uv, _BarTex);
                o.uvtexback = TRANSFORM_TEX(v.uv, _BarBackTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 newUV = (i.uv * 2.0f) - 1.0f;
				float uvDist = sqrt(newUV.x * newUV.x + newUV.y * newUV.y);

				float4 col = lerp(tex2D(_BarTex, i.uvtex), _BarColor, _BarBalance);
				col.a *= _BarAlpha;

				float4 backCol = lerp(tex2D(_BarBackTex, i.uvtexback), _BarBackColor, _BarBackBalance);
				backCol.a *= _BarBackAlpha;

                float4 alphaMask = tex2D(_BarAlphaMask, i.uvalpha);

				float angle = AngleBetween(float2(0.0f, -1.0f), newUV);

				if (alphaMask.r == 0.0f || uvDist > _BarRadius || uvDist < _BarRadius - _BarWidth || angle < _BarAngle)
				{
					discard;
				}

				angle = sign(newUV.x) == 1.0f ? 360.0f - angle : angle;
				float progression = (angle - _BarAngle) / (360.0f - _BarAngle - _BarAngle);

                float4 finalColor = progression > _BarProgress ? backCol : col;
                finalColor.a *= alphaMask.r;

				return finalColor;
			}
			ENDCG
		}
	}
}
