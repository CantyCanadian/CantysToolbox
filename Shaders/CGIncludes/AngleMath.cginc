// CG implementation of Vector2.Angle().
float AngleBetween(float2 v1, float2 v2)
{
	return dot(v1, v2) / (length(v1) * length(v2));
}

// CG implementation of Vector3.Angle().
float AngleBetween(float3 v1, float3 v2)
{
	return dot(v1, v2) / (length(v1) * length(v2));
}

// Return point from the unit circle at given angle.
float2 UnitVector(float angle)
{
	return float2(cos(radians(angle)), sin(radians(angle)));
}

float2 RotateAroundCenter(float2 value, float angle)
{
	return float2(value.x * cos(radians(angle)) - value.y * sin(radians(angle)), value.x * sin(radians(angle)) + value.y * cos(radians(angle)));
}