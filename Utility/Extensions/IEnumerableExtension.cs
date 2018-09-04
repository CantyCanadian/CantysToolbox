using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains utility functions that applies to any containers that can be iterated upon.
/// </summary>
public static class IEnumerableExtension
{
    /// <summary>
    /// Applies a passed function to every member of the IEnumerable. Passed in function cannot change the item reference.
    /// </summary>
    /// <param name="action">Function with a single Item argument.</param>
    public static void DoOnAll<I>(this IEnumerable<I> target, System.Action<I> action)
    {
        foreach(I item in target)
        {
            action.Invoke(item);
        }
    }

    /// <summary>
    /// Applies a passed function to every member of the 2D IEnumerable. Passed in function cannot change the item reference.
    /// </summary>
    /// <param name="action">Function with a single Item argument.</param>
    public static void DoOnAll<I, J>(this IEnumerable<I> target, System.Func<J, J> action) where I : IEnumerable<J>
    {
        foreach (I itemX in target)
        {
            foreach (J itemY in itemX)
            {
                action.Invoke(itemY);
            }
        }
    }

    /// <summary>
    /// Return a debug string containing every values, separated by commas.
    /// </summary>
    /// <returns>String containing every values.</returns>
    public static string ToDebugString<I>(this IEnumerable<I> target)
    {
        string result = "";

        foreach (I item in target)
        {
            result += item.ToString() + ", ";
        }

        result = result.Remove(result.Length - 3, 2);

        return result;
    }

    /// <summary>
    /// Removes any duplicates inside a IEnumerable.
    /// </summary>
    /// <returns>Modified object.</returns>
    public static IEnumerable<I> RemoveDuplicates<I>(this IEnumerable<I> target)
    {
        List<I> result = new List<I>();

        foreach (I item in target)
        {
            if (!result.Contains(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    /// <summary>
    /// Extracts any duplicates inside a IEnumerable.
    /// </summary>
    /// <returns>IEnumerable containing any duplicates (but without any duplicate items).</returns>
    public static IEnumerable<I> ExtractDuplicates<I>(this IEnumerable<I> target)
    {
        Dictionary<I, bool> dupes = new Dictionary<I, bool>();

        foreach (I item in target)
        {
            if (dupes.ContainsKey(item))
            {
                dupes[item] = true;
            }
            else
            {
                dupes.Add(item, false);
            }
        }

        List<I> result = new List<I>();

        foreach (KeyValuePair<I, bool> kv in dupes)
        {
            if (kv.Value)
            {
                result.Add(kv.Key);
            }
        }

        return result;
    }

    /// <summary>
    /// Extracts duplicates inside a IEnumerable. However, the item must be found over [dupeCount] times before being extracted.
    /// </summary>
    /// <param name="dupeCount">How many times the item needs to be found before being extracted.</param>
    /// <returns>IEnumerable containing duplicates (but without any duplicate items).</returns>
    public static IEnumerable<I> ExtractDuplicates<I>(this IEnumerable<I> target, int dupeCount)
    {
        Dictionary<I, int> dupes = new Dictionary<I, int>();

        foreach (I i in target)
        {
            if (dupes.ContainsKey(i))
            {
                dupes[i]++;
            }
            else
            {
                dupes.Add(i, 1);
            }
        }

        List<I> result = new List<I>();

        foreach (KeyValuePair<I, int> kv in dupes)
        {
            if (kv.Value >= dupeCount)
            {
                result.Add(kv.Key);
            }
        }

        return result;
    }

    /// <summary>
    /// Converts the IEnumerable from type I to type NI using a passed in conversion function.
    /// </summary>
    /// <param name="valueConverter">Conversion function taking a item I and returning an item NI.</param>
    /// <returns>IEnumerable containing original list, but converted.</returns>
    public static IEnumerable<NI> ConvertUsing<I, NI>(this IEnumerable<I> target, System.Func<I, NI> valueConverter)
    {
        List<NI> result = new List<NI>();

        foreach (I value in target)
        {
            result.Add(valueConverter.Invoke(value));
        }

        return result;
    }
}
