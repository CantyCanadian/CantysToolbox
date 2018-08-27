// From the book Unity 5.x Shaders and Effects Cookbook. Taken from ellioman's Shader Project. https://github.com/ellioman/ShaderProject
Shader "Custom/PostProcess/BrightnessSaturationContrast"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RGBAAffect ("RGBA Effect", vector) = (0.5, 0.5, 0.5, 1.0)
		_BrightnessAmount ("Brightness Amount", Range(0.0, 1.0)) = 1.0
		_SaturationAmount ("Saturation Amount", Range(0.0, 1.0)) = 1.0
		_ContrastAmount ("Contrast Amount", Range(0.0, 1.0)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Cull Off 
		ZWrite Off 
		ZTest Always

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

			float4 _RGBAAffect;

			float _BrightnessAmount;
			float _SaturationAmount;
			float _ContrastAmount;

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 renderTex = tex2D(_MainTex, i.uv);
					
				float3 luminanceCoefficient = float3(0.2125, 0.7154, 0.0721);
					
				float3 brightnessColor = renderTex.rgb * _BrightnessAmount;
				float brightnessIntensity = dot(brightnessColor, luminanceCoefficient);
					
				float3 saturationColor = lerp(float3(brightnessIntensity, brightnessIntensity, brightnessIntensity), brightnessColor, _SaturationAmount);
					
				float3 contrastColor = lerp(_RGBAAffect.rgb, saturationColor, _ContrastAmount);

				renderTex.rgb = lerp(contrastColor, renderTex.rgb, _RGBAAffect.a);
				return renderTex;
			}
			ENDCG
		}
	}
}
