using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IListExtension
{
    /// <summary>
    /// Applies a passed function to every member of the collection.
    /// </summary>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I>(this IList<I> target, System.Func<I, I> action)
    {
        for (int i = 0; i < target.Count; i++)
        {
            target[i] = action.Invoke(target[i]);
        }
    }

    /// <summary>
    /// Gets a random object from the array.
    /// </summary>
    /// <returns>Random object.</returns>
    public static I GetRandom<I>(this IList<I> target)
    {
        int index = Random.Range(0, target.Count);
        return target[index];
    }

    /// <summary>
    /// Return a debug string containing every values, separated by commas.
    /// </summary>
    /// <returns>String containing every values.</returns>
    public static string ToDebugString<I>(this IList<I> target)
    {
        string result = "";

        for (int i = 0; i < target.Count - 1; i++)
        {
            result += target[i].ToString() + ", ";
        }

        result += target[target.Count - 1].ToString();

        return result;
    }

    /// <summary>
    /// Returns a portion of the original collection.
    /// </summary>
    /// <param name="start">First index.</param>
    /// <param name="end">Last index (non-included).</param>
    /// <returns>Portion of the original collection.</returns>
    public static IList<I> Subdivide<I>(this IList<I> target, int start, int end)
    {
        if (end > target.Count)
        {
            Debug.LogError("Collection Subdivide : End index larger than array length.");
        }
        else if (start < 0)
        {
            Debug.LogError("Collection Subdivide : Start index under 0 (why did you do this?).");
        }

        I[] result = new I[end - start + 1];

        for (int i = start; i < end; i++)
        {
            result[i - start] = target[i];
        }

        return result;
    }

    /// <summary>
    /// Adds an item to the collection, but only if that item doesn't already exists.
    /// </summary>
    /// <param name="item">Item being added.</param>
    /// <returns>If the value got added.</returns>
    public static bool AddOnce<I>(this IList<I> target, I item)
    {
        if (!target.Contains(item))
        {
            target.Add(item);
            return true;
        }

        return false;
    }


    /// <summary>
    /// Adds a collection of items to another collection, but only if those items don't already exist.
    /// </summary>
    /// <param name="items">Item set being added.</param>
    public static void AddRangeOnce<I>(this IList<I> target, IEnumerable<I> items)
    {
        foreach (I i in items)
        {
            if (!target.Contains(i))
            {
                target.Add(i);
            }
        }
    }


    /// <summary>
    /// Removes any duplicates inside a collection.
    /// </summary>
    /// <returns>Modified object.</returns>
    public static IList<I> RemoveDuplicates<I>(this IList<I> target)
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
    /// Extracts any duplicates inside a collection.
    /// </summary>
    /// <returns>Collection containing any duplicates (but without any duplicate items).</returns>
    public static IList<I> ExtractDuplicates<I>(this IList<I> target)
    {
        Dictionary<I, bool> dupes = new Dictionary<I, bool>();

        foreach (I i in target)
        {
            if (dupes.ContainsKey(i))
            {
                dupes[i] = true;
            }
            else
            {
                dupes.Add(i, false);
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
    /// Extracts duplicates inside a collection. However, the item must be found over [dupeCount] times before being extracted.
    /// </summary>
    /// <param name="dupeCount">How many times the item needs to be found before being extracted.</param>
    /// <returns>Collection containing duplicates (but without any duplicate items).</returns>
    public static IList<I> ExtractDuplicates<I>(this IList<I> target, int dupeCount)
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
    /// Extention of the Sort function. Takes a simple bool function in a similar fashion to Linq's OrderBy function. If the function returns true, it'll get placed before.
    /// <para>If you want pre-existing number sorting algorithms, use SortUtil functions. This is mostly for non-ascending comparisons.</para>
    /// </summary>
    /// <param name="sortBy">Sorting function. Will usually be written in the following style : (item1, item2) => comparison</param>
    /// <returns>Sorted list.</returns>
    public static IList<I> Sort<I>(this IList<I> target, System.Func<I, I, bool> sortBy)
    {
        List<I> result = new List<I>();

        for (int x = 0; x < target.Count; x++)
        {
            if (x == 0)
            {
                result.Add(target[x]);
                continue;
            }

            bool exit = false;
            for (int y = 0; y < result.Count; y++)
            {
                if (sortBy(target[x], result[y]))
                {
                    result.Insert(y, target[x]);
                    exit = true;
                    break;
                }
            }

            if (!exit)
            {
                result.Add(target[x]);
            }
        }

        return result;
    }

    /// <summary>
    /// Applies a passed function to every member of the 2D list.
    /// </summary>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I, J>(this List<I> target, System.Func<J, J> action) where I : List<J>
    {
        for (int i = 0; i < target.Count; i++)
        {
            for (int j = 0; j < target[i].Count; j++)
            {
                target[i][j] = action.Invoke(target[i][j]);
            }
        }
    }
}
