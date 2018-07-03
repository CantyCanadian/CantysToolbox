using System.Collections.Generic;
using UnityEngine;

public static class ListExtention
{
    /// <summary>
    /// Adds an item to the list, but only if that item doesn't already exists.
    /// </summary>
    /// <param name="item">Item being added.</param>
    public static void AddOnce<I>(this List<I> target, I item)
    {
        if (!target.Contains(item))
        {
            target.Add(item);
        }
    }

    /// <summary>
    /// Adds a collection of items to the list, but only if those items don't already exist.
    /// </summary>
    /// <param name="items">Item set being added.</param>
    public static void AddRangeOnce<I>(this List<I> target, IEnumerable<I> items)
    {
        foreach(I i in items)
        {
            if (!target.Contains(i))
            {
                target.Add(i);
            }
        }
    }

    /// <summary>
    /// Adds an item to the list. If the list is longer than max, cull out the first values to make the list length equal to max.
    /// </summary>
    /// <param name="item">Item being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddClamp<I>(this List<I> target, I item, int max)
    {
        if (target.Count >= max)
        {
            int difference = target.Count - max + 1;
            target.RemoveRange(0, difference);
        }

        target.Add(item);
    }

    /// <summary>
    /// Adds a collection of items to the list. If the list is longer than max, cull out the first values to make the list length equal to max.
    /// </summary>
    /// <param name="items">Item set being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddRangeClamp<I>(this List<I> target, IEnumerable<I> items, int max)
    {
        int count = 0;
        foreach(I i in items)
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
    /// Add an item to the list, but only if those items don't already exist. If the list is longer than max, cull out the first values to make the list length equal to max.
    /// </summary>
    /// <param name="item">Item being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddOnceClamp<I>(this List<I> target, I item, int max)
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
    /// Add an item set to the list, but only if those items don't already exist. If the list is longer than max, cull out the first values to make the list length equal to max.
    /// </summary>
    /// <param name="items">Item set being added.</param>
    /// <param name="max">List maximum length.</param>
    public static void AddRangeOnceClamp<I>(this List<I> target, IEnumerable<I> items, int max)
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
    /// Removes any duplicates inside a list.
    /// </summary>
    /// <returns>Modified object.</returns>
    public static List<I> RemoveDuplicates<I>(this List<I> target)
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
    /// Extracts any duplicates inside a list.
    /// </summary>
    /// <returns>List containing any duplicates (but without any duplicate items).</returns>
    public static List<I> ExtractDuplicates<I>(this List<I> target)
    {
        Dictionary<I, bool> dupes = new Dictionary<I, bool>();

        foreach(I i in target)
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

        foreach(KeyValuePair<I, bool> kv in dupes)
        {
            if (kv.Value)
            {
                result.Add(kv.Key);
            }
        }

        return result;
    }

    /// <summary>
    /// Extracts duplicates inside a list. However, the item must be found over [dupeCount] times before being extracted.
    /// </summary>
    /// <param name="dupeCount">How many times the item needs to be found before being extracted.</param>
    /// <returns>List containing duplicates (but without any duplicate items).</returns>
    public static List<I> ExtractDuplicates<I>(this List<I> target, int dupeCount)
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
    /// </summary>
    /// <param name="sortBy">Sorting function. Will usually be written in the following style : (item1, item2) => comparison</param>
    /// <returns>Sorted list.</returns>
    public static List<I> Sort<I>(this List<I> target, System.Func<I, I, bool> sortBy)
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
            for(int y = 0; y < result.Count; y++)
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
    /// Applies a passed function to every member of the list.
    /// </summary>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I>(this List<I> target, System.Func<I, I> action)
    {
        for(int i = 0; i < target.Count; i++)
        {
            target[i] = action.Invoke(target[i]);
        }
    }

    /// <summary>
    /// Applies a passed function to every member of the 2D list.
    /// </summary>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I, J>(this List<I> target, System.Func<J, J> action) where I : List<J>
    {
        for (int i = 0; i < target.Count; i++)
        {
            for(int j = 0; j < target[i].Count; j++)
            {
                target[i][j] = action.Invoke(target[i][j]);
            }
        }
    }

    /// <summary>
    /// Gets a random object from the list.
    /// </summary>
    /// <returns>Random object.</returns>
    public static I GetRandom<I>(this List<I> target)
    {
        int index = Random.Range(0, target.Count);
        return target[index];
    }

    /// <summary>
    /// Converts the values of a list to another type using a passed-in conversion method.
    /// </summary>
    /// <typeparam name="NI">New value object type.</typeparam>
    /// <param name="valueConverter">Value conversion method.</param>
    /// <returns>List using new object types.</returns>
    public static List<NI> ConvertUsing<I, NI>(this List<I> target, System.Func<I, NI> valueConverter)
    {
        List<NI> result = new List<NI>();

        foreach (I value in target)
        {
            result.Add(valueConverter.Invoke(value));
        }

        return result;
    }
}
