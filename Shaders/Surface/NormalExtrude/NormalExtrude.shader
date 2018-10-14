///====================================================================================================
///
///     NormalExtrude by
///     - CantyCanadian
///		- ellioman
///
///====================================================================================================
Shader "Custom/Surface/NormalExtrude" 
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_ExtrusionTex ("Extrusion Texture", 2D) = "white" {}
		_Amount ("Extrusion Amount", Range(0,1)) = 0
		_Power ("Extrusion Power", Float) = 1.0
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		LOD 200
		
		CGPROGRAM
			#pragma surface surf Lambert vertex:vert
			#pragma target 3.0
			
			sampler2D _MainTex;
			sampler2D _ExtrusionTex;

			float4 _ExtrusionTex_ST;
			fixed4 _Color;

			float _Amount;
			float _Power;
			
			struct Input
			{
				float2 uv_MainTex;
			};
			
			void vert(inout appdata_full v)
			{
				v.texcoord.xy = TRANSFORM_TEX(v.texcoord, _ExtrusionTex);
				
				float4 tex = tex2Dlod(_ExtrusionTex, float4(v.texcoord.xy, 0.0, 0.0));
				float extrusion = tex.r;
				
				v.vertex.xyz += v.normal * _Amount * extrusion * _Power;
			}
			
			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		ENDCG
	}
}
