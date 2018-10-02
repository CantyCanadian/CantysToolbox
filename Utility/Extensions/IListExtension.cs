using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from ICollection. Contains utility functions that applies to any containers that can be indexed.
/// </summary>
public static class IListExtension
{
    /// <summary>
    /// Applies a passed function to every member of the IList. Passed in function changes the item itself.
    /// </summary>
    /// <param name="function">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I>(this IList<I> target, System.Func<I, I> function)
    {
        for (int i = 0; i < target.Count; i++)
        {
            target[i] = function.Invoke(target[i]);
        }
    }

    /// <summary>
    /// Applies a passed function to every member of the 2D list. Passed in function changes the item itself.
    /// </summary>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I, J>(this IList<I> target, System.Func<J, J> function) where I : IList<J>
    {
        for (int i = 0; i < target.Count; i++)
        {
            for (int j = 0; j < target[i].Count; j++)
            {
                target[i][j] = function.Invoke(target[i][j]);
            }
        }
    }

    /// <summary>
    /// Returns a portion of the original IList.
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
    /// Adds an item to the IList, but only if that item doesn't already exists.
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
    /// Adds a generic collection of items to IList, but only if those items don't already exist.
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
    /// Adds an item to the IList. If the IList is longer than max, cull out the first values to make the list length equal to max.
    /// </summary>
    /// <param name="item">Item being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddClamp<I>(this IList<I> target, I item, int max)
    {
        if (target.Count >= max)
        {
            int difference = target.Count - max + 1;
            target.RemoveRange(0, difference);
        }

        target.Add(item);
    }

    /// <summary>
    /// Adds a collection of items to the IList. If the IList is longer than max, cull out the first values to make the list length equal to max.
    /// </summary>
    /// <param name="items">Item set being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddRangeClamp<I>(this IList<I> target, IEnumerable<I> items, int max)
    {
        int count = 0;
        foreach (I i in items)
        {
            count++;
        }

        if (target.Count >= max)
        {
            int difference = target.Count - max + count;
            target.RemoveRange(0, difference);
        }

        target.AddRange(items);
    }

    /// <summary>
    /// Add an item to the IList, but only if those items don't already exist. If the list is longer than max, cull out the first values to make the IList length equal to max.
    /// </summary>
    /// <param name="item">Item being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddOnceClamp<I>(this IList<I> target, I item, int max)
    {
        if (!target.Contains(item))
        {
            if (target.Count >= max)
            {
                int difference = target.Count - max + 1;
                target.RemoveRange(0, difference);
            }

            target.Add(item);
        }
    }

    /// <summary>
    /// Add an item set to the IList, but only if those items don't already exist. If the IList is longer than max, cull out the first values to make the list length equal to max.
    /// </summary>
    /// <param name="items">Item set being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddRangeOnceClamp<I>(this IList<I> target, IEnumerable<I> items, int max)
    {
        int delete = 0;
        List<I> toAdd = new List<I>();
        foreach (I i in items)
        {
            if (!target.Contains(i))
            {
                delete++;
                toAdd.Add(i);
            }
        }

        if (target.Count >= max)
        {
            int difference = target.Count - max + delete;
            target.RemoveRange(0, difference);
        }

        target.AddRange(toAdd);
    }

    /// <summary>
    /// Removes multiple items at once..
    /// </summary>
    /// <param name="firstIndex">First item being removed.</param>
    /// <param name="removeCount">How many items are being removed.</param>
    public static void RemoveRange<I>(this IList<I> target, int firstIndex, int removeCount)
    {
        if (firstIndex + removeCount > target.Count)
        {
            Debug.LogError("IListExtension : Trying to remove too many items from IList using RemoveRange.");
            return;
        }

        for (int i = 0; i < removeCount; i++)
        {
            target.RemoveAt(firstIndex);
        }
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
    /// Removes the last item in the IList and returns it.
    /// </summary>
    /// <returns>Last item in the IList.</returns>
    public static I Pop<I>(this IList<I> target)
    {
        if (target.Count == 0)
        {
            return default(I);
        }

        I obj = target[target.Count - 1];
        target.Remove(obj);
        return obj;
    }
}
