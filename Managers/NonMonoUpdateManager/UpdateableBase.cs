///====================================================================================================
///
///     UpdateableBase by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.Managers
{
    public abstract class UpdateableBase
    {
        protected bool m_Initialized = false;

        /// <summary>
        /// Register itself to the updateable list.
        /// </summary>
        protected void Initialize()
        {
            NonMonoUpdateManager.Instance.RegisterUpdateable(this);
        }

        public abstract void Update();
    }
}