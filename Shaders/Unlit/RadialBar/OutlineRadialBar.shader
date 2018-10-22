///====================================================================================================
///
///     OutlineRadialBar by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/Unlit/RadialBar/OutlineRadialBar"
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

        [Header(Bar Outline)]
        _BarOutlineSize("Size", Range(0.0, 1.0)) = 0.0
        [Toggle] _BarOutlineProgress("Outline Follows Progress", Int) = 1

        [Header(Bar Outline)]
        _BarOutlineTex ("Texture", 2D) = "white" {}
        _BarOutlineAlpha ("Texture Alpha", Range(0.0, 1.0)) = 1.0
        [HDR] _BarOutlineColor ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _BarOutlineBalance ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

		[Header(Bar Data)]
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
                float2 uvtex : TEXCOORD1;
                float2 uvtexback : TEXCOORD2;
                float2 uvtexoutline : TEXCOORD3;
			};

			sampler2D _BarTex;
			sampler2D _BarBackTex;
            sampler2D _BarOutlineTex;

            float4 _BarTex_ST;
            float4 _BarBackTex_ST;
            float4 _BarOutlineTex_ST;

			float4 _BarColor;
			float4 _BarBackColor;
            float4 _BarOutlineColor;

			float _BarAlpha;
			float _BarBackAlpha;
            float _BarOutlineAlpha;

			float _BarBalance;
			float _BarBackBalance;
            float _BarOutlineBalance;

            float _BarOutlineSize;
			float _BarProgress;
			float _BarAngle;
            float _BarRadius;
			float _BarWidth;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.uvtex = lerp(TRANSFORM_TEX(v.uv, _BarTex0), TRANSFORM_TEX(v.uv, _BarTex1), _BarProgress);
                o.uvtexback = lerp(TRANSFORM_TEX(v.uv, _BarBackTex0), TRANSFORM_TEX(v.uv, _BarBackTex1), _BarProgress);
                o.uvtexoutline = lerp(TRANSFORM_TEX(v.uv, _BarOutlineTex0), TRANSFORM_TEX(v.uv, _BarOutlineTex1), _BarProgress);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 newUV = (i.uv * 2.0f) - 1.0f;
				float uvDist = sqrt(newUV.x * newUV.x + newUV.y * newUV.y);

				float4 front = lerp(tex2D(_BarTex, i.uvtex), _BarColor, _BarBalance);
                front.a *= _BarAlpha;

				float4 back = lerp(tex2D(_BarBackTex, i.uvtexback), _BarBackColor, _BarBackBalance);
                back.a *= _BarBackAlpha;

                float4 outline = lerp(tex2D(_BarOutlineTex, i.uvtexoutline), _BarOutlineColor, _BarOutlineBalance);
                outline.a *= _BarOutlineAlpha;

				float angle = AngleBetween(float2(0.0f, -1.0f), newUV);

                if (uvDist > _BarRadius + _BarOutlineSize || uvDist < _BarRadius - _BarWidth - _BarOutlineSize || (!_BarOutlineProgress && angle < _BarAngle - _BarOutlineSize))
                {
                    discard;
                }

                if (uvDist > _BarRadius || uvDist < _BarRadius - _BarWidth || angle < _BarAngle)
                {
                    return outline;
                }

				angle = sign(newUV.x) == 1.0f ? 360.0f - angle : angle;
				float progression = (angle - _BarAngle) / (360.0f - _BarAngle - _BarAngle);

				return progression > _BarProgress ? back : front;
			}
			ENDCG
		}
	}
}
