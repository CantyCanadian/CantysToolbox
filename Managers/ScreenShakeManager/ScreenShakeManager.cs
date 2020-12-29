///====================================================================================================
///
///     ScreenShake by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty.Managers
{
    public class ScreenShakeManager : Singleton<ScreenShakeManager>
    {
        public Vector3 PositionInfluence = new Vector3(1.0f, 1.0f, 1.0f);
        public Vector3 RotationInfluence = new Vector3(1.0f, 1.0f, 1.0f);

        public bool UseLocalPosition = true;
        public bool UseLocalRotation = true;

        private Dictionary<GameObject, ShakeInformation> m_Shakes = null;

        // This is a class instead of a struct because its data has to be changed.
        private class ShakeInformation
        {
            public Vector3 OriginalPosition;
            public Vector3 OriginalRotation;

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
                        strength = Mathf.Lerp(Strength, 0.0f, Delta / EaseOutTime);
                        smoothness = Mathf.Lerp(Smoothness, 0.0f, Delta / EaseOutTime);
                    }
                }

                return false;
            }
        }

		/// <summary>
		/// Shakes the given object until you stop it.
		/// </summary>
        public void StartShake(GameObject target, float strength, float smoothness)
        {
            Shake(target, strength, smoothness, 0.0f, -1.0f, 0.0f);
        }

		/// <summary>
		/// Shakes the given object until you stop it. Starts with an ease in.
		/// </summary>
        public void StartShake(GameObject target, float strength, float smoothness, float easeInTime)
        {
            Shake(target, strength, smoothness, easeInTime, -1.0f, 0.0f);
        }

		/// <summary>
		/// Shakes the given object for the given time.
		/// </summary>
        public void ShakeOnce(GameObject target, float strength, float smoothness, float time)
        {
            Shake(target, strength, smoothness, 0.0f, time, 0.0f);
        }

		/// <summary>
		/// Shakes the given object for the given time. Will both ease in and out. Note, easing is added to total time.
		/// </summary>
        public void ShakeOnce(GameObject target,  float strength, float smoothness, float easeInTime, float upTime, float easeOutTime)
        {
            Shake(target, strength, smoothness, easeInTime, upTime, easeOutTime);
        }

		/// <summary>
		/// Forces the given object to stop shaking.
		/// </summary>
        public void EndShake(GameObject target)
        {
            EndShake(target, 0.0f);
        }

		/// <summary>
		/// Forces the given object to stop shaking with a given ease out.
		/// </summary>
        public void EndShake(GameObject target, float easeOutTime)
        {
            if (m_Shakes == null)
            {
                m_Shakes = new Dictionary<GameObject, ShakeInformation>();
            }

            if (m_Shakes.ContainsKey(target))
            {
                ShakeInformation info = m_Shakes[target];

                info.UpTime = 0.0f;
                info.EaseOutTime = easeOutTime;

                m_Shakes[target] = info;
            }
            else
            {
                Debug.LogWarning("ScreenShake : Trying to end a non-existing shake.");
            }
        }

		/// <summary>
		/// Shake function used by all public shake functions.
		/// </summary>
        private void Shake(GameObject target, float strength, float smoothness, float easeInTime, float upTime, float easeOutTime)
        {
            if (m_Shakes == null)
            {
                m_Shakes = new Dictionary<GameObject, ShakeInformation>();
            }

            ShakeInformation newShake = new ShakeInformation();

            newShake.OriginalPosition = UseLocalPosition ? target.transform.localPosition : target.transform.position;
            newShake.OriginalRotation = UseLocalRotation ? target.transform.localEulerAngles : target.transform.eulerAngles;

            newShake.Strength = strength;
            newShake.Smoothness = smoothness;
            newShake.EaseInTime = easeInTime;
            newShake.UpTime = upTime;
            newShake.EaseOutTime = easeOutTime;
            newShake.Delta = 0.0f;

            if (m_Shakes != null && m_Shakes.ContainsKey(target) && m_Shakes[target].TotalTime() > newShake.TotalTime())
            {
                return;
            }
            else if (m_Shakes.ContainsKey(target))
            {
                m_Shakes.Remove(target);
            }

            m_Shakes.Add(target, newShake);
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(ShakeLoop());
        }

		/// <summary>
		/// Shaking coroutine.
		/// </summary>
        private IEnumerator ShakeLoop()
        {
            List<GameObject> toRemove = new List<GameObject>();

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

                        if (UseLocalPosition)
                        {
                            shake.Key.transform.localPosition = shake.Value.OriginalPosition;
                        }
                        else
                        {
                            shake.Key.transform.position = shake.Value.OriginalPosition;
                        }

                        if (UseLocalRotation)
                        {
                            shake.Key.transform.localEulerAngles = shake.Value.OriginalRotation;
                        }
                        else
                        {
                            shake.Key.transform.eulerAngles = shake.Value.OriginalRotation;
                        }
                    }
                    else
                    {
                        float time = Time.time / Mathf.Max(smoothness, 0.001f);
                        float mod = 100.0f / Mathf.Max(smoothness, 0.001f);

                        // Making sure to offset each perlin noise samples to prevent overlap.
                        Vector3 shakePosition = new Vector3(Mathf.PerlinNoise(time + 1 * mod, time + 1 * mod), Mathf.PerlinNoise(time + 2 * mod, time + 2 * mod), Mathf.PerlinNoise(time + 3 * mod, time + 3 * mod));
                        Vector3 shakeRotation = new Vector3(Mathf.PerlinNoise(time + 4 * mod, time + 4 * mod), Mathf.PerlinNoise(time + 5 * mod, time + 5 * mod), Mathf.PerlinNoise(time + 6 * mod, time + 6 * mod));
                        
                        // Centering the shaking from (0,1) to (-1,1)
                        shakePosition = (shakePosition * 2).MinusScalar(1);
                        shakeRotation = (shakeRotation * 2).MinusScalar(1);

                        shakePosition *= strength;
                        shakeRotation *= strength;

                        shakePosition = shakePosition.Multiply(PositionInfluence);
                        shakeRotation = shakeRotation.Multiply(RotationInfluence);

                        if (UseLocalPosition)
                        {
                            shake.Key.transform.localPosition = shake.Value.OriginalPosition + shakePosition;
                        }
                        else
                        {
                            shake.Key.transform.position = shake.Value.OriginalPosition + shakePosition;
                        }

                        if (UseLocalRotation)
                        {
                            shake.Key.transform.localEulerAngles = shake.Value.OriginalRotation + shakeRotation;
                        }
                        else
                        {
                            shake.Key.transform.eulerAngles = shake.Value.OriginalRotation + shakeRotation;
                        }
                    }
                }

                foreach (GameObject removed in toRemove)
                {
                    m_Shakes.Remove(removed);
                }

                yield return null;
            }
        }
    }
}