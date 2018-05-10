using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomUtil
{
    /// <summary>
    /// Extension of Unity's Random.Range function, adding extra integers that the result CANNOT be.
    /// </summary>
    /// <param name="min">Minimum value [inclusive]</param>
    /// <param name="max">Maximum value [exclusive]</param>
    /// <param name="exceptions">Values that can't be the result.</param>
    /// <returns>Random value.</returns>
    public static int RangeExcept(int min, int max, params int[] exceptions)
    {
        List<int> container = new List<int>(exceptions);
        int result;

        while (true)
        {
            result = Random.Range(min, max);

            if (!container.Contains(result))
            {
                break;
            }
        }

        return result;
    }
}
