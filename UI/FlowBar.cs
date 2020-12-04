using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowBar : MonoBehaviour
{
    private Func<float> m_ListeningCallback = null;

    private RectTransform m_RectTransform = null;
    private float m_InitialWidth = 0.0f;

    private float m_CurrentValue = 1.0f;

    public void SetListeningCallback(Func<float> callback)
    {
        m_ListeningCallback = callback;

        m_RectTransform.sizeDelta = new Vector2(m_InitialWidth * m_ListeningCallback.Invoke(), m_RectTransform.sizeDelta.y);
    }

    public void SetValue(float value)
    {
        m_CurrentValue = value;
        m_ListeningCallback = null;

        m_RectTransform.sizeDelta = new Vector2(m_InitialWidth * m_CurrentValue, m_RectTransform.sizeDelta.y);
    }

    private void Update()
    {
        if (m_ListeningCallback != null)
        {
            m_RectTransform.sizeDelta = new Vector2(m_InitialWidth * m_ListeningCallback.Invoke(), m_RectTransform.sizeDelta.y);
        }
    }

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();

        if (m_RectTransform == null)
        {
            Debug.LogError("FlowBar created without RectTransform.");
        }

        m_InitialWidth = m_RectTransform.sizeDelta.x;
    }
}
