using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : Singleton<ScreenShakeManager>
{
    private List<ScreenShake> m_Receivers = null;
    private List<KeyValuePair<float, float>> m_Shakes = null;

    public void RegisterScreenShakeReceiver(ScreenShake receiver)
    {
        if (m_Receivers == null)
        {
            m_Receivers = new List<ScreenShake>();
        }

        if (!m_Receivers.Contains(receiver))
        {
            m_Receivers.Add(receiver);
        }
    }

    public void UnregisterScreenShakeReceiver(ScreenShake receiver)
    {
        if (m_Receivers == null)
        {
            m_Receivers = new List<ScreenShake>();
        }

        if (m_Receivers.Contains(receiver))
        {
            m_Receivers.Remove(receiver);
        }
    }

    public void Shake(float time, float strength)
    {
        if (m_Shakes == null)
        {
            m_Shakes = new List<KeyValuePair<float, float>>();
        }

        m_Shakes.Add(new KeyValuePair<float, float>(time, strength));
    }

    private IEnumerator ShakeLoop()
    {
        while(true)
        {
            if (m_Shakes.Count > 0)
            {
                float strength = 0.0f;

                for(int i = 0; i < m_Shakes.Count; i++)
                {
                    if (i == 0)
                    {
                        strength = m_Shakes[i].Value;
                    }

                    m_Shakes[i].Key -= Time.deltaTime;

                    if (m_Shakes[i].Key <= 0.0f)
                    {
                        m_Shakes.RemoveAt(i);
                        i--;
                    }
                }

                //Screenshake code
            }

            yield return true;
        }
    }

    private void Start()
    {
        StartCoroutine(ShakeLoop());
    }
}
