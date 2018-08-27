// Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
Shader "Custom/Surface/NormalMap" 
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_NormalMapIntensity ("Normal Map Intensity", range(0, 10)) = 1
		_Occlusion ("Occlusion", 2D) = "white" {}
		_Specular ("Specular Map", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM

		#pragma surface Surface StandardSpecular
			
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float2 uv_Occlusion;
			float2 uv_Specular;
		};

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _Occlusion;
		sampler2D _Specular;

		float _NormalMapIntensity;
			
		void Surface(Input IN, inout SurfaceOutputStandardSpecular o)
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			o.Occlusion = tex2D(_Occlusion, IN.uv_Occlusion).rgb;
			o.Specular = tex2D(_Specular, IN.uv_Specular).rgb;
				
			fixed3 normal = UnpackNormal (tex2D(_NormalMap, IN.uv_NormalMap)).rgb;
			normal.x *= _NormalMapIntensity;
			normal.y *= _NormalMapIntensity;
			o.Normal = normalize(normal);
		}
		ENDCG
	} 

	FallBack "Diffuse"
}
