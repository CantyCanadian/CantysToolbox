using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtension
{
    /// <summary>
    /// Gets a component from the object or adds it if it doesn't exist.
    /// </summary>
    /// <returns>Component, either pre-existing or added.</returns>
    public static T GetOrAddComponent<T>(this Component value) where T : Component
    {
        T result = value.GetComponent<T>();

        if (result == null)
        {
            result = value.gameObject.AddComponent<T>();
        }

        return result;
    }
}
