using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public static string SaveLocation { get { return Application.persistantDataPath; } }

    private Dictionary<Type, ISaveable> m_RegisteredSaveables;
    private Dictionary<Type, string[]> m_SavedData;
    private bool m_SaveInitialized = false;

    public bool LoadOrCreateSave(string saveName, string extention = ".sav")
    {
        if (File.Exists(saveName + extention))
        {
            return LoadSaveFile(saveName, extention);
        }
        else
        {
            return CreateNewSaveFile(saveName, extention);
        }
    }

    public bool CreateNewSaveFile(string saveName, string extention = ".sav")
    {


        m_SaveInitialized = true;
    }

    public bool LoadSaveFile(string saveName, string extention = ".sav")
    {


        m_SaveInitialized = true;
    }

    public void SaveData()
    {
        foreach(KeyValuePair<Type, string[]> savePair in m_SavedObjects)
        {
            string[] data = savePair.Value.GetSaveableData();

        }
    }

    private void LoadDataFromFile(string fullSaveName)
    {

    }

    private void SaveDataToFile(string fullSaveName)
    {

    }

    private void LoadDataFromSaveables()
    {
        foreach(KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
        {
            m_SavedData[saveablePair.Key] = saveablePair.Value.SaveData();
        }
    }

    private void LoadDataToSaveables()
    {
        foreach (KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
        {
            saveablePair.Value.LoadData(m_SavedData[saveablePair.Key]);
        }
    }
}
