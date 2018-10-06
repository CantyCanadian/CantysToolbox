///====================================================================================================
///
///     TimedDestroy by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Basic class to set a destroy timer onto an object.
    /// </summary>
    public class TimedDestroy : MonoBehaviour
    {
        public float SecondsToWait = 5.0f;

        private void Update()
        {
            float timer = SecondsToWait;

            while (timer > 0.0f)
            {
                timer -= Time.deltaTime;
            }

            Destroy(gameObject);
        }
    }
}