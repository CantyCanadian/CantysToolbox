using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public static string SaveLocation { get { return Application.persistantDataPath + "\\Saves\\"; } }

    private Dictionary<Type, ISaveable> m_RegisteredSaveables;
    private Dictionary<string, string[]> m_SavedData;
    private string m_LoadedSaveLocation = "";

    public bool LoadOrCreateSave(string saveName, string extention = ".sav", bool encrypt = true)
    {
        if (File.Exists(SaveLocation + saveName + extention))
        {
            return LoadSaveFile(saveName, extention, encrypt);
        }
        else
        {
            return CreateNewSaveFile(saveName, extention, encrypt);
        }
    }

    public bool CreateNewSaveFile(string saveName, string extention = ".sav", bool encrypt = true)
    {
        string fileLocation = SaveLocation + saveName + extention;

        Directory.CreateDirectory(SaveLocation);

        if (File.Exists(fileLocation))
        {
            return false;
        }

        File.Create(fileLocation);

        DataDefaultToSaveables();
        DataSaveablesToCache();
        DataCacheToFile();

        return true;
    }

    public bool CreateOrOverwriteNewSaveFile(string saveName, string extention = ".sav", bool encrypt = true)
    {
        string fileLocation = SaveLocation + saveName + extention;

        Directory.CreateDirectory(SaveLocation);

        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }

        File.Create(fileLocation);

        DataDefaultToSaveables();
        DataSaveablesToCache();
        DataCacheToFile();

        return true;
    }

    public bool LoadSaveFile(string saveName, string extention = ".sav", bool encrypt = true)
    {
        string fileLocation = SaveLocation + saveName + extention;

        if (!File.Exists(fileLocation))
        {
            return false;
        }

        m_LoadedSaveLocation = fileLocation;

        DataFileToCache();
        DataCacheToSaveables();

        return true;
    }

    public void SaveData()
    {
        DataSaveablesToCache();
        DataCacheToFile();
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
            readyData = DecryptData(ref readyData);
        }

        foreach(string readyLine in readyData)
        {
            string[] readyItems = readyLine.Split('|');

            m_SavedData[readyItems[0]] = readyItems.Subdivide(1, readyItems.Length - 1);
        }
    }

    private void DataCacheToFile(bool encrypt)
    {
        List<string> dataSet = new List<string>();

        foreach(KeyValuePair<string, string[]> dataPair in m_SavedData)
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
            readyData = EncryptData(ref readyData);
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
        m_SavedData.Clear();

        foreach(KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
        {
            m_SavedData[saveablePair.Key.ToString()] = saveablePair.Value.SaveData();
        }
    }

    private void DataCacheToSaveables()
    {
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
