///====================================================================================================
///
///     ScreenShake by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections;
using UnityEngine;

namespace Canty.Managers
{
    public class ScreenShake : Singleton<ScreenShake>
    {
        public Vector3 PositionInfluence = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 RotationInfluence = new Vector3(1.0f, 1.0f, 1.0f);

        private ShakeInformation? m_MainShake = null;
        private ShakeInformation? m_UnderlyingShake = null;

        private struct ShakeInformation
        {
            public float Strength;
            public float Smoothness;
            public float EaseInTime;
            public float UpTime;
            public float EaseOutTime;
            public float Delta;

            public float TotalTime()
            {
                return EaseInTime + UpTime + EaseOutTime;
            }

            public bool UpdateInformation(ref float strength, ref float smoothness)
            {
                if (EaseInTime > 0.0f)
                {
                    Delta += Time.deltaTime;

                    if (Delta >= EaseInTime)
                    {
                        EaseInTime = 0.0f;
                        Delta = 0.0f;
                        strength = Strength;
                        smoothness = Smoothness;
                    }
                    else
                    {
                        strength = Mathf.Lerp(0.0f, Strength, Delta / EaseInTime);
                        smoothness = Mathf.Lerp(0.0f, Smoothness, Delta / EaseInTime);
                    }
                }
                else if (UpTime == -1.0f)
                {
                    strength = Strength;
                    smoothness = Smoothness;
                }
                else if (UpTime > 0.0f)
                {
                    Delta += Time.deltaTime;

                    if (Delta >= UpTime)
                    {
                        UpTime = 0.0f;
                        Delta = 0.0f;
                    }

                    strength = Strength;
                    smoothness = Smoothness;
                }
                else if (EaseOutTime > 0.0f)
                {
                    Delta += Time.deltaTime;

                    if (Delta >= EaseOutTime)
                    {
                        strength = 0.0f;
                        smoothness = 0.0f;
                        return true;
                    }
                    else
                    {
                        strength = Mathf.Lerp(Strength, 0.0f, Delta / EaseInTime);
                        smoothness = Mathf.Lerp(Smoothness, 0.0f, Delta / EaseInTime);
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
            ShakeInformation mainShake = new ShakeInformation();

            mainShake.Strength = strength;
            mainShake.Smoothness = smoothness;
            mainShake.EaseInTime = easeInTime;
            mainShake.UpTime = upTime;
            mainShake.EaseOutTime = easeOutTime;

            if (m_MainShake != null && m_MainShake.Value.TotalTime() > mainShake.TotalTime())
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
            ShakeInformation underlyingShake = new ShakeInformation();

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
            ShakeInformation currentShake = m_UnderlyingShake.Value;

            currentShake.UpTime = 0.0f;
            currentShake.EaseOutTime = easeOutTime;

            m_UnderlyingShake = currentShake;
        }

        private void Start()
        {
            if (transform.localPosition != new Vector3(0.0f, 0.0f, 0.0f))
            {
                Debug.LogWarning(
                    "ScreenShake : Object local position not set to 0. Must be at 0 to use screen shake. Will force it to 0.");
                transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }

            if (transform.localEulerAngles != new Vector3(0.0f, 0.0f, 0.0f))
            {
                Debug.LogWarning(
                    "ScreenShake : Object local rotation not set to 0. Must be at 0 to use screen shake. Will force it to 0.");
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        private IEnumerator ShakeLoop()
        {
            while (true)
            {
                float underlyingStrength = 0.0f;
                float underlyingSmoothness = 0.0f;

                if (m_UnderlyingShake != null)
                {
                    if (m_UnderlyingShake.Value.UpdateInformation(ref underlyingStrength, ref underlyingSmoothness))
                    {
                        m_UnderlyingShake = null;
                    }
                }

                float mainStrength = 0.0f;
                float mainSmoothness = 0.0f;

                if (m_MainShake != null)
                {
                    if (m_MainShake.Value.UpdateInformation(ref mainStrength, ref mainSmoothness))
                    {
                        m_MainShake = null;
                    }
                }

                //Screenshake code

                yield return null;
            }
        }
    }
}