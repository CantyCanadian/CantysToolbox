///====================================================================================================
///
///     ExternalDataManager by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canty.Managers
{
    public class ExternalDataManager : Singleton<ExternalDataManager>
    {
        private struct PreparedFileContainer
        {
            public PreparedFileContainer(TextAsset file, int[] columns)
            {
                File = file;
                Columns = columns;
            }

            public TextAsset File;
            public int[] Columns;
        }

        private Dictionary<string, PreparedFileContainer> m_PreparedFiles = null;
        private Dictionary<string, Dictionary<string, string[]>> m_Data = null;

        /// <summary>
        /// Checks if the passed-in key exists.
        /// </summary>
        public bool HasData(string fileKey, string itemKey)
        {
            return m_Data[fileKey].ContainsKey(itemKey);
        }

        /// <summary>
        /// Gets all the keys associated to the file key.
        /// </summary>
        public string[] GetKeys(string fileKey)
        {
            return m_Data[fileKey].ExtractKeys().ToArray();
        }

        /// <summary>
        /// Gets how many items are loaded for the file key.
        /// </summary>
        public int GetItemCount(string fileKey)
        {
            return m_Data[fileKey].Count;
        }

        /// <summary>
        /// Gets how many items in total are loaded.
        /// </summary>
        public int GetAllItemCount()
        {
            int result = 0;
            foreach (KeyValuePair<string, Dictionary<string, string[]>> data in m_Data)
            {
                result += data.Value.Count;
            }

            return result;
        }

        /// <summary>
        /// Returns how many values there are for a given file and item.
        /// </summary>
        public int GetValueCount(string fileKey, string itemKey)
        {
            return m_Data[fileKey][itemKey].Length;
        }

        /// <summary>
        /// Gets the first string associated to the key from cache.
        /// </summary>
        public string GetValue(string fileKey, string itemKey)
        {
            return m_Data[fileKey][itemKey][0];
        }

        /// <summary>
        /// Gets a specific string associated to the key from cache.
        /// </summary>
        public string GetValue(string fileKey, string itemKey, int index)
        {
            return m_Data[fileKey][itemKey][index];
        }

        /// <summary>
        /// Gets the first string associated to the key from cache, converted in the desired type.
        /// </summary>
        public T GetValue<T>(string fileKey, string itemKey) where T : IConvertible
        {
            return m_Data[fileKey][itemKey][0].ConvertTo<T>();
        }

        /// <summary>
        /// Gets a specific string associated to the key from cache, converted in the desired type.
        /// </summary>
        public T GetValue<T>(string fileKey, string itemKey, int index) where T : IConvertible
        {
            return m_Data[fileKey][itemKey][index].ConvertTo<T>();
        }

        /// <summary>
        /// Gets all the strings associated to the key from cache.
        /// </summary>
        public string[] GetValues(string fileKey, string itemKey)
        {
            return m_Data[fileKey][itemKey];
        }

        /// <summary>
        /// Gets all the strings associated to the key from cache, converted in the desired type.
        /// </summary>
        public T[] GetValues<T>(string fileKey, string itemKey) where T : IConvertible
        {
            return m_Data[fileKey][itemKey].ConvertUsing<string, T, List<T>>((obj) => { return obj.ConvertTo<T>(); }).ToArray();
        }

        /// <summary>
        /// Gets all the items and their strings from a file.
        /// </summary>
        public Dictionary<string, string[]> GetAllValues(string fileKey)
        {
            return m_Data[fileKey];
        }

        /// <summary>
        /// Sets the file as ready to be loaded. Will load all its columns.
        /// </summary>
        public void PrepareFile(TextAsset file, string fileKey)
        {
            PrepareFile(file, fileKey, new int[] { -1 });
        }

        /// <summary>
        /// Sets the file as ready to be loaded. Will load only the specified column.
        /// </summary>
        public void PrepareFile(TextAsset file, string fileKey, int column)
        {
            if (column <= 0)
            {
                Debug.Log("ExternalDataManager : Column provided to prepare file " + file.name + " is invalid.");
                return;
            }

            PrepareFile(file, fileKey, new int[] { column });
        }

        /// <summary>
        /// Sets the file as ready to be loaded. Will load only the specified columns.
        /// </summary>
        public void PrepareFile(TextAsset file, string fileKey, int[] column)
        {
            if (m_PreparedFiles == null)
            {
                m_PreparedFiles = new Dictionary<string, PreparedFileContainer>();
            }

            m_PreparedFiles.Add(fileKey, new PreparedFileContainer(file, column));
        }

        /// <summary>
        /// Load all the prepared files into cache.
        /// </summary>
        public void LoadPreparedFiles()
        {
            if (m_PreparedFiles == null)
            {
                return;
            }

            if (m_Data == null)
            {
                m_Data = new Dictionary<string, Dictionary<string, string[]>>();
            }

            foreach (KeyValuePair<string, PreparedFileContainer> file in m_PreparedFiles)
            {
                if (file.Value.Columns.Length == 0)
                {
                    Debug.Log("ExternalDataManager : Trying to load file " + file.Key + " but it has no specified column.");
                }
                else if (file.Value.Columns.Length == 1)
                {
                    if (file.Value.Columns[0] == -1)
                    {
                        Dictionary<string, List<string>> loadedData = CSVUtil.LoadAllColumns(file.Value.File);
                        m_Data.Add(file.Key, loadedData.ConvertUsing((obj) => { return obj.ToArray(); }));
                    }
                    else
                    {
                        Dictionary<string, List<string>> loadedData = CSVUtil.LoadSingleColumn(file.Value.File, file.Value.Columns[0]);
                        m_Data.Add(file.Key, loadedData.ConvertUsing((obj) => { return obj.ToArray(); }));
                    }
                }
                else
                {
                    Dictionary<string, List<string>> loadedData = CSVUtil.LoadMultipleColumns(file.Value.File, file.Value.Columns);
                    m_Data.Add(file.Key, loadedData.ConvertUsing((obj) => { return obj.ToArray(); }));
                }
            }

            m_PreparedFiles.Clear();
        }
    }
}