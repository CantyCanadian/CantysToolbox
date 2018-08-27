// Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
Shader "Custom/PostProcess/BlackWhite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Blend ("Black & White blend", Range (0, 1)) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			float4 _MainTex_TexelSize;

			float _Blend;

			fixed4 frag (v2f i) : COLOR
			{
				float4 col = tex2D(_MainTex, i.uv);
						
				// The three magic numbers represent the sensitivity of the human eye to the R, G and B components. This is taken from http://www.alanzucconi.com/2015/07/08/screen-shaders-and-postprocessing-effects-in-unity3d/
				float luminosity = (col.r * 0.3f) + (col.g * 0.59f) + (col.b * 0.11f);
				float3 blackWhite = float3(luminosity, luminosity, luminosity); 
						
				float4 result = col;
				result.rgb = lerp(col.rgb, blackWhite, _Blend);
				return result;
			}
			ENDCG
		}
	}
}
