///====================================================================================================
///
///     TransitiveRadialBar by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/Unlit/RadialBar/Transitive"
{
	Properties
	{
		[Header(Bar Under Progress at 0)]
        _BarTex0 ("Texture", 2D) = "white" {}
		_BarAlpha0 ("Texture Alpha", Range(0.0, 1.0)) = 1.0
		[HDR] _BarColor0 ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BarBalance0 ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

        [Header(Bar Under Progress at 1)]
        _BarTex1 ("Texture", 2D) = "white" {}
        _BarAlpha1 ("Texture Alpha", Range(0.0, 1.0)) = 1.0
        [HDR] _BarColor1 ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _BarBalance1 ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

		[Header(Bar Over Progress at 0)]
        _BarBackTex0 ("Texture", 2D) = "white" {}
		_BarBackAlpha0 ("Texture Alpha", Range(0.0, 1.0)) = 1.0
		[HDR] _BarBackColor0 ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BarBackBalance0 ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

        [Header(Bar Over Progress at 1)]
        _BarBackTex1 ("Texture", 2D) = "white" {}
        _BarBackAlpha1 ("Texture Alpha", Range(0.0, 1.0)) = 1.0
        [HDR] _BarBackColor1 ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _BarBackBalance1 ("Texture-Color Balance", Range(0.0, 1.0)) = 0.0

		[Header(Bar Data)]
        _BarAlphaMask("Alpha Mask", 2D) = "white" {}
		_BarProgress ("Progress", Range(0.0, 1.0)) = 0.0
		_BarAngle ("Angle", Range(0.0, 180.0)) = 45.0
        _BarRadius ("Radius", Range(0.0, 1.0)) = 1.0
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

			sampler2D _BarTex0;
            sampler2D _BarTex1;
			sampler2D _BarBackTex0;
            sampler2D _BarBackTex1;
            sampler2D _BarOutlineTex0;
            sampler2D _BarOutlineTex1;
			sampler2D _BarAlphaMask;

            float4 _BarTex0_ST;
            float4 _BarTex1_ST;
            float4 _BarBackTex0_ST;
            float4 _BarBackTex1_ST;
            float4 _BarOutlineTex0_ST;
            float4 _BarOutlineTex1_ST;
			float4 _BarAlphaMask_ST;

			float4 _BarColor0;
            float4 _BarColor1;
			float4 _BarBackColor0;
            float4 _BarBackColor1;

			float _BarAlpha0;
            float _BarAlpha1;
			float _BarBackAlpha0;
            float _BarBackAlpha1;

			float _BarBalance0;
            float _BarBalance1;
			float _BarBackBalance0;
            float _BarBackBalance1;

			float _BarProgress;
			float _BarAngle;
            float _BarRadius;
			float _BarWidth;

            int _BarOutlineProgress;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uvalpha = TRANSFORM_TEX(v.uv, _BarAlphaMask);
				o.uvtex = lerp(TRANSFORM_TEX(v.uv, _BarTex0), TRANSFORM_TEX(v.uv, _BarTex1), _BarProgress);
                o.uvtexback = lerp(TRANSFORM_TEX(v.uv, _BarBackTex0), TRANSFORM_TEX(v.uv, _BarBackTex1), _BarProgress);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 newUV = (i.uv * 2.0f) - 1.0f;
				float uvDist = sqrt(newUV.x * newUV.x + newUV.y * newUV.y);

                float4 frontTex = lerp(tex2D(_BarTex0, i.uvtex), tex2D(_BarTex1, i.uvtex), _BarProgress);
                float4 frontCol = lerp(_BarColor0, _BarColor1, _BarProgress);
				float4 front = lerp(frontTex, frontCol, lerp(_BarBalance0, _BarBalance1, _BarProgress));
                front.a *= lerp(_BarAlpha0, _BarAlpha1, _BarProgress);

                float4 backTex = lerp(tex2D(_BarBackTex0, i.uvtexback), tex2D(_BarBackTex1, i.uvtexback), _BarProgress);
                float4 backCol = lerp(_BarBackColor0, _BarBackColor1, _BarProgress);
				float4 back = lerp(backTex, backCol, lerp(_BarBackBalance0, _BarBackBalance1, _BarProgress));
                back.a *= lerp(_BarBackAlpha0, _BarBackAlpha1, _BarProgress);

                float4 alphaMask = tex2D(_BarAlphaMask, i.uvalpha);

				float angle = AngleBetween(float2(0.0f, -1.0f), newUV);

				if (uvDist > _BarRadius || uvDist < _BarRadius - _BarWidth || angle < _BarAngle)
				{
					discard;
				}

				angle = sign(newUV.x) == 1.0f ? 360.0f - angle : angle;
				float progression = (angle - _BarAngle) / (360.0f - _BarAngle - _BarAngle);

                float4 finalColor = progression > _BarProgress ? back : front;
                finalColor.a *= alphaMask.r;

                return finalColor;
			}
			ENDCG
		}
	}
}
