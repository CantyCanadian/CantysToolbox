using System.Collections.Generic;
using UnityEngine;

public static class ListExtention
{
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
