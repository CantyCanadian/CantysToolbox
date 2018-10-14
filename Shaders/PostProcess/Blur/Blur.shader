///====================================================================================================
///
///     Blur by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/PostProcess/Blur"
{
	Properties
	{
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}

		_BlurLayers ("Blur Layers", Int) = 3
		_Blur ("Blur Offset", Float) = 1
	}
	SubShader
	{
		Cull Off 
		ZWrite Off 
		ZTest Always
		Fog { Mode off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"
			#include "../../CGIncludes/EffectMath.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;

			float _Blur;

			int _BlurLayers;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return Blur(_MainTex, i.uv, _BlurLayers, _Blur);
			}
			ENDCG
		}
	}
}
