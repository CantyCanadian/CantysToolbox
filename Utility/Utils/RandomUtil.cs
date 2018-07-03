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
        int result = -1;
        int loop = 0;

        while (loop < 20) //You'd assume it would never go 20 times without touching an exception. If it does, you have no possible results.
        {
            result = Random.Range(min, max);

            if (!container.Contains(result))
            {
                break;
            }

            loop++;
        }

        return result;
    }

    /// <summary>
    /// Extension of Unity's Random.Range function, but allows the user to put more chances behind certain numbers.
    /// </summary>
    /// <param name="min">Minimum value [inclusive]</param>
    /// <param name="max">Maximum value [exclusive]</param>
    /// <param name="emphasisPower">How many extra time this number has the chance to be picked up.</param>
    /// <param name="emphasisNumbers">The numbers who gets extra chances.</param>
    /// <returns>Random number.</returns>
    public static int RangeEmphasis(int min, int max, int emphasisPower, params int[] emphasisNumbers)
    {
        List<int> numbers = new List<int>();

        for(int i = min; i < max; i++)
        {
            numbers.Add(i);
        }

        for (int i = 0; i < emphasisNumbers.Length; i++)
        {
            for (int j = 0; j < emphasisPower; j++)
            {
                numbers.Add(emphasisNumbers[i]);
            }
        }

        return numbers[Random.Range(0, numbers.Count)];
    }

    /// <summary>
    /// Extension of Unity's Random.Range function, but allows the user to put more chances behind certain numbers.
    /// </summary>
    /// <param name="min">Minimum value [inclusive]</param>
    /// <param name="max">Maximum value [exclusive]</param>
    /// <param name="emphasisPower">How many extra time this number has the chance to be picked up.</param>
    /// <param name="emphasisNumbers">The numbers who gets extra chances.</param>
    /// <returns>Random number.</returns>
    public static int RangeEmphasis(int min, int max, int emphasisPower, ICollection<int> emphasisNumbers)
    {
        List<int> numbers = new List<int>();

        for (int i = min; i < max; i++)
        {
            numbers.Add(i);
        }

        List<int> emphasis = new List<int>(emphasisNumbers);
        for (int i = 0; i < emphasis.Count; i++)
        {
            for (int j = 0; j < emphasisPower; j++)
            {
                numbers.Add(emphasis[i]);
            }
        }

        return numbers[Random.Range(0, numbers.Count)];
    }

    /// <summary>
    /// Implementation of Unity's Random.Range function, but to return a bool instead of a number.
    /// </summary>
    /// <returns>Random bool.</returns>
    public static bool RandomBool()
    {
        return Random.Range(0, 2) == 0 ? false : true;
    }
}
