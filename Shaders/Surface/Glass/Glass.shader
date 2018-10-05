///====================================================================================================
///
///     Glass by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/Surface/Glass" 
{
	Properties 
	{
		_MainColor ("Glass Color", Color) = (1, 1, 1, 1)

		_FresnelWidth ("Fresnel Width", Range(0, 1)) = 1
		_FresnelColor ("Fresnel Color", Color) = (1, 1, 1, 1)

		[Normal] _BumpMap ("Bump Map", 2D) = "white" {}
		_BumpMapIntensity ("Bump Map Intensity", Range(0, 10)) = 1

		_BlurLayers ("Blur Layers", Int) = 3
		_Blur ("Blur Offset", Float) = 0.0

		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0.0
	}
	SubShader 
	{
		GrabPass { }

		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows alpha
		#pragma target 3.0

		#include "../../CGIncludes/EffectMath.cginc"

		struct Input 
		{
			float2 uv_BumpMap;
			float3 viewDir;
			float4 screenPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		sampler2D _BumpMap;
		sampler2D _GrabTexture;

		float4 _GrabTexture_TexelSize;
		fixed4 _MainColor;
		fixed4 _FresnelColor;

		float _BumpMapIntensity;
		fixed _Blur;
		half _FresnelWidth;
		half _Smoothness;
		half _Metallic;

		int _BlurLayers;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			#ifndef UNITY_UV_STARTS_AT_TOP
				IN.screenPos = 1.0f - IN.screenPos;
			#endif

			half3 normalizedNormal = normalize(IN.worldNormal);
			half3 normalizedViewDir = normalize(IN.viewDir);

			float fresnel = 1 - saturate(dot( IN.worldNormal, IN.viewDir ));
			fresnel = smoothstep(1.0f - _FresnelWidth, 1.0f, fresnel);

			half3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_BumpMap), _BumpMapIntensity);

			float2 refractionUV = IN.screenPos.xy;
			refractionUV = (refractionUV / IN.screenPos.w) + normal.xy;

			fixed4 refractedColor = Blur(_GrabTexture, refractionUV, _BlurLayers, _Blur);			

			o.Albedo = lerp(_MainColor.rgb * refractedColor.rgb, _FresnelColor, fresnel);
			o.Smoothness = _Smoothness;
			o.Alpha = 1.0f;
			o.Metallic = _Metallic;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
