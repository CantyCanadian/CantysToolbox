public interface ISaveable
{
    string[] SaveData();
    void LoadDefaultData();
    void LoadData(string[] data);
}