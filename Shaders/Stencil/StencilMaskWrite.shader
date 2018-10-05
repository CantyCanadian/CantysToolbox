///====================================================================================================
///
///     StencilMaskWrite by
///     - CantyCanadian
///		- ellioman
///
///====================================================================================================
Shader "Custom/Stencil/StencilMaskWrite"
{
	// This shader is used to write the "Ref" number, in the Stencil part, to the Stencil Buffer.

	Properties
	{
		_StencilVal ("Stencil Mask ID", Range(0, 255)) = 1
	}
		
	SubShader 
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry-100" }

		ColorMask 0
		ZWrite Off

		Stencil 
		{
			Ref [_StencilVal]
			Comp always
			Pass replace
		}
		
		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				struct VSInput 
				{
					float4 vertex : POSITION;
				};
				
				struct VSOutput 
				{
					float4 pos : SV_POSITION;
				};
				
				VSOutput vert(VSInput IN) 
				{
					VSOutput OUT;
					OUT.pos = UnityObjectToClipPos(IN.vertex);
					return OUT;
				}
				
				half4 frag(VSOutput IN) : COLOR 
				{
					return half4(1, 1, 1, 1);
				}
			ENDCG
		}
	}
}