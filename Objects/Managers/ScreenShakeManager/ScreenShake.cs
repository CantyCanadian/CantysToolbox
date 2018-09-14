using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Personal note : https://assetstore.unity.com/packages/tools/camera/ez-camera-shake-33148
public class ScreenShake : Singleton<ScreenShake>
{
    public Vector3 PositionInfluence = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 RotationInfluence = new Vector3(1.0f, 1.0f, 1.0f);

    private ShakeInformation m_MainShake;
    private ShakeInformation m_UnderlyingShake;

    private struct ShakeInformation
    {
        public float Strength = 0.0f;
        public float Smoothness = 0.0f;
        public float EaseInTime = 0.0f;
        public float UpTime = 0.0f;
        public float EaseOutTime = 0.0f;
        public float Delta = 0.0f;

        public float TotalTime()
        {
            return EaseInTime + UpTime + EaseOutTime;
        }

        public bool UpdateInformation(ref float strength, ref float smoothness)
        {
            if (m_UnderlyingShake.EaseInTime > 0.0f)
            {
                m_UnderlyingShake.Delta += Time.deltaTime;

                if (m_UnderlyingShake.Delta >= m_UnderlyingShake.EaseInTime)
                {
                    m_UnderlyingShake.EaseInTime = 0.0f;
                    m_UnderlyingShake.Delta = 0.0f;
                    strength = m_UnderlyingShake.Strength;
                    smoothness = m_UnderlyingShake.Smoothness;
                }
                else
                {
                    strength = Mathf.Lerp(0.0f, m_UnderlyingShake.Strength, m_UnderlyingShake.Delta / m_UnderlyingShake.EaseInTime);
                    smoothness = Mathf.Lerp(0.0f, m_UnderlyingShake.Smoothness, m_UnderlyingShake.Delta / m_UnderlyingShake.EaseInTime);
                }
            }
            else if (m_UnderlyingShake.UpTime == -1.0f)
            {
                strength = m_UnderlyingShake.Strength;
                smoothness = m_UnderlyingShake.Smoothness;
            }
            else if (m_UnderlyingShake.UpTime > 0.0f)
            {
                m_UnderlyingShake.Delta += Time.deltaTime;

                if (m_UnderlyingShake.Delta >= m_UnderlyingShake.UpTime)
                {
                    m_UnderlyingShake.UpTime = 0.0f;
                    m_UnderlyingShake.Delta = 0.0f;
                }

                strength = m_UnderlyingShake.Strength;
                smoothness = m_UnderlyingShake.Smoothness;
            }
            else if (m_UnderlyingShake.EaseOutTime > 0.0f)
            {
                m_UnderlyingShake.Delta += Time.deltaTime;

                if (m_UnderlyingShake.Delta >= m_UnderlyingShake.EaseOutTime)
                {
                    strength = 0.0f;
                    smoothness = 0.0f;
                    return true;
                }
                else
                {
                    strength = Mathf.Lerp(m_UnderlyingShake.Strength, 0.0f, m_UnderlyingShake.Delta / m_UnderlyingShake.EaseInTime);
                    smoothness = Mathf.Lerp(m_UnderlyingShake.Smoothness, 0.0f, m_UnderlyingShake.Delta / m_UnderlyingShake.EaseInTime);
                }
            }

            return false;
        }
    }

    public void Shake(float strength, float smoothness, float time)
    {
        Shake(strength, smoothness, 0.0f, time, 0.0f);
    }

    public void Shake(float strength, float smoothness, float easeInTime, float upTime, float easeOutTime)
    {
        ShakeInformation mainShake;

        mainShake.Strength = strength;
        mainShake.Smoothness = smoothness;
        mainShake.EaseInTime = easeInTime;
        mainShake.UpTime = upTime;
        mainShake.EaseOutTime = easeOutTime;

        if (m_MainShake != null && m_MainShake.TotalTime() > mainShake.TotalTime())
        {
            return;
        }

        m_MainShake = mainShake;
    }

    public void StartShake(float strength, float smoothness)
    {
        StartShake(strength, smoothness, 0.0f);
    }

    public void StartShake(float strength, float smoothness, float easeInTime)
    {
        ShakeInformation underlyingShake;

        underlyingShake.Strength = strength;
        underlyingShake.Smoothness = smoothness;
        underlyingShake.EaseInTime = easeInTime;
        underlyingShake.UpTime = -1.0f;
        underlyingShake.EaseOutTime = 0.0f;

        m_UnderlyingShake = underlyingShake;
    }

    public void EndShake()
    {
        EndShake(0.0f);
    }

    public void EndShake(float easeOutTime)
    {
        m_UnderlyingShake.UpTime = 0.0f;
        m_UnderlyingShake.EaseOutTime = easeOutTime;
    }

    private void Start()
    {
        if (transform.localPosition != new Vector3(0.0f, 0.0f, 0.0f))
        {
            Debug.Warning("ScreenShake : Object local position not set to 0. Must be at 0 to use screen shake. Will force it to 0.");
            transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (transform.localRotation != new Vector3(0.0f, 0.0f, 0.0f))
        {
            Debug.Warning("ScreenShake : Object local rotation not set to 0. Must be at 0 to use screen shake. Will force it to 0.");
            transform.localRotation = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    private IEnumerator ShakeLoop()
    {
        while(true)
        {
            float underlyingStrength = 0.0f;
            float underlyingSmoothness = 0.0f;

            if (m_UnderlyingShake != null)
            {
                if (m_UnderlyingShake.UpdateInformation(ref underlyingStrength, ref underlyingSmoothness))
                {
                    m_UnderlyingShake = null;
                }
            }

            float mainStrength = 0.0f;
            float mainSmoothness = 0.0f;

            if (m_MainShake != null)
            {
                if (m_MainShake.UpdateInformation(ref mainStrength, ref mainSmoothness))
                {
                    m_MainShake = null;
                }
            }

            //Screenshake code

            yield return null;
        }
    }
}
