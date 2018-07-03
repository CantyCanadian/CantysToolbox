using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Part of SerializableDictionary by azixMcAze on Github : https://github.com/azixMcAze/Unity-SerializableDictionary

public static class SerializableDictionary
{
    /// <summary>
    /// Serializable storage object in order to have serialized collections within a serializable dictionary. To use, create a custom implementation locally with set type, serialize it and voila. Ex. [System.Serializable] public class SerializableStringList : SerializableDictionary.Storage&lt;string&gt; { }
    /// </summary>
    /// <typeparam name="T">Collection type.</typeparam>
    public class Storage<T> : SerializableDictionaryBase.Storage
    {
        public T data;
    }
}

/// <summary>
/// To use, create a custom implementation locally with set types, serialize it and voila. Ex. [System.Serializable] public class KeyStringDictionary : SerializableDictionary&lt;string, string&gt; { }
/// </summary>
public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue, TValue>
{
    public SerializableDictionary() { }
    public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict) { }
    protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override TValue GetValue(TValue[] storage, int i)
    {
        return storage[i];
    }

    protected override void SetValue(TValue[] storage, int i, TValue value)
    {
        storage[i] = value;
    }
}

/// <summary>
/// To use, create a custom implementation locally with set types, serialize it and voila. This version is made to support a serialized collection using SerializableDictionary.Storage. Ex. [System.Serializable] public class KeyStringListDictionary : SerializableDictionary&lt;string, List&lt;string&gt;, StringListStorage&gt; { }
/// </summary>
public class SerializableDictionary<TKey, TValue, TValueStorage> : SerializableDictionaryBase<TKey, TValue, TValueStorage> where TValueStorage : SerializableDictionary.Storage<TValue>, new()
{
    public SerializableDictionary() { }
    public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict) { }
    protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override TValue GetValue(TValueStorage[] storage, int i)
    {
        return storage[i].data;
    }

    protected override void SetValue(TValueStorage[] storage, int i, TValue value)
    {
        storage[i] = new TValueStorage();
        storage[i].data = value;
    }
}

public abstract class SerializableDictionaryBase
{
    public abstract class Storage { }

    protected class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>
    {
        public Dictionary() { }
        public Dictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public Dictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

[Serializable]
public abstract class SerializableDictionaryBase<TKey, TValue, TValueStorage> : SerializableDictionaryBase, IDictionary<TKey, TValue>, IDictionary, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
    Dictionary<TKey, TValue> m_Dictionary;
    [SerializeField]
    TKey[] m_Keys;
    [SerializeField]
    TValueStorage[] m_Values;

    public SerializableDictionaryBase()
    {
        m_Dictionary = new Dictionary<TKey, TValue>();
    }

    public SerializableDictionaryBase(IDictionary<TKey, TValue> dictionary)
    {
        m_Dictionary = new Dictionary<TKey, TValue>(dictionary);
    }

    protected abstract void SetValue(TValueStorage[] storage, int i, TValue value);
    protected abstract TValue GetValue(TValueStorage[] storage, int i);

    public void CopyFrom(IDictionary<TKey, TValue> dictionary)
    {
        m_Dictionary.Clear();
        foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
        {
            m_Dictionary[kvp.Key] = kvp.Value;
        }
    }

    public void OnAfterDeserialize()
    {
        if (m_Keys != null && m_Values != null && m_Keys.Length == m_Values.Length)
        {
            m_Dictionary.Clear();
            int n = m_Keys.Length;
            for (int i = 0; i < n; ++i)
            {
                m_Dictionary[m_Keys[i]] = GetValue(m_Values, i);
            }

            m_Keys = null;
            m_Values = null;
        }
    }

    public void OnBeforeSerialize()
    {
        int n = m_Dictionary.Count;
        m_Keys = new TKey[n];
        m_Values = new TValueStorage[n];

        int i = 0;
        foreach (KeyValuePair<TKey, TValue> kvp in m_Dictionary)
        {
            m_Keys[i] = kvp.Key;
            SetValue(m_Values, i, kvp.Value);
            ++i;
        }
    }

    #region IDictionary<TKey, TValue>

    public ICollection<TKey> Keys { get { return ((IDictionary<TKey, TValue>)m_Dictionary).Keys; } }
    public ICollection<TValue> Values { get { return ((IDictionary<TKey, TValue>)m_Dictionary).Values; } }
    public int Count { get { return ((IDictionary<TKey, TValue>)m_Dictionary).Count; } }
    public bool IsReadOnly { get { return ((IDictionary<TKey, TValue>)m_Dictionary).IsReadOnly; } }

    public TValue this[TKey key]
    {
        get
        {
            return ((IDictionary<TKey, TValue>)m_Dictionary)[key];
        }

        set
        {
            ((IDictionary<TKey, TValue>)m_Dictionary)[key] = value;
        }
    }

    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)m_Dictionary).Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return ((IDictionary<TKey, TValue>)m_Dictionary).ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return ((IDictionary<TKey, TValue>)m_Dictionary).Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return ((IDictionary<TKey, TValue>)m_Dictionary).TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((IDictionary<TKey, TValue>)m_Dictionary).Add(item);
    }

    public void Clear()
    {
        ((IDictionary<TKey, TValue>)m_Dictionary).Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)m_Dictionary).Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)m_Dictionary).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)m_Dictionary).Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return ((IDictionary<TKey, TValue>)m_Dictionary).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IDictionary<TKey, TValue>)m_Dictionary).GetEnumerator();
    }

    #endregion

    #region IDictionary

    public bool IsFixedSize
    {
        get
        {
            return ((IDictionary)m_Dictionary).IsFixedSize;
        }
    }

    ICollection IDictionary.Keys
    {
        get
        {
            return ((IDictionary)m_Dictionary).Keys;
        }
    }

    ICollection IDictionary.Values
    {
        get
        {
            return ((IDictionary)m_Dictionary).Values;
        }
    }

    public bool IsSynchronized
    {
        get
        {
            return ((IDictionary)m_Dictionary).IsSynchronized;
        }
    }

    public object SyncRoot
    {
        get
        {
            return ((IDictionary)m_Dictionary).SyncRoot;
        }
    }

    public object this[object key]
    {
        get
        {
            return ((IDictionary)m_Dictionary)[key];
        }

        set
        {
            ((IDictionary)m_Dictionary)[key] = value;
        }
    }

    public void Add(object key, object value)
    {
        ((IDictionary)m_Dictionary).Add(key, value);
    }

    public bool Contains(object key)
    {
        return ((IDictionary)m_Dictionary).Contains(key);
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return ((IDictionary)m_Dictionary).GetEnumerator();
    }

    public void Remove(object key)
    {
        ((IDictionary)m_Dictionary).Remove(key);
    }

    public void CopyTo(Array array, int index)
    {
        ((IDictionary)m_Dictionary).CopyTo(array, index);
    }

    #endregion

    #region IDeserializationCallback

    public void OnDeserialization(object sender)
    {
        ((IDeserializationCallback)m_Dictionary).OnDeserialization(sender);
    }

    #endregion

    #region ISerializable

    protected SerializableDictionaryBase(SerializationInfo info, StreamingContext context)
    {
        m_Dictionary = new Dictionary<TKey, TValue>(info, context);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ((ISerializable)m_Dictionary).GetObjectData(info, context);
    }

    #endregion
}