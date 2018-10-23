///====================================================================================================
///
///     CustomBar by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/Unlit/CustomBar"
{
	Properties
	{
        _BarShape ("Bar Shape", 2D) = "white" {}
        _BarProgressTex ("Bar Progression Texture", 2D) = "white" {}
		_BarFrontTex ("Bar Under Progress Texture", 2D) = "white" {}
        _BarBackTex ("Bar Over Progress Texture", 2D) = "white" {}
        _BarProgress ("Progress", Range(0.0, 1.0)) = 0.0
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
                float2 uvshape : TEXCOORD1;
                float2 uvprogress : TEXCOORD2;
                float2 uvtexfront : TEXCOORD3;
                float2 uvtexback : TEXCOORD4;
			};

			sampler2D _BarShape;
			sampler2D _BarProgressTex;
            sampler2D _BarFrontTex;
            sampler2D _BarBackTex;

            float4 _BarShape_ST;
            float4 _BarProgressTex_ST;
            float4 _BarFrontTex_ST;
            float4 _BarBackTex_ST;

			float _BarProgress;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.uvshape = TRANSFORM_TEX(v.uv, _BarShape);
                o.uvprogress = TRANSFORM_TEX(v.uv, _BarProgressTex);
                o.uvtexfront = TRANSFORM_TEX(v.uv, _BarFrontTex);
                o.uvtexback = TRANSFORM_TEX(v.uv, _BarBackTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 shape = tex2D(_BarTex, i.uvshape);
                float4 progress = tex2D(_BarTex, i.uvprogress);
                float4 front = tex2D(_BarTex, i.uvtexfront);
                float4 back = tex2D(_BarTex, i.uvtexback);

				if (shape.r == 0.0f)
				{
					discard;
				}

				return progress > _BarProgress ? back : front;
			}
			ENDCG
		}
	}
}
