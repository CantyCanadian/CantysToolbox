///====================================================================================================
///
///     SizeToggle by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections;
using UnityEngine;

namespace Canty.UI
{
    /// <summary>
    /// Basic class that toggles a UI element's size between two values.
    /// </summary>
    public class SizeToggle : MonoBehaviour
    {
        public Vector2 SizeDifference;
        public float TransitionTime;

        private RectTransform m_Transform;

        private Vector2 m_FalseSize;
        private Vector2 m_TrueSize;

        private Coroutine m_CoroutineRef;
        private bool m_State = false;

        public void Initialize()
        {
            m_Transform = GetComponent<RectTransform>();

            m_FalseSize = m_Transform.sizeDelta;
            m_TrueSize = m_FalseSize + SizeDifference;
        }

        public void Initialize(Vector2 newOrigin)
        {
            m_Transform = GetComponent<RectTransform>();

            m_FalseSize = newOrigin;
            m_TrueSize = m_FalseSize + SizeDifference;
        }

        public void Initialize(Vector2 newOrigin, Vector2 newDifference)
        {
            m_Transform = GetComponent<RectTransform>();
            SizeDifference = newDifference;

            m_FalseSize = newOrigin;
            m_TrueSize = m_FalseSize + SizeDifference;
        }

        public void Play(bool state)
        {
            if (m_Transform == null)
            {
                Initialize();
            }

            if (m_CoroutineRef != null)
            {
                Stop();
            }

            m_State = state;
            m_CoroutineRef = StartCoroutine(PlayLoop());
        }

        public IEnumerator PlayCoroutine(bool state)
        {
            if (m_Transform == null)
            {
                Initialize();
            }

            if (m_CoroutineRef != null)
            {
                Stop();
            }

            m_State = state;

            yield return StartCoroutine(PlayLoop());
        }

        public void Toggle()
        {
            if (m_Transform == null)
            {
                Initialize();
            }

            if (m_CoroutineRef != null)
            {
                Stop();
            }

            m_State = !m_State;
            m_CoroutineRef = StartCoroutine(PlayLoop());
        }

        public void Stop()
        {
            if (m_Transform == null)
            {
                Initialize();
            }

            if (m_CoroutineRef == null)
            {
                return;
            }

            StopCoroutine(m_CoroutineRef);
            m_CoroutineRef = null;
            m_Transform.sizeDelta = m_State ? m_TrueSize : m_FalseSize;
        }

        private IEnumerator PlayLoop()
        {
            float delta = 0.0f;

            while (delta < TransitionTime)
            {
                delta += Time.deltaTime;

                m_Transform.sizeDelta = m_State
                    ? Vector3.Lerp(m_FalseSize, m_TrueSize, delta / TransitionTime)
                    : Vector3.Lerp(m_TrueSize, m_FalseSize, delta / TransitionTime);

                yield return null;
            }
        }
    }
}