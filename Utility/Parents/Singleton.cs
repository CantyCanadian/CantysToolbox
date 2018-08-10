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
            if (s_ApplicationIsQuitting)
            {
                return null;
            }

            lock (s_ThreadSafety)
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

    private static T s_Instance = null;
    private static Object s_ThreadSafety = new Object();
    private static bool s_ApplicationIsQuitting = false;

    // Adding a check to OnApplicationQuit in order to prevent a weird Unity racing bug. 
    // Slight chance the singleton will be destroyed, then recreated as the game is quitting.
    public void OnApplicationQuit()
    {
        s_ApplicationIsQuitting = true;
    }
}
