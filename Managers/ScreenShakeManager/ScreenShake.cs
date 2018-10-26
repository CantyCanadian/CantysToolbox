///====================================================================================================
///
///     ScreenShake by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Canty.Managers
{
    public class ScreenShake : Singleton<ScreenShake>
    {
        public Vector3 PositionInfluence = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 RotationInfluence = new Vector3(1.0f, 1.0f, 1.0f);

        private Dictionary<GameObject, ShakeInformation> m_Shakes = null;

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

        public void StartShake(GameObject target, float strength, float smoothness)
        {
            Shake(target, strength, smoothness, 0.0f, -1.0f, 0.0f);
        }

        public void StartShake(GameObject target, float strength, float smoothness, float easeInTime)
        {
            Shake(target, strength, smoothness, easeInTime, -1.0f, 0.0f);
        }

        public void ShakeOnce(GameObject target, float strength, float smoothness, float time)
        {
            Shake(target, strength, smoothness, 0.0f, time, 0.0f);
        }

        public void ShakeOnce(GameObject target,  float strength, float smoothness, float easeInTime, float upTime, float easeOutTime)
        {
            Shake(target, strength, smoothness, easeInTime, upTime, easeOutTime);
        }

        public void EndShake(GameObject target)
        {
            EndShake(target, 0.0f);
        }

        public void EndShake(GameObject target, float easeOutTime)
        {
            if (m_Shakes.ContainsKey(target))
            {
                m_Shakes[target].UpTime = 0.0f;
                m_Shakes[target].EaseOutTime = easeOutTime;
            }
            else
            {
                Debug.LogWarning("ScreenShake : Trying to end a non-existing shake.");
            }
        }

        private void Shake(GameObject target, float strength, float smoothness, float easeInTime, float upTime, float easeOutTime)
        {
            ShakeInformation newShake = new ShakeInformation();

            newShake.Strength = strength;
            newShake.Smoothness = smoothness;
            newShake.EaseInTime = easeInTime;
            newShake.UpTime = upTime;
            newShake.EaseOutTime = easeOutTime;

            if (m_Shakes != null && m_Shakes.Contains(target) && m_Shakes[target].Value.TotalTime() > newShake.TotalTime())
            {
                return;
            }
            else if (m_Shakes.Contains(target))
            {
                m_Shakes.Remove(target);
            }

            m_Shakes.Add(newShake);
        }

        private void Start()
        {
            if (transform.localPosition != new Vector3(0.0f, 0.0f, 0.0f))
            {
                Debug.LogWarning("ScreenShake : Object local position not set to 0. Must be at 0 to use screen shake. Will force it to 0.");
                transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }

            if (transform.localEulerAngles != new Vector3(0.0f, 0.0f, 0.0f))
            {
                Debug.LogWarning("ScreenShake : Object local rotation not set to 0. Must be at 0 to use screen shake. Will force it to 0.");
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }

            m_Shakes = new Dictionary<GameObject, ShakeInformation>();
        }

        private IEnumerator ShakeLoop()
        {
            List<string> toRemove = new List<string>();

            while (true)
            {
                toRemove.Clear();

                foreach (KeyValuePair<GameObject, ShakeInformation> shake in m_Shakes)
                {
                    float strength = 0.0f;
                    float smoothness = 0.0f;

                    if (shake.Value.UpdateInformation(ref strength, ref smoothness))
                    {
                        toRemove.Add(shake.Key);
                    }

                    //Screenshake code
                }
            }
        }
    }
}