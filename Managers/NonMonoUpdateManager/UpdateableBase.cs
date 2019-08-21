///====================================================================================================
///
///     UpdateableBase by
///     - CantyCanadian
///
///====================================================================================================

namespace Canty.Managers
{
    public abstract class UpdateableBase
    {
        /// <summary>
        /// Register itself to the updateable list.
        /// </summary>
        public UpdateableBase()
        {
            NonMonoUpdateManager.Instance.RegisterUpdateable(this);
        }
        
        public abstract void Update();
    }
}