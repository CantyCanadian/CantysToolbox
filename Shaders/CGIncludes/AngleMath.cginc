///====================================================================================================
///
///     AngleMath by
///     - CantyCanadian
///
///====================================================================================================

// CG implementation of Vector2.Angle().
float AngleBetween(float2 v1, float2 v2)
{
	return degrees(acos(dot(v1, v2) / (length(v1) * length(v2))));
}

// CG implementation of Vector3.Angle().
float AngleBetween(float3 v1, float3 v2)
{
	return degrees(acos(dot(v1, v2) / (length(v1) * length(v2))));
}

// Return point from the unit circle at given angle.
float2 UnitVector(float angle)
{
	return float2(cos(radians(angle)), sin(radians(angle)));
}

// Rotates value around 0,0 by the given angle.
float2 RotateAroundCenter(float2 value, float angle)
{
	return float2(value.x * cos(radians(angle)) - value.y * sin(radians(angle)), value.x * sin(radians(angle)) + value.y * cos(radians(angle)));
}

float4 EulerToQuaternion(float3 euler)
{
	float4 quaternion;
	float3 radEuler = float3(radians(euler.x), radians(euler.y), radians(euler.z));

	double cx = cos(radEuler.x * 0.5);
	double sx = sin(radEuler.x * 0.5);
	double cy = cos(radEuler.y * 0.5);
	double sy = sin(radEuler.y * 0.5);
	double cz = cos(radEuler.z * 0.5);
	double sz = sin(radEuler.z * 0.5);

	quaternion.w = cy * cz * cx + sy * sz * sx;
	quaternion.x = cy * sz * cx - sy * cz * sx;
	quaternion.y = cy * cz * sx + sy * sz * cx;
	quaternion.z = sy * cz * cx - cy * sz * sx;

	return quaternion;
}

float3 QuaternionToEuler(float4 quaternion)
{
	float x0 = 2.0 * (quaternion.w * quaternion.x + quaternion.y * quaternion.z);
	float x1 = 1.0 - 2.0 * (quaternion.x * quaternion.x + quaternion.y * quaternion.y);
	float x = atan2(x0, x1);

	float y0 = 2.0 * (quaternion.w * quaternion.y - quaternion.z * quaternion.x);
	if (y0 > 1.0) 
	{
		y0 = 1.0;
	} 
	else if (y0 < -1.0)
	{
		y0 = -1.0;
	}
	float y = asin(y0);

	float z0 = 2.0 * (quaternion.w * quaternion.z + quaternion.x * quaternion.y);
	float z1 = 1.0 - 2.0 * (quaternion.y * quaternion.y + quaternion.z * quaternion.z);
	float z = atan2(z0, z1);

	return float3(x, y, z);
}