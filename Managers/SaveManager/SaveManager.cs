///====================================================================================================
///
///     SaveManager by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Canty.Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
		/// <summary>
		/// Location where the save file will be created.
		/// </summary>
        public string SaveLocation { get { return Application.persistentDataPath + "\\Saves\\"; } }

        /// <summary>
        /// Extension used for the save file.
        /// </summary>
        public string FileExtension { get { return ".sav"; } }


        private Dictionary<Type, ISaveable> m_RegisteredSaveables = null;
        private Dictionary<string, string[]> m_SavedData;
        private string m_LoadedSaveLocation = "";

        private bool m_DefaultEncrypted = true;

		/// <summary>
		/// Set if save data should be encrypted by default. True by default.
		/// </summary>
        public void SetDefaultIsEncrypted(bool encrypt)
        {
            m_DefaultEncrypted = encrypt;
        }

		/// <summary>
		/// Register a saveable object. Can only register a single object of each type. Heavily recommended to create a container if registering a collection of the same object.
		/// </summary>
        public void RegisterSaveable(ISaveable saveable, Type originalType)
        {
            if (m_RegisteredSaveables == null)
            {
                m_RegisteredSaveables = new Dictionary<Type, ISaveable>();
            }

            if (m_RegisteredSaveables.ContainsKey(originalType))
            {
                Debug.LogError("SaveManager : Trying to add two objects of the same type, [" + originalType.ToString() + "], as saveables. Recommended to create a container if multiples of the same item needs saving.");
                return;
            }

            m_RegisteredSaveables.Add(originalType, saveable);
        }

		/// <summary>
		/// Loads the given save file. If it doesn't exist, create a new one then load it.
		/// </summary>
        public bool LoadOrCreateSave(string saveName)
        {
            return LoadOrCreateSave(saveName, m_DefaultEncrypted);
        }

		/// <summary>
		/// Loads the given save file with specified encryption. If it doesn't exist, create a new one then load it.
		/// </summary>
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

		/// <summary>
		/// Creates a new save file. Returns false if a save file already exists.
		/// </summary>
        public bool CreateNewSaveFile(string saveName)
        {
            return CreateNewSaveFile(saveName, m_DefaultEncrypted);
        }

		/// <summary>
		/// Creates a new save file with specified encryption. Returns false if a save file already exists.
		/// </summary>
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

		/// <summary>
		/// Creates a new save file at given location. If it already exists, delete it first.
		/// </summary>
        public bool CreateOrOverwriteNewSaveFile(string saveName)
        {
            return CreateOrOverwriteNewSaveFile(saveName, m_DefaultEncrypted);
        }

		/// <summary>
		/// Creates a new save file at given location with specific encryption. If it already exists, delete it first.
		/// </summary>
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
            SaveData(encrypt);

            return true;
        }

		/// <summary>
		/// Loads a save file. Returns false if the file doesn't exist.
		/// </summary>
        public bool LoadSaveFile(string saveName)
        {
            return LoadSaveFile(saveName, m_DefaultEncrypted);
        }

		/// <summary>
		/// Loads a save file with specified encryption. Returns false if the file doesn't exist.
		/// </summary>
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

		/// <summary>
		/// Saves data to loaded save file.
		/// </summary>
        public void SaveData()
        {
            SaveData(m_DefaultEncrypted);
        }

		/// <summary>
		/// Saves data to loaded save file with specified encryption.
		/// </summary>
        public void SaveData(bool encrypt)
        {
            DataSaveablesToCache();
            DataCacheToFile(encrypt);
        }

		/// <summary>
		/// Loads data from a file.
		/// </summary>
        private void DataFileToCache(bool isEncrypted)
        {
            StreamReader reader = new StreamReader(m_LoadedSaveLocation);
            List<string> dataSet = new List<string>();

            while (reader.Peek() >= 0)
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

            foreach (string readyLine in readyData)
            {
                string[] readyItems = readyLine.Split('|');

                m_SavedData[readyItems[0]] = readyItems.Subdivide(1, readyItems.Length - 1).ToArray();
            }
        }

		/// <summary>
		/// Saves data to a file.
		/// </summary>
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

                foreach (string data in dataPair.Value)
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

		/// <summary>
		/// Loads data from all the saveable objects.
		/// </summary>
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

            foreach (KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
            {
                m_SavedData[saveablePair.Key.ToString()] = saveablePair.Value.SaveData();
            }
        }

		/// <summary>
		/// Loads data for all the saveables.
		/// </summary>
        private void DataCacheToSaveables()
        {
            if (m_SavedData == null)
            {
                Debug.LogError(
                    "SaveManager : Trying to move data to saveables while there is no data in the save cache. Write to cache first.");
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
                    Debug.Log("SaveManager : Data from file type " + saveablePair.Key.ToString() +
                              " not found. Setting default values.");
                    saveablePair.Value.LoadDefaultData();
                }
            }
        }

		/// <summary>
		/// Loads every saveables' default data.
		/// </summary>
        private void DataDefaultToSaveables()
        {
            foreach (KeyValuePair<Type, ISaveable> saveablePair in m_RegisteredSaveables)
            {
                saveablePair.Value.LoadDefaultData();
            }
        }

		/// <summary>
		/// Encrypts given data.
		/// </summary>
        private void EncryptData(ref string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = EncryptionUtil.EncryptString(data[i]);
            }
        }

		/// <summary>
		/// Decrypts given data.
		/// </summary>
        private void DecryptData(ref string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = EncryptionUtil.DecryptString(data[i]);
            }
        }
    }
}