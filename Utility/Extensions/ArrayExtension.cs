using UnityEngine;

public static class ArrayExtention
{
    /// <summary>
    /// Applies a passed function to every member of the array.
    /// </summary>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I>(this I[] target, System.Func<I, I> action)
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i] = action.Invoke(target[i]);
        }
    }

    /// <summary>
    /// Gets a random object from the array.
    /// </summary>
    /// <returns>Random object.</returns>
    public static I GetRandom<I>(this I[] target)
    {
        int index = Random.Range(0, target.Length);
        return target[index];
    }

    /// <summary>
    /// Returns a portion of the original array.
    /// </summary>
    /// <param name="start">First index.</param>
    /// <param name="end">Last index (non-included).</param>
    /// <returns>Portion of the original array.</returns>
    public static I[] Subdivide<I>(this I[] target, int start, int end)
    {
        if (end > target.Length)
        {
            Debug.LogError("Array Subdivide : End index larger than array length.");
        }
        else if (start < 0)
        {
            Debug.LogError("Array Subdivide : Start index under 0 (why did you do this?).");
        }

        I[] result = new I[end - start + 1];

        for(int i = start; i < end; i++)
        {
            result[i - start] = target[i];
        }

        return result;
    }
}
