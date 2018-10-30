///====================================================================================================
///
///     IntTextObservable by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Canty.UI
{
    /// <summary>
    /// Int implementation of the SerializableCallback class. Changes the text dynamically using passed-in int-returning function.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class IntTextObservable : MonoBehaviour
    {
        public IntObservable ValueToObserve;

        private Text m_Text = null;

        void Update()
        {
            if (m_Text == null)
            {
                m_Text = GetComponent<Text>();
            }

            m_Text.text = ValueToObserve.Invoke().ToString();
        }
    }

    [System.Serializable]
    public class IntObservable : SerializableCallback<int>
    {
    }
}