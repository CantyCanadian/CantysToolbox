///====================================================================================================
///
///     AlphaMaskCulled by
///     - CantyCanadian
///		- ellioman
///
///====================================================================================================
Shader "Custom/Surface/Masks/AlphaMaskCulled" 
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_AlphaMask ("Alpha Mask", 2D) = "white" {}
		_AlphaAmount ("Alpha Amount", Range (-1, 1)) = 1
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
			float2 uv_AlphaMask;
		};

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _AlphaMask;

		half _AlphaAmount;
			
		void Surface(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 col = tex2D (_MainTex, IN.uv_MainTex);
			float alphaMask = tex2D (_AlphaMask, IN.uv_AlphaMask).g - _AlphaAmount;

			o.Albedo = col.rgb;
			o.Alpha = alphaMask;
		}
		ENDCG
	}
	
	Fallback off
}
