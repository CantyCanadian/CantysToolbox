using UnityEngine;

public abstract class SaveableBase<I> : MonoBehaviour
{
    protected void Start()
    {
        SaveManager.Instance.RegisterSaveable(this, typeof(I));
    }

    public abstract string[] SaveData();
    public abstract void LoadDefaultData();
    public abstract void LoadData(string[] data);
}
