using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T : Component
{
    private static T s_Instance = null;
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                T find = FindObjectOfType<T>();

                if (find != null)
                {
                    s_Instance = find;
                }
                else
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    DontDestroyOnLoad(obj);

                    s_Instance = obj.AddComponent<T>();
                }
            }

            return s_Instance;
        }
    }
}
