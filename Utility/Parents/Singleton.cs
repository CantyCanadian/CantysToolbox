using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class turning child into a singleton implementation.
/// </summary>
/// <typeparam name="T">Object type of the child.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
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

    private static T s_Instance = null;
}
