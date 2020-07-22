///====================================================================================================
///
///     SaveableBase by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.Managers
{
    /// <summary>
    /// Base class the object must inherit from to be used by the SaveManager.
    /// </summary>
    public abstract class SaveableBase : MonoBehaviour, ISaveable
    {
        protected void Start()
        {
            SaveManager.Instance.RegisterSaveable(this, GetType());
        }

        /// <summary>
        /// Saving data from objects to file.
        /// </summary>
        public abstract string[] SaveData();

        /// <summary>
        /// Prepares the default data for each objects.
        /// </summary>
        public abstract void LoadDefaultData();

        /// <summary>
        /// Loading data from a file to the object. If data can differ between versions, make sure to implement a way to migrate data using the version string.
        /// </summary>
        public abstract void LoadData(string[] data, string version);
    }

    public interface ISaveable
    {
        string[] SaveData();
        void LoadDefaultData();
        void LoadData(string[] data, string version);
    }
}