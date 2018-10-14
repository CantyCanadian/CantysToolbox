///====================================================================================================
///
///     MatrixMath by
///     - CantyCanadian
///
///====================================================================================================

float4x4 CreateIdentityMatrix()
{
	float4x4 identity;

	identity[0][0] = 1;
	identity[1][0] = 0;
	identity[2][0] = 0;
	identity[3][0] = 0;

	identity[0][1] = 0;
	identity[1][1] = 1;
	identity[2][1] = 0;
	identity[3][1] = 0;

	identity[0][2] = 0;
	identity[1][2] = 0;
	identity[2][2] = 1;
	identity[3][2] = 0;

	identity[0][3] = 0;
	identity[1][3] = 0;
	identity[2][3] = 0;
	identity[3][3] = 1;

	return identity;
}

float4x4 CreateTranslationMatrix(float3 position)
{
	float4x4 translation = CreateIdentityMatrix();

	translation[3][0] = position.x;
	translation[3][1] = position.y;
	translation[3][2] = position.z;

	return translation;
}

float4x4 CreateRotationMatrix(float3 euler)
{
	float3 radEuler = float3(radians(euler.x), radians(euler.y), radians(euler.z));

	float4x4 rotationX = CreateIdentityMatrix();
	float4x4 rotationY = CreateIdentityMatrix();
	float4x4 rotationZ = CreateIdentityMatrix();

	rotationX[1][1] = cos(radEuler.x);
	rotationX[2][1] = -sin(radEuler.x);
	rotationX[1][2] = sin(radEuler.x);
	rotationX[2][2] = cos(radEuler.x);

	rotationY[0][0] = cos(radEuler.x);
	rotationY[2][0] = sin(radEuler.x);
	rotationY[0][2] = -sin(radEuler.x);
	rotationY[2][2] = cos(radEuler.x);

	rotationZ[0][0] = cos(radEuler.x);
	rotationZ[1][0] = sin(radEuler.x);
	rotationZ[0][1] = -sin(radEuler.x);
	rotationZ[1][1] = cos(radEuler.x);

	return rotationX * rotationY * rotationZ;
}

float4x4 CreateScalingMatrix(float3 scale)
{
	float4x4 size = CreateIdentityMatrix();

	size[0][0] = scale.x;
	size[1][1] = scale.y;
	size[2][2] = scale.z;

	return size;
}

float4x4 CreateTRSMatrix(float3 position, float3 euler, float3 scale)
{
	return CreateScalingMatrix(scale) * CreateRotationMatrix(euler) * CreateTranslationMatrix(position);
}