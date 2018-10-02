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