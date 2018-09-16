using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public static string SaveLocation { get { return Application.persistentDataPath + "\\Saves\\"; } }
    public static string FileExtension { get { return ".sav"; } }

    private Dictionary<Type, ISaveable> m_RegisteredSaveables = null;
    private Dictionary<string, string[]> m_SavedData;
    private string m_LoadedSaveLocation = "";

    private bool m_DefaultEncrypted = true;

    public void SetDefaultIsEncrypted(bool encrypt)
    {
        m_DefaultEncrypted = encrypt;
    }

    public void RegisterSaveable(ISaveable saveable, Type originalType)
    {
        if (m_RegisteredSaveables == null)
        {
            m_RegisteredSaveables = new Dictionary<Type, ISaveable>();
        }

        if (m_RegisteredSaveables.ContainsKey(originalType))
        {
            Debug.LogError("SaveManager : Trying to add two objects of the same type, [" + originalType.ToString() + "], as saveables. Recommended to create a manager if multiples of the same item needs saving.");
            return;
        }

        m_RegisteredSaveables.Add(originalType, saveable);
    }

    public bool LoadOrCreateSave(string saveName)
    {
        return LoadOrCreateSave(saveName, m_DefaultEncrypted);
    }

    public bool LoadOrCreateSave(string saveName, bool encrypt)
    {
        if (File.Exists(SaveLocation + saveName + FileExtension))
        {
            return LoadSaveFile(saveName, encrypt);
        }
        else
        {
            return CreateNewSaveFile(saveName, encrypt);
        }
    }

    public bool CreateNewSaveFile(string saveName)
    {
        return CreateNewSaveFile(saveName, m_DefaultEncrypted);
    }

    public bool CreateNewSaveFile(string saveName, bool encrypt)
    {
        string fileLocation = SaveLocation + saveName + FileExtension;

        Directory.CreateDirectory(SaveLocation);

        if (File.Exists(fileLocation))
        {
            return false;
        }

        File.Create(fileLocation);

        DataDefaultToSaveables();
        DataSaveablesToCache();
        DataCacheToFile(encrypt);

        return true;
    }

    public bool CreateOrOverwriteNewSaveFile(string saveName)
    {
        return CreateOrOverwriteNewSaveFile(saveName, m_DefaultEncrypted);
    }

    public bool CreateOrOverwriteNewSaveFile(string saveName, bool encrypt)
    {
        string fileLocation = SaveLocation + saveName + FileExtension;

        Directory.CreateDirectory(SaveLocation);

        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }

        File.Create(fileLocation);

        DataDefaultToSaveables();
        DataSaveablesToCache();
        DataCacheToFile(encrypt);

        return true;
    }

    public bool LoadSaveFile(string saveName)
    {
        return LoadSaveFile(saveName, m_DefaultEncrypted);
    }

    public bool LoadSaveFile(string saveName, bool encrypt)
    {
        string fileLocation = SaveLocation + saveName + FileExtension;

        if (!File.Exists(fileLocation))
        {
            return false;
        }

        m_LoadedSaveLocation = fileLocation;

        DataFileToCache(encrypt);
        DataCacheToSaveables();

        return true;
    }

    public void SaveData(bool encrypt)
    {
        DataSaveablesToCache();
        DataCacheToFile(encrypt);
    }

    private void DataFileToCache(bool isEncrypted)
    {
        StreamReader reader = new StreamReader(m_LoadedSaveLocation);
        List<string> dataSet = new List<string>();

        while(reader.Peek() >= 0)
        {
            dataSet.Add(reader.ReadLine());
        }

        reader.Close();

        string[] readyData = dataSet.ToArray();

        if (isEncrypted)
        {
            DecryptData(ref readyData);
        }

        if (m_SavedData == null)
        {
            m_SavedData = new Dictionary<string, string[]>();
        }

        foreach(string readyLine in readyData)
        {
            string[] readyItems = readyLine.Split('|');

            m_SavedData[readyItems[0]] = readyItems.Subdivide(1, readyItems.Length - 1).ToArray();
        }
    }

    private void DataCacheToFile(bool encrypt)
    {
        List<string> dataSet = new List<string>();

        if (m_SavedData == null)
        {
            Debug.LogError("SaveManager : Trying to write data to file while there is no data in the save cache. Write to cache first.");
            return;
        }

        foreach (KeyValuePair<string, string[]> dataPair in m_SavedData)
        {
            string line = dataPair.Key + "|";

            foreach(string data in dataPair.Value)
            {
                line += data + "|";
            }

            dataSet.Add(line);
        }

        string[] readyData = dataSet.ToArray();

        if (encrypt)
        {
            EncryptData(ref readyData);
        }

        StreamWriter writer = new StreamWriter(m_LoadedSaveLocation, false);

        foreach (string readyLine in readyData)
        {
            writer.WriteLine(readyLine);
        }

        writer.Close();
    }

    private void DataSaveablesToCache()
    {
        if (m_SavedData == null)
        {
            m_SavedData = new Dictionary<string, string[]>();
        }
        else
        {
            m_SavedData.Clear();
        }

        foreach(KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
        {
            m_SavedData[saveablePair.Key.ToString()] = saveablePair.Value.SaveData();
        }
    }

    private void DataCacheToSaveables()
    {
        if (m_SavedData == null)
        {
            Debug.LogError("SaveManager : Trying to move data to saveables while there is no data in the save cache. Write to cache first.");
            return;
        }

        foreach (KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
        {
            if (m_SavedData.ContainsKey(saveablePair.Key.ToString()))
            {
                saveablePair.Value.LoadData(m_SavedData[saveablePair.Key.ToString()]);
            }
            else
            {
                Debug.Log("SaveManager : Data from file type " + saveablePair.Key.ToString() + " not found. Setting default values.");
                saveablePair.Value.LoadDefaultData();
            }
        }
    }

    private void DataDefaultToSaveables()
    {
        foreach (KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
        {
            saveablePair.Value.LoadDefaultData();
        }
    }

    private void EncryptData(ref string[] data)
    {
        for(int i = 0; i < data.Length; i++)
        {
            data[i] = EncryptionUtil.EncryptString(data[i]);
        }
    }

    private void DecryptData(ref string[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = EncryptionUtil.DecryptString(data[i]);
        }
    }
}
