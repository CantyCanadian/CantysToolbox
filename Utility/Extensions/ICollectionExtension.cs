using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from ICollection. Contains utility functions that applies to any containers that can be counted.
/// </summary>
public static class ICollectionExtension
{
    /// <summary>
    /// Gets a random object from the ICollection.
    /// </summary>
    /// <returns>Random object.</returns>
    public static I GetRandom<I>(this ICollection<I> target)
    {
        int index = Random.Range(0, target.Count);
        I result = default(I);

        foreach(I item in target)
        {
            if (index == 0)
            {
                result = item;
            }
            else
            {
                index--;
            }
        }

        return result;
    }

    /// <summary>
    /// Converts ICollection to array.
    /// </summary>
    /// <returns>Array containing values inside ICollection.</returns>
    public static I[] ToArray<I>(this ICollection<I> target)
    {
        I[] result = new I[target.Count];
        target.CopyTo(result, 0);
        return result;
    }
}
