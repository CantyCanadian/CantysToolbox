using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Pool> m_Pools;

    private struct Pool
    {
        public Pool(GameObject original, GameObject parent, int poolMaximum)
        {
            Objects = new List<GameObject>();
            AvailableObjects = new List<int>();
            UsedObjects = new List<int>();
            Original = original;
            Parent = parent;
            PoolMaximum = poolMaximum;
        }

        public List<GameObject> Objects;
        public List<int> AvailableObjects;
        public List<int> UsedObjects;
        public GameObject Original;
        public GameObject Parent;
        public int PoolMaximum;
    }

    public void CreatePool(string key, GameObject original, int count)
    {
        CreatePool(key, original, count, count);
    }

    public void CreatePool(string key, GameObject original, int count, int maximum)
    {
        if (m_Pools == null)
        {
            m_Pools = new Dictionary<string, Pool>();
        }

        if (m_Pools.ContainsKey(key))
        {
            Debug.LogWarning("PoolManager : Trying to create a pool using pre-existing key. Key : " + key);
            return;
        }

        GameObject poolParent = new GameObject();
        poolParent.name = key;

        Pool newPool = new Pool(original, poolParent, maximum);

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(original);
            obj.name = key + i.ToString();
            obj.transform.SetParent(poolParent.transform);
            obj.SetActive(false);

            IPoolComponent[] poolComponents = obj.GetComponents<IPoolComponent>();
            for(int j = 0; j < poolComponents.Length; j++)
            {
                poolComponents[j].Initialize();
            }

            newPool.Objects.Add(obj);
            newPool.AvailableObjects.Add(i);
        }

        m_Pools.Add(key, newPool);
    }

    public GameObject[] GetObjects(string key, int count)
    {
        GameObject[] result = new GameObject[count];

        for(int i = 0; i < count; i++)
        {
            result[i] = GetObject(key);
        }

        return result;
    }

    public GameObject GetObject(string key)
    {
        if (!m_Pools.ContainsKey(key))
        {
            Debug.LogError("PoolManager : Trying to get an item from a non-existing pool. Key : " + key);
            return null;
        }

        Pool pool = m_Pools[key];

        if (pool.AvailableObjects.Count == 0)
        {
            if (pool.PoolMaximum <= pool.Objects.Count)
            {
                int index = pool.UsedObjects[0];
                pool.UsedObjects.RemoveAt(0);
                pool.AvailableObjects.Add(index);
                return GetObject(key);
            }

            GameObject obj = Instantiate(pool.Original);
            obj.name = key + pool.Objects.Count;
            obj.transform.SetParent(pool.Parent.transform);
            obj.SetActive(false);

            pool.AvailableObjects.Add(pool.Objects.Count);
            pool.Objects.Add(obj);

            return GetObject(key);
        }
        else
        {
            int index = pool.AvailableObjects[0];
            pool.AvailableObjects.RemoveAt(0);
            pool.UsedObjects.Add(index);

            GameObject result = pool.Objects[index];

            IPoolComponent[] poolComponents = result.GetComponents<IPoolComponent>();
            for (int i = 0; i < poolComponents.Length; i++)
            {
                poolComponents[i].OnTrigger();
            }

            return result;
        }
    }

    public void DiscardObject(GameObject obj)
    {
        GameObject go = default(GameObject);

        foreach(KeyValuePair<string, Pool> pool in m_Pools)
        {
            int index = pool.Value.Objects.FindIndex(x => x.name == obj.name);

            if (go != default(GameObject))
            {
                if (pool.Value.UsedObjects.Contains(index))
                {
                    IPoolComponent[] poolComponents = pool.Value.Objects[index].GetComponents<IPoolComponent>();
                    for (int i = 0; i < poolComponents.Length; i++)
                    {
                        poolComponents[i].OnDiscard();
                    }

                    pool.Value.UsedObjects.Remove(index);
                    pool.Value.AvailableObjects.Add(index);

                    pool.Value.Objects[index].SetActive(false);
                }
                else
                {
                    Debug.LogWarning("PoolManager : Trying to discard an object that is not in use. Object : " + obj.name);
                }

                break;
            }
        }
    }

    public void DoOnAllItems(string key, System.Action<GameObject> action)
    {
        Pool pool = m_Pools[key];

        for (int i = 0; i < pool.Objects.Count; i++)
        {
            action(pool.Objects[i]);
        }
    }

    public Vector3 GetPoolCounts(string key)
    {
        Pool pool = m_Pools[key];
        return new Vector3(pool.AvailableObjects.Count, pool.UsedObjects.Count, pool.Objects.Count);
    }
}
