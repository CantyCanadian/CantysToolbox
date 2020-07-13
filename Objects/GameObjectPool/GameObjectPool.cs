using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Simple pool object meant to pool Unity's GameObjects.
    /// </summary>
    public class GameObjectPool
    {
        private List<GameObject> m_ObjectPool = null;
        private List<bool> m_ObjectPoolState = null;

        /// <summary>
        /// Returns given object to a gettable state.
        /// </summary>
        public void ReleaseGameObject(GameObject released)
        {
            for (int i = 0; i < m_ObjectPool.Count; i++)
            {
                if (m_ObjectPool[i] == released)
                {
                    m_ObjectPoolState[i] = true;
                }
            }
        }

        /// <summary>
        /// Gets an object and keeps track of its pool index.
        /// </summary>
        public GameObject GetGameObject(out int index)
        {
            for (int i = 0; i < m_ObjectPoolState.Count; i++)
            {
                if (m_ObjectPoolState[i])
                {
                    m_ObjectPoolState[i] = false;
                    index = i;
                    return m_ObjectPool[i];
                }
            }

            index = -1;
            return null;
        }

        /// <summary>
        /// Gets the specific object at the given index.
        /// </summary>
        public GameObject GetGameObject(int specificIndex)
        {
            if (specificIndex > 0 && specificIndex < m_ObjectPool.Count && m_ObjectPoolState[specificIndex])
            {
                m_ObjectPoolState[specificIndex] = false;
                return m_ObjectPool[specificIndex];
            }

            return null;
        }

        /// <summary>
        /// Gets an object from the pool.
        /// </summary>
        public GameObject GetGameObject()
        {
            for (int i = 0; i < m_ObjectPoolState.Count; i++)
            {
                if (m_ObjectPoolState[i])
                {
                    m_ObjectPoolState[i] = false;
                    return m_ObjectPool[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the pool object using a given prefab, how many objects are available and the parent where the objects are placed.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="count"></param>
        /// <param name="parent"></param>
        public GameObjectPool(GameObject prefab, int count, Transform parent)
        {
            m_ObjectPool = new List<GameObject>();
            m_ObjectPoolState = new List<bool>();

            for (int i = 0; i < count; i++)
            {
                GameObject go = GameObject.Instantiate(prefab, parent);
                go.SetActive(false);
                m_ObjectPool.Add(go);
                m_ObjectPoolState.Add(true);
            }
        }
    }
}