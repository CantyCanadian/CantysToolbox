///====================================================================================================
///
///     EffectMath by
///     - CantyCanadian
///
///====================================================================================================

// Blur function that decrease in strength the further you are from the vertex. Based of the CommandBuffer blur example by Unity.
float4 Blur(sampler2D tex, float2 uv, int layers, float off)
{
	float4 col = float4(0.0f, 0.0f, 0.0f, 0.0f);
	float baseRatio = 0;

	for(int l = 1; l <= layers; l++)
	{
		baseRatio += l;
	}

	baseRatio = 1.0f / ((baseRatio * 4) + 1);

	off /= 1000.0f;

	if (layers > 0)
	{
		for(int l = 0; l < layers; l++)
		{
			float ratio = baseRatio * (layers - l);
			col += tex2D(tex, uv + (float2(off, off) * (l + 1))) * ratio;
			col += tex2D(tex, uv + (float2(-off, off) * (l + 1))) * ratio;
			col += tex2D(tex, uv + (float2(off, -off) * (l + 1))) * ratio;
			col += tex2D(tex, uv + (float2(-off, -off) * (l + 1))) * ratio;
		}

		col += tex2D(tex, uv) * baseRatio;
	}
	else
	{
		col += tex2D(tex, uv);
	}

	return col;
}