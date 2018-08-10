using System;
using UnityEngine;

/// <summary>A collection of interpolation functions based on Robert Penner's easing equations that works using generic types casted to dynamic variable.
/// <para>The functions will assume that the passed-in variable type can be used for math equations. If it can't, problems with ensue.</para>
/// <para>Code will only work using .NET 4.6 because of the usage of dynamic variables.</para></summary>
public class CurvesUtil
{
    #region Linear

    /// <summary>
    /// Linear interpolation. y = x
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I Linear<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * delta);
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Exponential

    /// <summary>
    /// Exponential interpolation with ease-out. y = 2^x
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ExponentialEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (-1 * Mathf.Pow(2, -10 * delta) + 1));
        }
        catch(Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Exponential interpolation with ease-in. y = 2^x
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ExponentialEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Pow(2, 10 * (delta - 1))));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Exponential interpolation with acceleration near the middle. y = 2^x
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ExponentialEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Pow(2, 10 * (trueX - 1))));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (-1 * Mathf.Pow(2, -10 * trueX) + 1));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Exponential interpolation with deceleration near the middle. y = 2^y
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ExponentialEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (-1 * Mathf.Pow(2, -10 * trueX) + 1));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Pow(2, 10 * (trueX - 1))));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Circular

    /// <summary>
    /// Circular interpolation with ease-out. y = sqrt(1 - x^2)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CircularEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Sqrt(1 - Mathf.Pow(delta - 1, 2))));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Circular interpolation with ease-in. y = sqrt(1 - x^2)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CircularEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (-1 * Mathf.Sqrt(1 - Mathf.Pow(delta, 2)) + 1));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Exponential interpolation with acceleration near the middle. y = sqrt(1 - x^2)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CircularEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (-1 * Mathf.Sqrt(1 - Mathf.Pow(trueX, 2)) + 1));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Sqrt(1 - Mathf.Pow(trueX - 1, 2))));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Exponential interpolation with deceleration near the middle. y = sqrt(1 - x^2)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CircularEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Sqrt(1 - Mathf.Pow(trueX - 1, 2))));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (-1 * Mathf.Sqrt(1 - Mathf.Pow(trueX, 2)) + 1));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Quadratic

    /// <summary>
    /// Quadratic interpolation with ease-out. y = x^2
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuadraticEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (-1 * delta * (delta - 2)));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quadratic interpolation with ease-in. y = x^2
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuadraticEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Pow(delta, 2.0f)));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quadratic interpolation with acceleration near the middle. y = x^2
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuadraticEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX, 2.0f)));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (-1 * trueX * (trueX - 2)));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quadratic interpolation with deceleration near the middle. y = x^2
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuadraticEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (-1 * trueX * (trueX - 2)));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX, 2.0f)));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Sine

    /// <summary>
    /// Sine interpolation with ease-out. y = sin(x)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I SineEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Sin(delta * (Mathf.PI / 2.0f))));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Sine interpolation with ease-in. y = sin(x)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I SineEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Sin(delta * (Mathf.PI / 2.0f) + (1.5f * Mathf.PI)) + 1.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Sine interpolation with acceleration near the middle. y = sin(x)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I SineEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Sin(trueX * (Mathf.PI / 2.0f) + (1.5f * Mathf.PI)) + 1.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Sin(trueX * (Mathf.PI / 2.0f))));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Sine interpolation with deceleration near the middle. y = sin(x)
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I SineEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Sin(trueX * (Mathf.PI / 2.0f))));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Sin(trueX * (Mathf.PI / 2.0f) + (1.5f * Mathf.PI)) + 1.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Cubic

    /// <summary>
    /// Cubic interpolation with ease-out. y = x^3
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CubicEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Pow(delta - 1.0f, 3.0f) + 1.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Cubic interpolation with ease-in. y = x^3
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CubicEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * Mathf.Pow(delta, 3.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Cubic interpolation with acceleration near the middle. y = x^3
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CubicEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * Mathf.Pow(trueX, 3.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 3.0f) + 1.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Cubic interpolation with deceleration near the middle. y = x^3
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I CubicEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 3.0f) + 1.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * Mathf.Pow(trueX, 3.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Quartic

    /// <summary>
    /// Quartic interpolation with ease-out. y = x^4
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuarticEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (-1.0f * Mathf.Pow(delta - 1.0f, 4.0f) + 1.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quartic interpolation with ease-in. y = x^4
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuarticEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * Mathf.Pow(delta, 4.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quartic interpolation with acceleration near the middle. y = x^4
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuarticEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * Mathf.Pow(trueX, 4.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (-1.0f * Mathf.Pow(trueX - 1.0f, 4.0f) + 1.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quartic interpolation with deceleration near the middle. y = x^4
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuarticEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (-1.0f * Mathf.Pow(trueX - 1.0f, 4.0f) + 1.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * Mathf.Pow(trueX, 4.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Quintic

    /// <summary>
    /// Quintic interpolation with ease-out. y = x^5
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuinticEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Pow(delta - 1.0f, 5.0f) + 1.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quintic interpolation with ease-in. y = x^5
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuinticEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * Mathf.Pow(delta, 5.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quintic interpolation with acceleration near the middle. y = x^5
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuinticEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * Mathf.Pow(trueX, 5.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 5.0f) + 1.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Quintic interpolation with deceleration near the middle. y = x^5
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I QuinticEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 5.0f) + 1.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * Mathf.Pow(trueX, 5.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Elastic

    /// <summary>
    /// Elastic interpolation with ease-out. y = sin(x) with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ElasticEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Pow(2.0f, -10.0f * delta) * Mathf.Sin(((delta - 0.07f) * (2.0f * Mathf.PI)) / 0.3f) + 1.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Elastic interpolation with ease-in. y = sin(x) with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ElasticEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (-Mathf.Pow(2.0f, 10.0f * (delta - 1.0f)) * Mathf.Sin(((delta - 1.07f) * (2.0f * Mathf.PI)) / 0.3f)));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Elastic interpolation with acceleration near the middle. y = sin(x) with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ElasticEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (-Mathf.Pow(2.0f, 10.0f * (trueX - 1.0f)) * Mathf.Sin(((trueX - 1.07f) * (2.0f * Mathf.PI)) / 0.3f)));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Pow(2.0f, -10.0f * trueX) * Mathf.Sin(((trueX - 0.07f) * (2.0f * Mathf.PI)) / 0.3f) + 1.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Elastic interpolation with deceleration near the middle. y = sin(x) with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I ElasticEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Pow(2.0f, -10.0f * trueX) * Mathf.Sin(((trueX - 0.07f) * (2.0f * Mathf.PI)) / 0.3f) + 1.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (-Mathf.Pow(2.0f, 10.0f * (trueX - 1.0f)) * Mathf.Sin(((trueX - 1.07f) * (2.0f * Mathf.PI)) / 0.3f)));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Bounce

    /// <summary>
    /// Bounce interpolation with ease-out. y = x^2 with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BounceEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float a = 7.5625f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if(delta < (1.0f / 2.75f))
            {
                return xd1 + ((xd2 - xd1) * (a * Mathf.Pow(delta, 2.0f)));
            }
            else if (delta < (2.0f / 2.75f))
            {
                return xd1 + ((xd2 - xd1) * (a * Mathf.Pow(delta - (1.5f / 2.75f), 2.0f) + (3.0f / 4.0f)));
            }
            else if (delta < (2.5f / 2.75f))
            {
                return xd1 + ((xd2 - xd1) * (a * Mathf.Pow(delta - (2.25f / 2.75f), 2.0f) + (15.0f / 16.0f)));
            }
            else
            {
                return xd1 + ((xd2 - xd1) * (a * Mathf.Pow(delta - (2.625f / 2.75f), 2.0f) + (63.0f / 64.0f)));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Bounce interpolation with ease-in. y = x^2 with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BounceEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float a = 7.5625f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < (0.25f / 2.75f))
            {
                return xd1 + ((xd2 - xd1) * (a * -Mathf.Pow(delta - (0.125f / 2.75f), 2.0f) + (1.0f / 64.0f)));
            }
            else if (delta < (0.75f / 2.75f))
            {
                return xd1 + ((xd2 - xd1) * (a * -Mathf.Pow(delta - (0.5f / 2.75f), 2.0f) + (1.0f / 16.0f)));
            }
            else if (delta < (1.75f / 2.75f))
            {
                return xd1 + ((xd2 - xd1) * (a * -Mathf.Pow(delta - (1.25f / 2.75f), 2.0f) + (1.0f / 4.0f)));
            }
            else
            {
                return xd1 + ((xd2 - xd1) * (a * -Mathf.Pow(delta - 1.0f, 2.0f) + 1.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Bounce interpolation with acceleration near the middle. y = x^2 with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BounceEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 7.5625f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                if (trueX < (0.25f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - (0.125f / 2.75f), 2.0f) + (1.0f / 64.0f)));
                }
                else if (trueX < (0.75f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - (0.5f / 2.75f), 2.0f) + (1.0f / 16.0f)));
                }
                else if (trueX < (1.75f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - (1.25f / 2.75f), 2.0f) + (1.0f / 4.0f)));
                }
                else
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - 1.0f, 2.0f) + 1.0f));
                }
            }
            else
            {
                if (trueX < (1.0f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX, 2.0f)));
                }
                else if (trueX < (2.0f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX - (1.5f / 2.75f), 2.0f) + (3.0f / 4.0f)));
                }
                else if (trueX < (2.5f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX - (2.25f / 2.75f), 2.0f) + (15.0f / 16.0f)));
                }
                else
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX - (2.625f / 2.75f), 2.0f) + (63.0f / 64.0f)));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Bounce interpolation with deceleration near the middle. y = x^2 with exponential decay
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BounceEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 7.5625f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            { 
                if (trueX < (1.0f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX, 2.0f)));
                }
                else if (trueX < (2.0f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX - (1.5f / 2.75f), 2.0f) + (3.0f / 4.0f)));
                }
                else if (trueX < (2.5f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX - (2.25f / 2.75f), 2.0f) + (15.0f / 16.0f)));
                }
                else
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) * (a * Mathf.Pow(trueX - (2.625f / 2.75f), 2.0f) + (63.0f / 64.0f)));
                }
            }
            else
            {
                if (trueX < (0.25f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - (0.125f / 2.75f), 2.0f) + (1.0f / 64.0f)));
                }
                else if (trueX < (0.75f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - (0.5f / 2.75f), 2.0f) + (1.0f / 16.0f)));
                }
                else if (trueX < (1.75f / 2.75f))
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - (1.25f / 2.75f), 2.0f) + (1.0f / 4.0f)));
                }
                else
                {
                    return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (a * -Mathf.Pow(trueX - 1.0f, 2.0f) + 1.0f));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion

    #region Back

    /// <summary>
    /// Back interpolation with ease-out. y = x^3 with overshooting
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BackEaseOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float a = 1.70158f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Pow(delta - 1.0f, 2.0f) * ((a + 1.0f) * (delta - 1.0f) + a) + 1.0f));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Back interpolation with ease-in. y = x^3 with overshooting
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BackEaseIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float a = 1.70158f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            return xd1 + ((xd2 - xd1) * (Mathf.Pow(delta, 2.0f) * ((a + 1.0f) * delta - a)));
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Back interpolation with acceleration near the middle. y = x^3 with overshooting
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BackEaseInOut<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 1.70158f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX, 2.0f) * ((a + 1.0f) * trueX - a)));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 2.0f) * ((a + 1.0f) * (trueX - 1.0f) + a) + 1.0f));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    /// <summary>
    /// Back interpolation with deceleration near the middle. y = x^3 with overshooting
    /// </summary>
    /// <param name="x1">Value at 0.</param>
    /// <param name="x2">Value at 1.</param>
    /// <param name="delta">Interpolation percentage. [0, 1]</param>
    /// <returns>Interpolated value.</returns>
    public static I BackEaseOutIn<I>(I x1, I x2, float delta)
    {
        try
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 1.70158f;

            dynamic xd1 = x1;
            dynamic xd2 = x2;

            if (delta < 0.5f)
            {
                return xd1 + (((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 2.0f) * ((a + 1.0f) * (trueX - 1.0f) + a) + 1.0f));
            }
            else
            {
                return xd1 + (((xd2 - xd1) / 2.0f) + ((xd2 - xd1) / 2.0f) * (Mathf.Pow(trueX, 2.0f) * ((a + 1.0f) * trueX - a)));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Curves : Invalid data type used for curve operations. " + ex.TargetSite);
            return x1;
        }
    }

    #endregion
}