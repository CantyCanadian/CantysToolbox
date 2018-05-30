using System.Collections.Generic;

public static class DictionaryExtention
{
    /// <summary>
    /// Appends an entire dictionary to another.
    /// </summary>
    /// <param name="origin">Dictionary that gets appended onto extended object.</param>
    public static void Append<K, V>(this Dictionary<K, V> target, Dictionary<K, V> origin)
    {
        if (origin == null || target == null)
        {
            throw new System.ArgumentNullException("Collection is null");
        }

        foreach (var item in origin)
        {
            if (!target.ContainsKey(item.Key))
            {
                target.Add(item.Key, item.Value);
            }
        }
    }

    /// <summary>
    /// Overload of the add function to support KeyValuePair.
    /// </summary>
    /// <param name="keyValue">Item to add.</param>
    public static void Add<K, V>(this Dictionary<K, V> target, KeyValuePair<K, V> keyValue)
    {
        target.Add(keyValue.Key, keyValue.Value);
    }

    /// <summary>
    /// Returns a list made of the Dictionnary keys.
    /// </summary>
    /// <returns>List containing keys.</returns>
    public static List<K> ExtractKeys<K, V>(this Dictionary<K, V> target)
    {
        return new List<K>(target.Keys);
    }

    /// <summary>
    /// Returns a list made of the Dictionnary values.
    /// </summary>
    /// <returns>List containing values.</returns>
    public static List<V> ExtractValues<K, V>(this Dictionary<K, V> target)
    {
        return new List<V>(target.Values);
    }
}