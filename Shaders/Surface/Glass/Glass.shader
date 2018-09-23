Shader "Custom/Surface/Glass" 
{
	Properties 
	{
		_MainColor ("Glass Color", Color) = (1, 1, 1, 1)

		_FresnelWidth ("Fresnel Width", Range(0, 1)) = 1
		_FresnelColor ("Fresnel Color", Color) = (1, 1, 1, 1)

		_RefractionStrength ("Refraction Strength", Float) = 10.0

		_BumpMap ("Bump Map", 2D) = "white" {}
		_BumpMapIntensity ("Bump Map Intensity", Range(0, 10)) = 1

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

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
			float3 worldPos;
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
		half _FresnelWidth;
		half _RefractionStrength;
		half _Smoothness;
		half _Metallic;

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

			half3 refraction = refract(normalizedViewDir, normalizedNormal, 1.0f);
			half4 grabPos = ComputeGrabScreenPos(IN.screenPos);
			grabPos.xy = refraction.xy * _RefractionStrength + grabPos.xy;
			fixed4 refractedColor = tex2Dproj(_GrabTexture, grabPos + float4(normal, 0.0f));

			o.Albedo = lerp(lerp(_MainColor.rgb, _MainColor.rgb * refractedColor, _Smoothness), _FresnelColor, fresnel);
			o.Smoothness = _Smoothness;
			o.Alpha = _MainColor.a;
			o.Metallic = _Metallic;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
