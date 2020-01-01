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
        /// Gets the instance of the singleton. Note : This code cannot be used in a thread due to the restriction placed by Unity's FindObjectOfType. 
        /// To use in a thread, get the instance and pass it to the thread as an argument. At that point, it becomes the object's job to be thread-safe.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (Thread.CurrentThread != s_MainThreadReference)
                {
                    Debug.LogError("Singleton<" + typeof(T).ToString() + "> : Trying to access instance from a non-main thread. Please read the Instance function summary.");
                    return null;
                }

                if (s_ApplicationIsQuitting)
                {
                    return null;
                }

                if (!s_Instance)
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
        private static bool s_ApplicationIsQuitting = false;
        private static Thread s_MainThreadReference = Thread.CurrentThread;

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