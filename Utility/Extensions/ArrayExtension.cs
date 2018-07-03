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
}
