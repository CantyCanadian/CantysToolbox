///====================================================================================================
///
///     Bounce by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.UI
{
    /// <summary>
    /// Adds a simple bounce effect to a UI element.
    /// </summary>
    public class Bounce : MonoBehaviour
    {
        public Vector2 BounceDifference;
        public CurveTimer BounceCurve;

        private RectTransform m_RectTransform;
        private Vector2 m_OriginalPosition;

        public void Initialize()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        public void Play()
        {
            BounceCurve.Play(true);
            m_OriginalPosition = m_RectTransform.anchoredPosition;
        }

        public void Stop()
        {
            BounceCurve.Stop();
            m_RectTransform.anchoredPosition = m_OriginalPosition;
        }

        private void Update()
        {
            if (BounceCurve.isPlaying)
            {
                m_RectTransform.anchoredPosition = m_OriginalPosition + (BounceDifference * BounceCurve.Value);
            }
        }
    }
}