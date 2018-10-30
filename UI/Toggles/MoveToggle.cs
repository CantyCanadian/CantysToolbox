///====================================================================================================
///
///     MoveToggle by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections;
using UnityEngine;

namespace Canty.UI
{
    /// <summary>
    /// Basic class that toggles a UI element's position between two values.
    /// </summary>
    public class MoveToggle : MonoBehaviour
    {
        public Vector2 PositionDifference;
        public float TransitionTime;

        private RectTransform m_Transform;

        private Vector2 m_FalseLocalPosition;
        private Vector2 m_TrueLocalPosition;

        private Coroutine m_CoroutineRef;
        private bool m_State = false;

        public void Initialize()
        {
            m_Transform = m_Transform == null ? GetComponent<RectTransform>() : m_Transform;

            m_FalseLocalPosition = transform.localPosition;
            m_TrueLocalPosition = m_FalseLocalPosition + PositionDifference;
        }

        public void Initialize(Vector2 newOrigin)
        {
            m_Transform = m_Transform == null ? GetComponent<RectTransform>() : m_Transform;

            m_FalseLocalPosition = newOrigin;
            m_TrueLocalPosition = m_FalseLocalPosition + PositionDifference;
        }

        public void Initialize(Vector2 newOrigin, Vector2 newDifference)
        {
            m_Transform = m_Transform == null ? GetComponent<RectTransform>() : m_Transform;
            PositionDifference = newDifference;

            m_FalseLocalPosition = newOrigin;
            m_TrueLocalPosition = m_FalseLocalPosition + PositionDifference;
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
            m_Transform.localPosition = m_State ? m_TrueLocalPosition : m_FalseLocalPosition;
        }

        private IEnumerator PlayLoop()
        {
            float delta = 0.0f;

            while (delta < TransitionTime)
            {
                delta += Time.deltaTime;

                m_Transform.localPosition = m_State
                    ? Vector3.Lerp(m_FalseLocalPosition, m_TrueLocalPosition, delta / TransitionTime)
                    : Vector3.Lerp(m_TrueLocalPosition, m_FalseLocalPosition, delta / TransitionTime);

                yield return null;
            }
        }
    }
}