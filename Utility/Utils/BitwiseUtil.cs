using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BitwiseUtil
{
    /// <summary>
    /// Counts how many bits within an integer is set to 1.
    /// </summary>
    /// <param name="value">Integer to work with.</param>
    /// <returns>Number of set bits.</returns>
    public static int CountSetBits(int value)
    {
        int count = 0;
        while (value > 0)
        {
            count += value & 1;
            value >>= 1;
        }

        return count;
    }
}
