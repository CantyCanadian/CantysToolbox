using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurveTimer : UpdateableBase
{
    public CurveSelector TimerCurve;
    public float CurveTime = 1.0f;

    public float Value { get { m_LastRead = false; return m_Value; } }
    public bool isPlaying { get { return m_Play || m_LastRead; } }

    private float m_Value = 0.0f;
    private float m_Delta = 0.0f;

    private bool m_Initialized = false;

    private bool m_Play = false;
    private bool m_LastRead = false;
    private bool m_Backwards = false;

    public void Play(bool backwards = false)
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
    }

    public override void Update()
    {
        if (m_Play && !m_LastRead)
        {
            float initial = m_Backwards ? 1.0f : 0.0f;
            float target = m_Backwards ? 0.0f : 1.0f;

            m_Delta += Time.deltaTime;

            if (m_Delta > CurveTime)
            {
                m_Delta = CurveTime;
                m_LastRead = true;
                m_Play = false;
            }

            m_Value = TimerCurve.Invoke(initial, target, m_Delta / CurveTime);
        }
    }
}
