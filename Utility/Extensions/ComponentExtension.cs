///====================================================================================================
///
///     ComponentExtension by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty
{
    public static class ComponentExtension
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
}