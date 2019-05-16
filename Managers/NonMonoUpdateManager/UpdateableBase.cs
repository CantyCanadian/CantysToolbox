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
		/// Register itself to the updatable list.
		/// </summary>
        public void Initialize()
        {
            NonMonoUpdateManager.Instance.RegisterUpdateable(this);
        }

        public abstract void Update();
    }
}