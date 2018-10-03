// Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
Shader "Custom/PostProcess/Pixelate"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_PixelSize ("Pixel Size", Range(1, 100)) = 1
	}

	Category
	{
		SubShader
		{
			Pass
			{
				CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
					
				// Helper functions
				#include "UnityCG.cginc"
					
				// User Defined Variables
				uniform float _PixelSize;
				uniform sampler2D _MainTex;
				uniform float4 _MainTex_TexelSize;
					
				// The Fragment Shader
				float4 frag(v2f_img IN) : COLOR
				{
					half2 uv = IN.uv;

					if (_PixelSize != 1.0)
					{
						_PixelSize *= _MainTex_TexelSize;
						int x = (IN.uv.x / _PixelSize);
						uv.x = x * _PixelSize;
							
						int y = (IN.uv.y / _PixelSize);
						uv.y = y * _PixelSize;
					}

					return tex2D(_MainTex, uv);
				}
					
				ENDCG
			}
		}
	}
}
