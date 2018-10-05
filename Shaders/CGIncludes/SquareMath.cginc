///====================================================================================================
///
///     SquareMath by
///     - CantyCanadian
///
///====================================================================================================

// Calculates the length of a 2x2 square from its center.
float SquareLength(float angle)
{
	return lerp(1.0f, 1.4142f, -1.0f * (abs((angle % 90) - 45.0f) / 45.0f) + 1.0f);
}

// Calculates the length of a 2x2 square from its center, but with slight imperfections that makes my transitions look better.
float ImperfectSquareLength(float angle)
{
	return lerp(1.1f, 1.4142f, 0.5f * cos(4.0f * 3.1416f * (angle / 180.0f) + 3.1416f) + 0.5f);
}

// Turns a value from (0,0) (1,1) to (-1,-1) (1,1).
float2 CenterUV(float2 uv)
{
	return (uv * 2.0f) - float2(1.0f, 1.0f);
}

// Makes it so a UV value goes back to 0 after going past 1 (and vice-versa).
float2 RepeatUV(float2 uv)
{
	float2 result = uv;//abs(uv);

	result.x = result.x % 1.0f;
	result.y = result.y % 1.0f;

	return result;
}

// Makes it so, if the UV value is out of the (1,1) range, it mirrors on itself and tiles back to (0,0) (and vice-versa).
float2 MirrorUV(float2 uv)
{
	float2 result = RepeatUV(uv);

	result.x = (floor(uv.x) % 2.0f == 1) ? 1.0f - result.x : result.x;
	result.y = (floor(uv.y) % 2.0f == 1) ? 1.0f - result.y : result.y;

	return result;
}

// Returns true if UV is outside (0,0) or (1,1).
bool IsOutside11Range(float2 uv)
{
	return (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1);
}