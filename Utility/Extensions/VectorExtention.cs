using UnityEngine;

public static class VectorExtention
{
    #region Multiply

    /// <summary>
    /// Simple vector multiplication function.
    /// </summary>
    /// <param name="vectors">Vectors to add.</param>
    /// <returns>The multiplied vector.</returns>
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
    /// <returns>The multiplied vector.</returns>
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
    /// <returns>The multiplied vector.</returns>
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
    /// <returns>The clamped vector.</returns>
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
    /// <returns>The clamped vector.</returns>
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
    /// <returns>The clamped vector.</returns>
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
    /// <returns>The clamped vector.</returns>
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
    /// <returns>The clamped vector.</returns>
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
    /// <returns>The clamped vector.</returns>
    public static Vector4 Clamp(this Vector4 target, Vector4 min, Vector4 max)
    {
        target.x = Mathf.Clamp(target.x, min.x, max.x);
        target.y = Mathf.Clamp(target.y, min.y, max.y);
        target.z = Mathf.Clamp(target.z, min.z, max.z);
        target.w = Mathf.Clamp(target.w, min.w, max.w);

        return target;
    }

    #endregion

    #region Sign

    /// <summary>
    /// Return vector filled with its original values passed through Mathf.Sign.
    /// </summary>
    /// <returns>The sign vector.</returns>
    public static Vector2 Sign(this Vector2 target)
    {
        return new Vector2(Mathf.Sign(target.x), Mathf.Sign(target.y));
    }

    /// <summary>
    /// Return vector filled with its original values passed through Mathf.Sign.
    /// </summary>
    /// <returns>The sign vector.</returns>
    public static Vector3 Sign(this Vector3 target)
    {
        return new Vector3(Mathf.Sign(target.x), Mathf.Sign(target.y), Mathf.Sign(target.z));
    }

    /// <summary>
    /// Return vector filled with its original values passed through Mathf.Sign.
    /// </summary>
    /// <returns>The sign vector.</returns>
    public static Vector4 Sign(this Vector4 target)
    {
        return new Vector4(Mathf.Sign(target.x), Mathf.Sign(target.y), Mathf.Sign(target.z), Mathf.Sign(target.w));
    }

    #endregion

    #region SignAdd

    /// <summary>
    /// Adds a value to the vector, but if the vector is negative, it changes the value to negative.
    /// </summary>
    /// <param name="value">Value to add.</param>
    /// <returns>The added vector.</returns>
    public static Vector2 SignAdd(this Vector2 target, float value)
    {
        target.x += value * Mathf.Sign(target.x);
        target.y += value * Mathf.Sign(target.y);

        return target;
    }

    /// <summary>
    /// Adds a value to the vector, but if the vector is negative, it changes the value to negative.
    /// </summary>
    /// <param name="value">Value to add.</param>
    /// <returns>The added vector.</returns>
    public static Vector3 SignAdd(this Vector3 target, float value)
    {
        target.x += value * Mathf.Sign(target.x);
        target.y += value * Mathf.Sign(target.y);
        target.z += value * Mathf.Sign(target.z);

        return target;
    }

    /// <summary>
    /// Adds a value to the vector, but if the vector is negative, it changes the value to negative.
    /// </summary>
    /// <param name="value">Value to add.</param>
    /// <returns>The added vector.</returns>
    public static Vector4 SignAdd(this Vector4 target, float value)
    {
        target.x += value * Mathf.Sign(target.x);
        target.y += value * Mathf.Sign(target.y);
        target.z += value * Mathf.Sign(target.z);
        target.w += value * Mathf.Sign(target.w);

        return target;
    }

    #endregion

    #region SignMinus

    /// <summary>
    /// Minus a value to the vector, but if the vector is negative, it changes the value to negative.
    /// </summary>
    /// <param name="value">Value to remove.</param>
    /// <returns>The minused vector.</returns>
    public static Vector2 SignMinus(this Vector2 target, float value)
    {
        target.x -= Mathf.Abs(target.x) <= Mathf.Abs(value) ? target.x : value * Mathf.Sign(target.x);
        target.y -= Mathf.Abs(target.y) <= Mathf.Abs(value) ? target.y : value * Mathf.Sign(target.y);

        return target;
    }

    /// <summary>
    /// Minus a value to the vector, but if the vector is negative, it changes the value to negative.
    /// </summary>
    /// <param name="value">Value to remove.</param>
    /// <returns>The minused vector.</returns>
    public static Vector3 SignMinus(this Vector3 target, float value)
    {
        target.x -= Mathf.Abs(target.x) <= Mathf.Abs(value) ? target.x : value * Mathf.Sign(target.x);
        target.y -= Mathf.Abs(target.y) <= Mathf.Abs(value) ? target.y : value * Mathf.Sign(target.y);
        target.z -= Mathf.Abs(target.z) <= Mathf.Abs(value) ? target.z : value * Mathf.Sign(target.z);

        return target;
    }

    /// <summary>
    /// Minus a value to the vector, but if the vector is negative, it changes the value to negative.
    /// </summary>
    /// <param name="value">Value to remove.</param>
    /// <returns>The minused vector.</returns>
    public static Vector4 SignMinus(this Vector4 target, float value)
    {
        target.x -= Mathf.Abs(target.x) <= Mathf.Abs(value) ? target.x : value * Mathf.Sign(target.x);
        target.y -= Mathf.Abs(target.y) <= Mathf.Abs(value) ? target.y : value * Mathf.Sign(target.y);
        target.z -= Mathf.Abs(target.z) <= Mathf.Abs(value) ? target.z : value * Mathf.Sign(target.z);
        target.w -= Mathf.Abs(target.w) <= Mathf.Abs(value) ? target.w : value * Mathf.Sign(target.w);

        return target;
    }

    #endregion

    #region MultiplicativeVector

    /// <summary>
    /// Returns a vector with values set to 0 if the target's value was set to 0 or 1 if the target had any value. Used whenever you need to change the values of a vector, but only when said values aren't 0.
    /// </summary>
    /// <returns>A vector composed of either 0s or 1s.</returns>
    public static Vector2 MultiplicativeVector(this Vector2 target)
    {
        return new Vector2(target.x == 0.0f ? 0.0f : 1.0f, target.y == 0.0f ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Returns a vector with values set to 0 if the target's value was set to 0 or 1 if the target had any value. Used whenever you need to change the values of a vector, but only when said values aren't 0.
    /// </summary>
    /// <returns>A vector composed of either 0s or 1s.</returns>
    public static Vector3 MultiplicativeVector(this Vector3 target)
    {
        return new Vector3(target.x == 0.0f ? 0.0f : 1.0f, target.y == 0.0f ? 0.0f : 1.0f, target.z == 0.0f ? 0.0f : 1.0f);
    }

    /// <summary>
    /// Returns a vector with values set to 0 if the target's value was set to 0 or 1 if the target had any value. Used whenever you need to change the values of a vector, but only when said values aren't 0.
    /// </summary>
    /// <returns>A vector composed of either 0s or 1s.</returns>
    public static Vector4 MultiplicativeVector(this Vector4 target)
    {
        return new Vector4(target.x == 0.0f ? 0.0f : 1.0f, target.y == 0.0f ? 0.0f : 1.0f, target.z == 0.0f ? 0.0f : 1.0f, target.w == 0.0f ? 0.0f : 1.0f);
    }

    #endregion

    #region Subdivision

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 xy(this Vector3 target)
    {
        return new Vector2(target.x, target.y);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 xz(this Vector3 target)
    {
        return new Vector2(target.x, target.z);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 yz(this Vector3 target)
    {
        return new Vector2(target.y, target.z);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 xy(this Vector4 target)
    {
        return new Vector2(target.x, target.y);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 xz(this Vector4 target)
    {
        return new Vector2(target.x, target.z);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 xw(this Vector4 target)
    {
        return new Vector2(target.x, target.w);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 yz(this Vector4 target)
    {
        return new Vector2(target.y, target.z);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 yw(this Vector4 target)
    {
        return new Vector2(target.y, target.w);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector2 zw(this Vector4 target)
    {
        return new Vector2(target.z, target.w);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector3 xyz(this Vector4 target)
    {
        return new Vector3(target.x, target.y, target.z);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector3 xyw(this Vector4 target)
    {
        return new Vector3(target.x, target.y, target.w);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector3 xzw(this Vector4 target)
    {
        return new Vector3(target.x, target.z, target.w);
    }

    /// <summary>
    /// Returns a vector made with the requested variables.
    /// </summary>
    /// <returns>Subdivided vector.</returns>
    public static Vector3 yzw(this Vector4 target)
    {
        return new Vector3(target.y, target.z, target.w);
    }

    #endregion
}