// Original shader made by @minionsart on Twitter, heavily modified by @cantycanadian.
// Special thanks to @bgolus for the grabpass hack I used to make the highly specific blending I wanted.

// Known bug : You can't see another liquid through a transparent liquid. Won't fix since I have no clue how to actually fix that. I already had issues getting the transparency right...
Shader "Unlit/Canty/Liquid"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_FillAmount("Fill Amount", Range(-10,10)) = 0.0
		_Alpha("General Alpha", Range(0, 1)) = 1.0

		_TopColor("Top Color", Color) = (1,1,1,1)
		_FoamColor("Foam Line Color", Color) = (1,1,1,1)
		_FoamRim("Foam Line Width", Range(0,0.1)) = 0.0

		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(0,10)) = 0.0

		[HideInInspector] _WobbleX("WobbleX", Range(-1,1)) = 0.0
		[HideInInspector] _WobbleZ("WobbleZ", Range(-1,1)) = 0.0
	}

		SubShader
	{
		Tags{ "Queue" = "Geometry+500" "RenderType" = "TransparentCutout" "IgnoreProjector" = "True" "DisableBatching" = "True" }

		GrabPass{ "_BackgroundTex" }
		Pass
		{
			Cull Front

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 uv_back : TEXCOORD1;
				float fillEdge : TEXCOORD2;
				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex, _BackgroundTex;
			float4 _MainTex_ST;
			float _FillAmount, _WobbleX, _WobbleZ, _Alpha;
			float4 _TopColor, _RimColor, _FoamColor, _Tint;
			float _FoamRim, _RimPower;

			float4 RotateAroundYInDegrees(float4 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, sina, -sina, cosa);
				return float4(vertex.yz , mul(m, vertex.xz)).xzyw;
			}

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv_back = ComputeGrabScreenPos(o.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				// get world position of the vertex
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);

				// rotate it around XY
				float3 worldPosX = RotateAroundYInDegrees(float4(worldPos,0),360);

				// rotate around XZ
				float3 worldPosZ = float3 (worldPosX.y, worldPosX.z, worldPosX.x);

				// combine rotations with worldPos, based on sine wave from script
				float3 worldPosAdjusted = worldPos + (worldPosX  * _WobbleX) + (worldPosZ* _WobbleZ);

				// y pos of the liquid
				o.fillEdge = worldPosAdjusted.y + _FillAmount;
				return o;
			}

			fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				if (i.fillEdge > 0.5f)
				{
					discard;
				}

				// foam edge
				float4 foam = (step(i.fillEdge, 0.5) - step(i.fillEdge, (0.5 - _FoamRim)));

				// rest of the liquid
				float4 result = step(i.fillEdge, 0.5) - foam;

				// color of backfaces/ top
				float4 topColor = _TopColor * (foam + result);

				// sample the background
				float2 screenuv = i.uv_back.xy / i.uv_back.w;
				fixed4 back = tex2D(_BackgroundTex, screenuv);

				// blend main color and background together
				return  fixed4(lerp(back.rgb, topColor.rgb, _Alpha) * topColor.a, topColor.a);
			}
			ENDCG
		}

		Pass
		{
			Cull Back

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 uv_back : TEXCOORD1;
				float fillEdge : TEXCOORD2;
				float3 viewDir : COLOR;
				float3 normal : COLOR2;
				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex, _BackgroundTex;
			float4 _MainTex_ST;
			float _FillAmount, _WobbleX, _WobbleZ, _Alpha;
			float4 _TopColor, _RimColor, _FoamColor, _Tint;
			float _FoamRim, _RimPower;

			float4 RotateAroundYInDegrees(float4 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, sina, -sina, cosa);
				return float4(vertex.yz , mul(m, vertex.xz)).xzyw;
			}

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv_back = ComputeGrabScreenPos(o.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				// get world position of the vertex
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);

				// rotate it around XY
				float3 worldPosX = RotateAroundYInDegrees(float4(worldPos,0),360);

				// rotate around XZ
				float3 worldPosZ = float3 (worldPosX.y, worldPosX.z, worldPosX.x);

				// combine rotations with worldPos, based on sine wave from script
				float3 worldPosAdjusted = worldPos + (worldPosX  * _WobbleX) + (worldPosZ* _WobbleZ);

				// y pos of the liquid
				o.fillEdge = worldPosAdjusted.y + _FillAmount;

				o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
				o.normal = v.normal;
				return o;
			}

			fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
			
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				if (i.fillEdge > 0.5f)
				{
					discard;
				}

				// rim light
				float dotProduct = 1 - pow(dot(i.normal, i.viewDir), _RimPower);
				float4 RimResult = smoothstep(0.5, 1.0, dotProduct);
				RimResult *= _RimColor;

				// foam edge
				float4 foam = (step(i.fillEdge, 0.5) - step(i.fillEdge, (0.5 - _FoamRim)));

				// rest of the liquid
				float4 result = step(i.fillEdge, 0.5) - foam;
				float4 resultColored = result * col;

				// both together, with the texture
				float4 finalResult = resultColored;
				finalResult.rgb = lerp(finalResult.rgb, RimResult.rgb, RimResult.a);

				// color of backfaces/ top
				float4 topColor = _TopColor * (foam + result);

				// decide if vertex is part of foam
				float4 final = step(i.fillEdge, 0.5 - _FoamRim) ? finalResult : topColor;

				// sample the background
				float2 screenuv = i.uv_back.xy / i.uv_back.w;
				fixed4 back = tex2D(_BackgroundTex, screenuv);

				// blend main color and background together
				return  fixed4(lerp(back.rgb, final.rgb, _Alpha) * final.a, final.a);
			}
			ENDCG
		}
	}
}