// Original shader made by JPBotelho on Github. https://github.com/JPBotelho/Dissolve-Shader
Shader "Custom/Surface/Dissolve"
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_EdgeColor ("Edge Color", Color) = (1,1,1,1)

		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_Noise ("Noise", 2D) = "white" {}

		_EdgeThreshold ("Edge", Range (0, 0.25)) = 0
		_SecondEdgeThreshold ("Second Edge", Range (0, 0.25)) = 0

		[MaterialToggle] _Invert("Invert", Float) = 0
	}
	SubShader 
	{
		Tags 
    	{
        	"Queue"="Transparent" 
        	"RenderType"="Transparent" 
		}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Noise;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_Noise;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _EdgeColor;
		float _EdgeThreshold;
		bool _Invert;

		uniform float _Dissolve; //Set in script

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 noiseSample = tex2D(_Noise, IN.uv_Noise);
			fixed4 col = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			float firstBorder;
			bool isBorder;

			if (_Invert)
			{
				firstBorder = _Dissolve - _EdgeThreshold;
				isBorder = noiseSample > firstBorder;
			}
			else
			{
				firstBorder = _Dissolve + _EdgeThreshold;
				isBorder = noiseSample < firstBorder;
			}

			if (isBorder)
			{
				o.Albedo = _EdgeColor;
			}
			else
			{
				o.Albedo = col.rgb;
			}

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			if (_Invert)
			{
				o.Alpha = noiseSample < _Dissolve;
			} 
			else
			{
				o.Alpha = noiseSample > _Dissolve;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}