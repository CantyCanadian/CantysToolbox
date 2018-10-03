// Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
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
			
			void vert(inout appdata_base v)
			{
				v.texcoord.xy = TRANSFORM_TEX(v.texcoord, _ExtrusionTex);
				
				// Get the data from the extrusion texture
				float4 tex = tex2Dlod(_ExtrusionTex, float4(v.texcoord.xy, 0.0, 0.0));
				float extrusion = tex.r;
				
				// Apply the amount from the variable and extrusion and multiply it to the normal
				v.vertex.xyz += v.normal * _Amount * extrusion * _Power;
			}
			
			// The Surface Shader
			void surf(Input IN, inout SurfaceOutput o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		ENDCG
	}
}
