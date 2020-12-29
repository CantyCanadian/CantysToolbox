///====================================================================================================
///
///     Singleton by
///     - CantyCanadian
///
///====================================================================================================

using System.Threading;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Parent class turning child into a singleton implementation. Note, to get stuff properly called on the Singleton's creation, use Awake and not Start.
    /// </summary>
    /// <typeparam name="T">Object type of the child.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// Gets the instance of the singleton. If it doesn't exist, create one.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (s_ApplicationIsQuitting)
                {
                    return null;
                }

                if (!s_Instance)
                {
                    if (Thread.CurrentThread == s_MainThreadReference)
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
                    else
                    {
                        Debug.LogError("Singleton<" + typeof(T).ToString() + "> : Trying to generate Singleton in non-main thread. Please create object in the world or call Instance in the main thread beforehand.");
                        return null;
                    }
                }

                return s_Instance;
            }
        }

        private static T s_Instance = null;
        private static bool s_ApplicationIsQuitting = false;
        private static Thread s_MainThreadReference = Thread.CurrentThread;

        protected virtual void Start()
        {
            if (!s_Instance)
            {
                if (transform.parent != null)
                {
                    transform.SetParent(null);
                }

                DontDestroyOnLoad(gameObject);
                s_Instance = (T)this;
            }
            else if (gameObject != s_Instance.gameObject)
            {
                Destroy(gameObject);
            }
        }

        // Adding a check to OnApplicationQuit and OnDestroy in order to prevent a weird Unity racing bug. 
        // Slight chance the singleton will be destroyed, then recreated as the game is quitting.
        private void OnApplicationQuit()
        {
            s_ApplicationIsQuitting = true;
        }

        private void OnDestroy()
        {
            s_ApplicationIsQuitting = true;
        }
    }
}