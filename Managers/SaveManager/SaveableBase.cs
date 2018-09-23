using UnityEngine;

public abstract class SaveableBase : MonoBehaviour
{
    public abstract string[] SaveData();
    public abstract void LoadDefaultData();
    public abstract void LoadData(string[] data);
}

public abstract class SaveableBase<I> : SaveableBase
{
    protected void Start()
    {
        SaveManager.Instance.RegisterSaveable(this, typeof(I));
    }
}
