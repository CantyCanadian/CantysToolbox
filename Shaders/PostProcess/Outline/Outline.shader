///====================================================================================================
///
///     Outline by
///     - CantyCanadian
///
///====================================================================================================
Shader "Custom/PostProcess/Outline" 
{
	Properties 
	{
		_OutlineColor ("Outline Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineWidth ("Outline Width", Int) = 1.0
		[Toggle] _SeeThrough ("See-Through", Int) = 1
		[Toggle] _FullOutline ("Full Outline", Int) = 1
		[Toggle] _FillShape ("Fill Shape", Int) = 0
		[Toggle] _OutlineCorner ("Show Outline Corner", Int) = 1
	}
	SubShader 
	{
		Pass
		{
			ZWrite Off
			Cull Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _RendererTex;
			sampler2D _OppositeTex;

			int _SeeThrough;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float4 col = tex2D(_RendererTex, i.uv);
				float depth1 = SAMPLE_DEPTH_TEXTURE(_RendererTex, i.uv);
				float depth101 = Linear01Depth(depth1);

				float4 result = float4(0.0f, 0.0f, 0.0f, 0.0f);

				if (depth101 != 1.0f)
				{
					result = float4(1.0f, 0.0f, 0.0f, 0.0f);
				}

				if (!_SeeThrough)
				{
					float4 col2 = tex2D(_OppositeTex, i.uv);
					float depth2 = SAMPLE_DEPTH_TEXTURE(_OppositeTex, i.uv);
					float depth201 = Linear01Depth(depth2);

					if (depth201 != 1.0f)
					{
						result = float4(0.0f, 1.0f, 0.0f, 0.0f);
					}
				}

				return result;
			}

			ENDCG
		}

		Pass
		{
			ZWrite Off
			Cull Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _OutlineTex;

			float4 _OutlineTex_TexelSize;
			float4 _OutlineColor;

			float _OutlineWidth;

			int _OutlineCorner;
			int _FillShape;
			int _FullOutline;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float4 col = tex2D(_OutlineTex, i.uv);

				if ((!_FillShape && col.r == 1.0f) || (!_FullOutline && col.g == 1.0f))
				{
					discard;
				}
				
				float4 sampleCol = float4(0.0f, 0.0f, 0.0f, 0.0f);

				float2 smallXY = float2(_OutlineWidth / _OutlineTex_TexelSize.z, _OutlineWidth / _OutlineTex_TexelSize.w);

				sampleCol += tex2D(_OutlineTex, i.uv + float2(smallXY.x, 0.0f));
				sampleCol += tex2D(_OutlineTex, i.uv + float2(-smallXY.x, 0.0f));
				sampleCol += tex2D(_OutlineTex, i.uv + float2(0.0f, smallXY.y));
				sampleCol += tex2D(_OutlineTex, i.uv + float2(0.0f, -smallXY.y));

				if (_OutlineCorner)
				{
					sampleCol += tex2D(_OutlineTex, i.uv + float2(smallXY.x, smallXY.y));
					sampleCol += tex2D(_OutlineTex, i.uv + float2(-smallXY.x, smallXY.y));
					sampleCol += tex2D(_OutlineTex, i.uv + float2(smallXY.x, -smallXY.y));
					sampleCol += tex2D(_OutlineTex, i.uv + float2(-smallXY.x, -smallXY.y));
				}	

				if (sampleCol.r == 0.0f)
				{
					discard;
				}

				return _OutlineColor;
			}
			ENDCG
		}
	}
}