///====================================================================================================
///
///     CurveTimer by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;
using Canty.Managers;

namespace Canty
{
    [System.Serializable]
    public class CurveTimer : UpdateableBase
    {
        public CurveSelector TimerCurve;
        public float CurveTime = 1.0f;

        public float Value
        {
            get
            {
                m_LastRead = false;
                return m_Value;
            }
        }

        public bool isPlaying
        {
            get { return m_Play || m_LastRead; }
        }

        private float m_Value = 0.0f;
        private float m_Delta = 0.0f;

        private bool m_Initialized = false;

        private bool m_Play = false;
        private bool m_LastRead = false;
        private bool m_Backwards = false;
        private bool m_Loop = false;
        private bool m_Flip = false;

        public void Play(bool loop = false, bool backwards = false)
        {
            if (m_Initialized == false)
            {
                Initialize();
                m_Initialized = true;
            }

            m_Play = true;
            m_LastRead = false;
            m_Backwards = backwards;
            m_Delta = 0.0f;
            m_Loop = loop;
        }

        public void Stop(bool stopAtTarget = true)
        {
            m_Play = false;

            if (stopAtTarget)
            {
                m_Value = m_Backwards ? 0.0f : 1.0f;
            }
        }

        public override void Update()
        {
            if (m_Flip)
            {
                m_Delta = 0.0f;
                m_Backwards = !m_Backwards;
                m_Flip = false;
            }

            if (m_Play && !m_LastRead)
            {
                float initial = m_Backwards ? 1.0f : 0.0f;
                float target = m_Backwards ? 0.0f : 1.0f;

                m_Delta += Time.deltaTime;

                if (m_Delta > CurveTime)
                {
                    if (m_Loop)
                    {
                        m_Delta = CurveTime;
                        m_Flip = true;
                    }
                    else
                    {
                        m_Delta = CurveTime;
                        m_LastRead = true;
                        m_Play = false;
                    }
                }

                m_Value = TimerCurve.Invoke(initial, target, m_Delta / CurveTime);
            }
        }
    }
}