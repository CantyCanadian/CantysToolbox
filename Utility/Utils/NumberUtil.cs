using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumberUtil
{
    /// <summary>
    /// Decrements a value, wrapping to max if it goes under min.
    /// </summary>
    /// <param name="value">Original value.</param>
    /// <param name="minus">Value to decrement by.</param>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>Returns decremented index.</returns>
    public static int DecrementWrap(int value, int minus, int min, int max)
    {
        value -= minus;

        if (value < min)
        {
            value = max;
        }

        return value;
    }

    /// <summary>
    /// Increments a value, wrapping to min if it goes over max.
    /// </summary>
    /// <param name="value">Original value.</param>
    /// <param name="minus">Value to increment by.</param>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    /// <returns>Returns incremented index.</returns>
    public static int IncrementWrap(int value, int add, int min, int max)
    {
        value += add;

        if (value > max)
        {
            value = min;
        }

        return value;
    }

    /// <summary>
    /// Equivalent to the Mathf.Sign function, but for integers. Returns -1 if negative, 1 if positive, 0 if zero.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <returns>Sign result.</returns>
    public static int Sign(int value)
    {
        return (value > 0 ? 1 : 0) - (value < 0 ? 1 : 0);
    }
}
