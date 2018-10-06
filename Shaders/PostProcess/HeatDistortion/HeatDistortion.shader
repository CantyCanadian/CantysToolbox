///====================================================================================================
///
///     HeatDistortion by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/PostProcess/HeatDistortion"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		
		[Header(Effect at Y0)]
		_Y0Amplitude ("Amplitude", Float) = 0.0
		_Y0Length ("Length", Float) = 3.14

		[Header(Effect at Y1)]
		_Y1Amplitude ("Amplitude", Float) = 3.0
		_Y1Length ("Length", Float) = 3.14
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			float _Y0Amplitude;
			float _Y0Length;
			float _Y1Amplitude;
			float _Y1Length;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				float length = lerp(_Y0Length, _Y1Length, i.uv.y);
				float amplitude = lerp(_Y0Amplitude, _Y1Amplitude, i.uv.y);

				float sinX = sin(_Time.y + i.uv.y * length) * amplitude;
				return tex2D(_MainTex, i.uv + float2(sinX, 0.0f));
			}
			ENDCG
		}
	}
}
