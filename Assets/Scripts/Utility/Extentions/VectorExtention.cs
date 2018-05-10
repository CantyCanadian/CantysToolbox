using UnityEngine;

public static class VectorExtention
{
    #region Multiply

    /// <summary>
    /// Simple vector multiplication function.
    /// </summary>
    /// <param name="vectors">Vectors to add.</param>
    /// <returns>A copy of the multiplied vector.</returns>
    public static Vector2 Multiply(this Vector2 target, params Vector2[] vectors)
    {
        for(int i = 0; i < vectors.Length; i++)
        {
            target.x *= vectors[i].x;
            target.y *= vectors[i].y;
        }

        return target;
    }

    /// <summary>
    /// Simple vector multiplication function.
    /// </summary>
    /// <param name="vectors">Vectors to add.</param>
    /// <returns>A copy of the multiplied vector.</returns>
    public static Vector3 Multiply(this Vector3 target, params Vector3[] vectors)
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            target.x *= vectors[i].x;
            target.y *= vectors[i].y;
            target.z *= vectors[i].z;
        }

        return target;
    }

    /// <summary>
    /// Simple vector multiplication function.
    /// </summary>
    /// <param name="vectors">Vectors to add.</param>
    /// <returns>A copy of the multiplied vector.</returns>
    public static Vector4 Multiply(this Vector4 target, params Vector4[] vectors)
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            target.w *= vectors[i].w;
            target.x *= vectors[1].x;
            target.y *= vectors[i].y;
            target.z *= vectors[i].z;
        }

        return target;
    }

    #endregion

    #region Clamp

    /// <summary>
    /// Clamps every vector values using floats.
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>A copy of the clamped vector.</returns>
    public static Vector2 Clamp(this Vector2 target, float min, float max)
    {
        target.x = Mathf.Clamp(target.x, min, max);
        target.y = Mathf.Clamp(target.y, min, max);

        return target;
    }

    /// <summary>
    /// Clamps every vector values using vectors.
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>A copy of the clamped vector.</returns>
    public static Vector2 Clamp(this Vector2 target, Vector2 min, Vector2 max)
    {
        target.x = Mathf.Clamp(target.x, min.x, max.x);
        target.y = Mathf.Clamp(target.y, min.y, max.y);

        return target;
    }

    /// <summary>
    /// Clamps every vector values using floats.
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>A copy of the clamped vector.</returns>
    public static Vector3 Clamp(this Vector3 target, float min, float max)
    {
        target.x = Mathf.Clamp(target.x, min, max);
        target.y = Mathf.Clamp(target.y, min, max);
        target.z = Mathf.Clamp(target.z, min, max);

        return target;
    }

    /// <summary>
    /// Clamps every vector values using vectors.
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>A copy of the clamped vector.</returns>
    public static Vector3 Clamp(this Vector3 target, Vector3 min, Vector3 max)
    {
        target.x = Mathf.Clamp(target.x, min.x, max.x);
        target.y = Mathf.Clamp(target.y, min.y, max.y);
        target.z = Mathf.Clamp(target.z, min.z, max.z);

        return target;
    }

    /// <summary>
    /// Clamps every vector values using floats.
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>A copy of the clamped vector.</returns>
    public static Vector4 Clamp(this Vector4 target, float min, float max)
    {
        target.x = Mathf.Clamp(target.x, min, max);
        target.y = Mathf.Clamp(target.y, min, max);
        target.z = Mathf.Clamp(target.z, min, max);
        target.w = Mathf.Clamp(target.w, min, max);

        return target;
    }

    /// <summary>
    /// Clamps every vector values using vectors.
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>A copy of the clamped vector.</returns>
    public static Vector4 Clamp(this Vector4 target, Vector4 min, Vector4 max)
    {
        target.x = Mathf.Clamp(target.x, min.x, max.x);
        target.y = Mathf.Clamp(target.y, min.y, max.y);
        target.z = Mathf.Clamp(target.z, min.z, max.z);
        target.w = Mathf.Clamp(target.w, min.w, max.w);

        return target;
    }

    #endregion
}