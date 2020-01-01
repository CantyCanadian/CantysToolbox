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
    /// Inherits from MonoBehaviour since a child class can't inherit from two parent classes.
    /// </summary>
    public abstract class SaveableBase : MonoBehaviour
    {
        protected void Start()
        {
            SaveManager.Instance.RegisterSaveable(this, GetType());
        }

        public abstract string[] SaveData();
        public abstract void LoadDefaultData();
        public abstract void LoadData(string[] data);
    }
}