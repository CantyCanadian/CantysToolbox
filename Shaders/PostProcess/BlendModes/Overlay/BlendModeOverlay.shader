// From the book Unity 5.x Shaders and Effects Cookbook. Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
Shader "Custom/PostProcess/BlendMode/Overlay"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlendTex ("Blend Texture (RGB)", 2D) = "white" {}
		_Opacity ("Blend Opacity", Range(0.0, 1.0)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
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
			sampler2D _BlendTex;

			float _Opacity;

			fixed OverlayBlendMode(fixed basePixel, fixed blendPixel)
			{
				if (basePixel < 0.5f)
				{
					return (2.0f * basePixel) * blendPixel;
				}
				else
				{
					return (1.0f - 2.0f * (1.0f - basePixel) * (1.0f - blendPixel));
				}
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed4 blendTex = tex2D(_BlendTex, i.uv);
					
				fixed4 blendedImage = renderTex;
				blendedImage.r = OverlayBlendMode(renderTex.r, blendTex.r);
				blendedImage.g = OverlayBlendMode(renderTex.g, blendTex.g);
				blendedImage.b = OverlayBlendMode(renderTex.b, blendTex.b);
					
				renderTex = lerp(renderTex, blendedImage, _Opacity);
				return renderTex;
			}
			ENDCG
		}
	}
}
