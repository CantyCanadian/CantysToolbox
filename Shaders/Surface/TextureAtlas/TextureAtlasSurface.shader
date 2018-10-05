///====================================================================================================
///
///     TextureAtlasSurface by
///     - CantyCanadian
///		- ellioman
///
///====================================================================================================
Shader "Custom/Surface/TextureAtlas"
{
	Properties
	{
		_MainTex ("Atlas", 2D) = "white" {}
		_CurrentIndex ("Current Texture Index", Int) = 0
		_TextureColumns ("Texture Columns", Int) = 1
		_TextureRows ("Texture Rows", Int) = 1
	}

	SubShader 
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		
		CGPROGRAM
			// Pragmas
			#pragma surface surf Lambert vertex:vert
			#pragma target 3.0
			
			// User Defined Variables
			sampler2D _MainTex;

			uint _CurrentIndex;
			uint _TextureColumns;
			uint _TextureRows;

			// Base Input Structs
			struct Input 
			{
				float2 AtlasUV;
			};
			
			// The Vertex Shader
			void vert(inout appdata_full IN, out Input OUT)
			{
				float texWidth = (1.0f / (int)_TextureColumns);
				float texHeight = (1.0f / (int)_TextureRows);

				int atlasX = _CurrentIndex % _TextureColumns;
				int atlasY = _CurrentIndex / _TextureColumns;

				float u = (atlasX * texWidth) + (texWidth * IN.texcoord.x);
				float v = (atlasY * texHeight) + (texHeight * IN.texcoord.y);

				OUT.AtlasUV = float2(u, v);
			}
			
			void surf(Input IN, inout SurfaceOutput OUT) 
			{
				half4 c = tex2D (_MainTex, IN.AtlasUV);
				OUT.Albedo = c.rgb;
				OUT.Alpha = c.a;
			}
		ENDCG
	}

    Fallback off
}