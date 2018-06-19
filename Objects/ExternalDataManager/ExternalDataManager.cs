using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalDataManager : Singleton<ExternalDataManager>
{
    public string PathFromRoot = "Assets/Assets/External";
    public int FilesPerFrame = 999;

    private List<LoadingData> m_LoadingList;
    private Dictionary<string, Dictionary<string, bool>> m_BoolContainers;
    private Dictionary<string, Dictionary<string, int>> m_IntContainers;
    private Dictionary<string, Dictionary<string, float>> m_FloatContainers;
    private Dictionary<string, Dictionary<string, string>> m_StringContainers;
    private Dictionary<string, Dictionary<string, IDataLoadable>> m_ObjectContainers;

    private float m_Progress = 0.0f;
    private bool m_DataLoaded = false;

    /// <summary>
    /// How should the data pulled from the file is loaded.
    /// </summary>
    public enum DataType
    {
        Bool,
        Int,
        Float,
        String,
        Object
    }

    /// <summary>
    /// Load data from a CSV file using the first column after the key as value.
    /// </summary>
    /// <param name="dictionaryKey">Key of the dictionnary this should be in.</param>
    /// <param name="file">Name of the file, excluding path and extention.</param>
    /// <param name="data">Which type of data will be extracted.</param>
    public void PrepareSingleColumnData(string dictionaryKey, string file, DataType data)
    {
        PrepareSingleColumnData(dictionaryKey, file, 1, data);
    }

    /// <summary>
    /// Load data from a CSV file using the specified column as value.
    /// </summary>
    /// <param name="dictionaryKey">Key of the dictionnary this should be in.</param>
    /// <param name="file">Name of the file, excluding path and extention.</param>
    /// <param name="column">Which column should be extracted.</param>
    /// <param name="data">Which type of data will be extracted.</param>
    public void PrepareSingleColumnData(string dictionaryKey, string file, int column, DataType data)
    {
        m_LoadingList = new List<LoadingData>();

        LoadingData loading;
        loading.Key = dictionaryKey;
        loading.File = file;
        loading.Columns = new int[] { column };
        loading.Data = data;
        loading.Loading = LoadingType.Single;
        loading.Object = null;

        m_LoadingList.Add(loading);
    }

    /// <summary>
    /// Load data from a CSV file using the specified columns as values to be passed into a loadable object.
    /// </summary>
    /// <typeparam name="T">Class type that inherits from IDataLoadable and is non-abstract (must be able to use 'new' keyword).</typeparam>
    /// <param name="dictionaryKey">Key of the dictionnary this should be in.</param>
    /// <param name="file">Name of the file, excluding path and extention.</param>
    /// <param name="column">Which column should be extracted.</param>
    public void PrepareMultipleColumnsObject<T>(string dictionaryKey, string file, int[] column) where T : IDataLoadable, new()
    {
        m_LoadingList = new List<LoadingData>();

        LoadingData loading;
        loading.Key = dictionaryKey;
        loading.File = file;
        loading.Columns = column;
        loading.Data = DataType.Object;
        loading.Loading = LoadingType.Multiple;
        loading.Object = new T();

        m_LoadingList.Add(loading);
    }

    /// <summary>
    /// Load data from a CSV file using all the columns as values to be passed into a loadable object.
    /// </summary>
    /// <typeparam name="T">Class type that inherits from IDataLoadable and is non-abstract (must be able to use 'new' keyword).</typeparam>
    /// <param name="dictionaryKey">Key of the dictionnary this should be in.</param>
    /// <param name="file">Name of the file, excluding path and extention.</param>
    public void PrepareAllColumnsObject<T>(string dictionaryKey, string file) where T : IDataLoadable, new()
    {
        m_LoadingList = new List<LoadingData>();

        LoadingData loading;
        loading.Key = dictionaryKey;
        loading.File = file;
        loading.Columns = new int[] { -1 };
        loading.Data = DataType.Object;
        loading.Loading = LoadingType.All;
        loading.Object = new T();

        m_LoadingList.Add(loading);
    }

    /// <summary>
    /// Load the prepared data through a coroutine running 'parallel' to the game.
    /// </summary>
    public void LoadData()
    {
        if (m_DataLoaded == false)
        {
            StartCoroutine(LoadingLoop());
        }
    }

    private enum LoadingType
    {
        Single,
        Multiple,
        All
    }

    private struct LoadingData
    {
        public string Key;
        public string File;
        public int[] Columns;
        public DataType Data;
        public LoadingType Loading;
        public IDataLoadable Object;
    }

    private IEnumerator LoadingLoop()
    {
        m_BoolContainers = new Dictionary<string, Dictionary<string, bool>>();
        m_IntContainers = new Dictionary<string, Dictionary<string, int>>();
        m_FloatContainers = new Dictionary<string, Dictionary<string, float>>();
        m_StringContainers = new Dictionary<string, Dictionary<string, string>>();
        m_ObjectContainers = new Dictionary<string, Dictionary<string, IDataLoadable>>();

        int files = 0;
        m_Progress = 0.0f;

        float progressPerLD = 1.0f / m_LoadingList.Count;

        foreach (LoadingData ld in m_LoadingList)
        {
            switch (ld.Loading)
            {
                case LoadingType.Single:
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>();

                        data.Append(ParsingUtil.LoadCSV_IDColumn(PathFromRoot, ld.File, ld.Columns[0]));

                        switch (ld.Data)
                        {
                            case DataType.Bool:
                                if (!m_BoolContainers.ContainsKey(ld.Key))
                                {
                                    m_BoolContainers.Add(ld.Key, new Dictionary<string, bool>());
                                }
                                m_BoolContainers[ld.Key].Append(data.ConvertUsing((obj) => { return obj.ToLower() == "true"; }));
                                break;

                            case DataType.Int:
                                if (!m_IntContainers.ContainsKey(ld.Key))
                                {
                                    m_IntContainers.Add(ld.Key, new Dictionary<string, int>());
                                }
                                m_IntContainers[ld.Key].Append(data.ConvertUsing((obj) => { return int.Parse(obj); }));
                                break;

                            case DataType.Float:
                                if (!m_FloatContainers.ContainsKey(ld.Key))
                                {
                                    m_FloatContainers.Add(ld.Key, new Dictionary<string, float>());
                                }
                                m_FloatContainers[ld.Key].Append(data.ConvertUsing((obj) => { return float.Parse(obj); }));
                                break;

                            case DataType.String:
                                if (!m_StringContainers.ContainsKey(ld.Key))
                                {
                                    m_StringContainers.Add(ld.Key, new Dictionary<string, string>());
                                }
                                m_StringContainers[ld.Key].Append(data);
                                break;

                            case DataType.Object:
                                if (!m_ObjectContainers.ContainsKey(ld.Key))
                                {
                                    m_ObjectContainers.Add(ld.Key, new Dictionary<string, IDataLoadable>());
                                }
                                m_ObjectContainers[ld.Key].Append(data.ConvertUsing((obj) => { IDataLoadable load = ld.Object.Clone(); load.LoadDataStrings(new string[] { obj }); return load; }));
                                break;
                        }
                    }
                    break;

                case LoadingType.Multiple:
                    {
                        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

                        data.Append(ParsingUtil.LoadCSV_IDMultipleColumn(PathFromRoot, ld.File, ld.Columns));

                        if (!m_ObjectContainers.ContainsKey(ld.Key))
                        {
                            m_ObjectContainers.Add(ld.Key, new Dictionary<string, IDataLoadable>());
                        }
                        m_ObjectContainers[ld.Key].Append(data.ConvertUsing((obj) => { IDataLoadable load = ld.Object.Clone(); load.LoadDataStrings(obj.ToArray()); return load; }));
                    }
                    break;

                case LoadingType.All:
                    {
                        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

                        data.Append(ParsingUtil.LoadCSV_AllColumn(PathFromRoot, ld.File));

                        if (!m_ObjectContainers.ContainsKey(ld.Key))
                        {
                            m_ObjectContainers.Add(ld.Key, new Dictionary<string, IDataLoadable>());
                        }
                        m_ObjectContainers[ld.Key].Append(data.ConvertUsing((obj) => { IDataLoadable load = ld.Object.Clone(); load.LoadDataStrings(obj.ToArray()); return load; }));
                    }
                    break;
            }

            files++;
            m_Progress += progressPerLD;

            if (files >= FilesPerFrame)
            {
                files = 0;
                yield return null;
            }
        }
    }
}
