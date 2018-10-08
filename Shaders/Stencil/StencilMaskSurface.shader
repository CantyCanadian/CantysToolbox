///====================================================================================================
///
///     StencilMaskSurface by
///     - CantyCanadian
///		- ellioman
///		- supyrb
///
///====================================================================================================
Shader "Custom/Stencil/StencilMaskSurface"
{
	Properties
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0.0

		[Header(Stencil)]
		[IntRange] _StencilVal ("Stencil Mask ID", Range(0, 255)) = 1
		[IntRange] _ReadMask ("Read Mask", Range(0, 255)) = 255
        [IntRange] _WriteMask ("Write Mask", Range(0, 255)) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 3
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilFail ("Stencil Fail", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail ("Stencil ZFail", Int) = 0

		[Header(Rendering)]
        [Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Int) = 2
        [Enum(Off,0,On,1)] _ZWrite ("ZWrite", Int) = 1
        [Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)] _ColorMask ("Writing Color Mask", Int) = 15
	}
	
	SubShader
	{		
		// Stencil Buffer: http://docs.unity3d.com/Manual/SL-Stencil.html
		Stencil
		{
			// The value to be compared against (if Comp is anything else than always) and/or the value to be
			// written to the buffer (if either Pass, Fail or ZFail is set to replace). 0–255 integer.
			Ref [_StencilVal]

			// An 8 bit mask as an 0–255 integer, used when comparing the reference value with the contents of 
			// the buffer (referenceValue & readMask) comparisonFunction (stencilBufferValue & readMask). Default: 255.
			ReadMask [_ReadMask]

			// An 8 bit mask as an 0–255 integer, used when writing to the buffer. Note that, like other write masks, it 
			// specifies which bits of stencil buffer will be affected by write (i.e. WriteMask 0 means that no bits are affected 
			// and not that 0 will be written). Default: 255.
            WriteMask [_WriteMask]
			
			// The function used to compare the reference value to the current contents of the buffer.
			// Here we use Equal because we only want to render when the stencil matched the "Ref" value above
			Comp [_StencilComp]
			
			// What to do with the contents of the buffer if the stencil test (and the depth test) passes.
			// We use keep because we don't want to mess with the buffer
			Pass [_StencilPass]
			
			// What to do with the contents of the buffer if the stencil test fails.
			// We use keep because we don't want to mess with the buffer
			Fail [_StencilFail]

			// What to do with the contents of the buffer if the stencil test passes, but the depth test fails. Default: keep.
			ZFail [_StencilZFail]
		}

		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 100
        Cull [_Culling]					// Controls which sides of polygons should be culled (not drawn)
        ZWrite [_ZWrite]				// Controls whether pixels from this object are written to the depth buffer (default is On).
        ColorMask [_ColorMask]			// Set color channel writing mask. For some special effects you might want to leave certain channels unmodified.
           
        CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
	
		struct Input
		{
			float2 uv_MainTex;
		};

		sampler2D _MainTex;

		fixed4 _Color;

		half _Glossiness;
		half _Metallic;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
        ENDCG
	}
}