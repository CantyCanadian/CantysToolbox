﻿///====================================================================================================
///
///     ColorMask by
///     - CantyCanadian
///		- ellioman
///
///====================================================================================================
Shader "Custom/Surface/Masks/ColorMask" 
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SecondTex ("Second Albedo (RGB)", 2D) = "white" {}
		_ColorMask ("Color Mask", 2D) = "white" {}
		_ColorAmount ("Color Amount", Range (-1,1)) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }

		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM

		#pragma surface Surface Standard fullforwardshadows alpha:fade
		#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_ColorMask;
			float2 uv_SecondTex;
		};

		sampler2D _MainTex;
		sampler2D _SecondTex;
		sampler2D _BumpMap;
		sampler2D _ColorMask;

		half _ColorAmount;
			
		void Surface(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 c2 = tex2D (_SecondTex, IN.uv_SecondTex);
			float cMask = clamp(tex2D (_ColorMask, IN.uv_ColorMask).r - _ColorAmount, 0, 1);
			o.Albedo = lerp(c.rgb, c2.rgb, cMask);
			o.Alpha = c.a;
		}
		ENDCG
	}
	
	Fallback off
}
