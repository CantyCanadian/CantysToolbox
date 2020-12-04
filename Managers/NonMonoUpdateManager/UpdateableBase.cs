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
        public void Initialize()
        {
            NonMonoUpdateManager.Instance.RegisterUpdateable(this);
        }

        public abstract void Update();
    }
}