///====================================================================================================
///
///     Flag by
///     - CantyCanadian
///		- AlanZucconi
///
///====================================================================================================
Shader "Custom/Surface/Flag"
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TintAmount ("Tint Amount", Range(0,1)) = 0.5
		_ColorA ("Main Tint", Color) = (1,1,1,1)
		_ColorB ("Shadow Tint", Color) = (1,1,1,1)
		_Speed ("Wave Speed", Range(0, 80)) = 5
		_Frequency ("Frequency", Range(0,5)) = 2
		_AmplitudeY ("Amplitude Y", Range(-5,5)) = 1
		_AmplitudeZ ("Amplitude Z", Range(-5,5)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
			#pragma surface surf Lambert vertex:vert
			#pragma target 3.0
			
			struct Input
			{
				float2 uv_MainTex;
				float3 vertColor;
			};

			sampler2D _MainTex;

			fixed4 _ColorA;
			fixed4 _ColorB;

			float _Speed;
			float _Frequency;
			float _AmplitudeY;
			float _AmplitudeZ;

			half _TintAmount;
			
			void vert(inout appdata_full IN, out Input OUT)
			{
				float time = _Time * _Speed; 
				float waveValueY = sin(time + IN.vertex.x * _Frequency) * _AmplitudeY * IN.texcoord.x;
				float waveValueZ = sin(time + IN.vertex.x * _Frequency) * _AmplitudeZ * IN.texcoord.x;

				IN.vertex.xyz = float3(IN.vertex.x, IN.vertex.y + waveValueZ, IN.vertex.z - waveValueY * 0.0625);
				IN.normal = normalize(float3(IN.normal.x + waveValueZ, IN.normal.y, IN.normal.z));
				
				OUT.vertColor = float3(waveValueY, waveValueY, waveValueZ);
				OUT.uv_MainTex = IN.texcoord;
			}
			
			void surf(Input IN, inout SurfaceOutput OUT)
			{
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
				float3 tintColor = lerp(_ColorA, _ColorB, IN.vertColor).rgb;
				OUT.Albedo = lerp(c.rgb, c.rgb * tintColor, _TintAmount);
				OUT.Alpha = c.a;
			}
		ENDCG
	}
	FallBack "Diffuse"
}
