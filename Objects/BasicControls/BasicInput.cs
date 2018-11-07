///====================================================================================================
///
///     BasicInput by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Canty
{
    /// <summary>
    /// Basic class to tie function callback to key events.
    /// </summary>
    public class BasicInput : MonoBehaviour
    {
        public KeyCodeUnityEventDictionary KeyEvents;

        private void Update()
        {
            foreach (KeyValuePair<KeyCode, UnityEvent> kvp in KeyEvents)
            {
                if (Input.GetKey(kvp.Key))
                {
                    kvp.Value.Invoke();
                }
            }
        }

        [System.Serializable]
        public class KeyCodeUnityEventDictionary : SerializableDictionary<KeyCode, UnityEvent>
        {
        }
    }
}
