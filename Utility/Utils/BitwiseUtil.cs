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

    /// <summary>
    /// Merges an array of booleans into a ints for storage or compression in packs of 32.
    /// </summary>
    /// <param name="valuesInOrder">Bools that gets merged. Make sure the order is right in your array.</param>
    /// <returns>Unsigned int containing bools as bits.</returns>
    public static uint[] MergeBoolsToInt(bool[] valuesInOrder)
    {
        int[] result = new int[Mathf.FloorToInt(valuesInOrder.Length / 32) + 1];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = 0;

            for (int j = 0; j < (int)Mathf.Min(valuesInOrder.Length - (i * 32), 32); j++)
            {
                if (valuesInOrder[i])
                {
                    SetBitAtPosition(ref result, i);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Separates an int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool[] SplitIntIntoBools(int value)
    {
        bool[] result = new bool[32];

        for(int i = 0; i < result.Length; i++)
        {
            result[i] = GetBitAtPosition(value, i);
        }
    }

    public static bool GetBitAtPosition(int value, int position)
    {
        return (value >> position) & 1;
    }

    public static void SetBitAtPosition(ref int value, int position)
    {
        value |= 1 << position;
    }

    public static void ClearBitAtPosition(ref int value, int position)
    {
        value &= ~(1 << position);
    }

    public static void ToggleBitAtPosition(ref int value, int position)
    {
        value ^= 1 << position;
    }
}
