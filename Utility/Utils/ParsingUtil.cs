using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParsingUtil
{
    /// <summary>
    /// Parse CSV file to extract a single column. Assumes column 0 is the data key.
    /// </summary>
    /// <param name="path">Path where the file should be.</param>
    /// <param name="filename">Filename without extention (it'll add it automatically).</param>
    /// <param name="column">Extracted column.</param>
    /// <returns>Dictionary containing extracted data.</returns>
    public static Dictionary<string, string> LoadCSV_IDColumn(string path, string filename, int column)
    {
        if (column == -1)
        {
            Debug.LogError("ParsingUtil Error : Invalid column number provided. Will not load file [" + filename + "].");
            return null;
        }

        string finalFilename = path + filename + ".csv";

        System.IO.FileStream readStream = System.IO.File.OpenRead(finalFilename);

        if (readStream == null)
        {
            Debug.LogError("ParsingUtil Error : Filename is invalid. Will not load file [" + filename + "].");
        }

        readStream.Dispose();

        Dictionary<string, string> localization = new Dictionary<string, string>();

        string[] file = System.IO.File.ReadAllText(finalFilename).Split('\n');
        bool skipFirst = false;

        foreach (string line in file)
        {
            if (skipFirst == false)
            {
                skipFirst = true;
                continue;
            }

            int index = 0;
            int wordIndex = 0;

            string id = "";

            while (true)
            {
                int newIndex = line.IndexOfAny(new char[] { ',', '"', '\r' }, index);

                if (newIndex == -1)
                {
                    string word = line.Substring(index, line.Length - index);

                    if (word != string.Empty)
                    {
                        if (wordIndex == 0)
                        {
                            Debug.LogWarning("ParsingUtil Warning : Reached end of the line with only id found inside file [" + filename + "].");
                        }
                        else if (wordIndex == column)
                        {
                            if (localization.ContainsKey(id))
                            {
                                Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                            }
                            else
                            {
                                localization.Add(id, word);
                            }
                        }
                    }

                    break;
                }

                if (line[newIndex] == ',')
                {
                    if (wordIndex == 0)
                    {
                        id = line.Substring(index, newIndex - index);

                        wordIndex++;
                    }
                    else if (wordIndex == column)
                    {
                        string word = line.Substring(index, newIndex - index);
                        if (localization.ContainsKey(id))
                        {
                            Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                        }
                        else
                        {
                            localization.Add(id, word);
                        }

                        break;
                    }
                    else
                    {
                        wordIndex++;
                    }
                }
                else if (line[newIndex] == '"')
                {
                    int quoteIndex = line.IndexOf('"', newIndex + 1);
                    index++;

                    if (wordIndex == 0)
                    {
                        id = line.Substring(index, quoteIndex - index);

                        wordIndex++;

                        index = quoteIndex + 1;
                        continue;
                    }
                    else if (wordIndex == column)
                    {
                        string word = line.Substring(index, quoteIndex - index);
                        if (localization.ContainsKey(id))
                        {
                            Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                        }
                        else
                        {
                            localization.Add(id, word);
                        }

                        break;
                    }
                    else
                    {
                        index = quoteIndex + 1;
                        continue;
                    }
                }
                else if (line[newIndex] == '\r')
                {
                    if (wordIndex == 0)
                    {
                        Debug.LogWarning("ParsingUtil Warning : Reached end of the line with only id found inside file [" + filename + "].");
                    }
                    else if (wordIndex == column)
                    {
                        string word = line.Substring(index, newIndex - index);
                        if (localization.ContainsKey(id))
                        {
                            Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                        }
                        else
                        {
                            localization.Add(id, word);
                        }
                    }

                    break;
                }
                else
                {
                    Debug.LogError("ParsingUtil Error : Wrong character is being loaded.");
                }
                
                index = newIndex + 1;
            }
        }

        return localization;
    }

    /// <summary>
    /// Parse CSV file to extract multiple columns. Assumes column 0 is the data key.
    /// </summary>
    /// <param name="path">Path where the file should be.</param>
    /// <param name="filename">Filename without extention (it'll add it automatically).</param>
    /// <param name="columns">Extracted columns.</param>
    /// <returns>Dictionary containing extracted data.</returns>
    public static Dictionary<string, List<string>> LoadCSV_IDMultipleColumn(string path, string filename, int[] columns)
    {
        string finalFilename = path + filename + ".csv";

        System.IO.FileStream readStream = System.IO.File.OpenRead(finalFilename);

        if (readStream == null)
        {
            Debug.LogError("ParsingUtil Error : Filename is invalid. Will not load file [" + filename + "].");
        }

        readStream.Dispose();

        Dictionary<string, List<string>> finalLocalization = new Dictionary<string, List<string>>();

        string data = System.IO.File.ReadAllText(finalFilename);

        foreach(int i in columns)
        {
            Dictionary<string, string> localization = LoadCSV_PartialIDColumn(data, i, filename);

            foreach(KeyValuePair<string, string> pair in localization)
            {
                if (finalLocalization.ContainsKey(pair.Key))
                {
                    finalLocalization[pair.Key].Add(pair.Value);
                }
                else
                {
                    finalLocalization.Add(pair.Key, new List<string> { pair.Value });
                }
            }
        }

        return finalLocalization;
    }

    /// <summary>
    /// Parse CSV file to extract all columns. Assumes column 0 is the data key.
    /// </summary>
    /// <param name="path">Path where the file should be.</param>
    /// <param name="filename">Filename without extention (it'll add it automatically).</param>
    /// <returns>Dictionary containing extracted data.</returns>
    public static Dictionary<string, List<string>> LoadCSV_AllColumn(string path, string filename)
    {
        string finalFilename = path + filename + ".csv";

        System.IO.FileStream readStream = System.IO.File.OpenRead(finalFilename);

        if (readStream == null)
        {
            Debug.LogError("ParsingUtil Error : Filename is invalid. Will not load file [" + filename + "].");
        }

        readStream.Dispose();

        Dictionary<string, List<string>> localization = new Dictionary<string, List<string>>();

        string[] file = System.IO.File.ReadAllText(finalFilename).Split('\n');
        bool skipFirst = false;

        foreach (string line in file)
        {
            if (skipFirst == false)
            {
                skipFirst = true;
                continue;
            }

            List<string> row = new List<string>();

            int index = 0;

            while (true)
            {
                int newIndex = line.IndexOfAny(new char[] { ',', '"', '\r' }, index);

                if (newIndex == -1)
                {
                    string word = line.Substring(index, line.Length - index);

                    if (word != string.Empty)
                    {
                        row.Add(word);
                    }

                    break;
                }
                else if (line[newIndex] == ',')
                {
                    string word = line.Substring(index, newIndex - index);

                    if (word != string.Empty)
                    {
                        row.Add(word);
                    }
                }
                else if (line[newIndex] == '"')
                {
                    int quoteIndex = line.IndexOf('"', newIndex + 1);
                    index++;

                    string word = line.Substring(index, quoteIndex - index);

                    if (word != string.Empty)
                    {
                        row.Add(word);
                    }

                    index = quoteIndex + 1;
                    continue;
                }
                else if (line[newIndex] == '\r')
                {
                    string word = line.Substring(index, newIndex - index);

                    if (word != string.Empty)
                    {
                        row.Add(word);
                    }
                }
                else
                {
                    Debug.LogError("ParsingUtil Error : Wrong character is being loaded.");
                }
                
                index = newIndex + 1;
            }

            string key = row[0];
            row.RemoveAt(0);
            localization.Add(key, row);
        }

        return localization;
    }

    //Used for LoadCSV_IDMultipleColumn.
    private static Dictionary<string, string> LoadCSV_PartialIDColumn(string data, int column, string filename)
    {
        Dictionary<string, string> localization = new Dictionary<string, string>();

        string[] file = data.Split('\n');
        bool skipFirst = false;

        foreach (string line in file)
        {
            if (skipFirst == false)
            {
                skipFirst = true;
                continue;
            }

            int index = 0;
            int wordIndex = 0;

            string id = "";

            while (true)
            {
                int newIndex = line.IndexOfAny(new char[] { ',', '"', '\r' }, index);

                if (newIndex == -1)
                {
                    string word = line.Substring(index, line.Length - index);

                    if (word != string.Empty)
                    {
                        if (wordIndex == 0)
                        {
                            Debug.LogWarning("ParsingUtil Warning : Reached end of the line with only id found inside file [" + filename + "].");
                        }
                        else if (wordIndex == column)
                        {
                            if (localization.ContainsKey(id))
                            {
                                Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                            }
                            else
                            {
                                localization.Add(id, word);
                            }
                        }
                    }

                    break;
                }

                if (line[newIndex] == ',')
                {
                    if (wordIndex == 0)
                    {
                        id = line.Substring(index, newIndex - index);

                        wordIndex++;
                    }
                    else if (wordIndex == column)
                    {
                        string word = line.Substring(index, newIndex - index);
                        if (localization.ContainsKey(id))
                        {
                            Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                        }
                        else
                        {
                            localization.Add(id, word);
                        }

                        break;
                    }
                    else
                    {
                        wordIndex++;
                    }
                }
                else if (line[newIndex] == '"')
                {
                    int quoteIndex = line.IndexOf('"', newIndex + 1);
                    index++;

                    if (wordIndex == 0)
                    {
                        id = line.Substring(index, quoteIndex - index);

                        wordIndex++;

                        index = quoteIndex + 1;
                    }
                    else if (wordIndex == column)
                    {
                        string word = line.Substring(index, quoteIndex - index);
                        if (localization.ContainsKey(id))
                        {
                            Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                        }
                        else
                        {
                            localization.Add(id, word);
                        }

                        break;
                    }
                    else
                    {
                        index = quoteIndex + 1;
                        continue;
                    }
                }
                else if (line[newIndex] == '\r')
                {
                    if (wordIndex == 0)
                    {
                        Debug.LogWarning("ParsingUtil Warning : Reached end of the line with only id found inside file [" + filename + "].");
                    }
                    else if (wordIndex == column)
                    {
                        string word = line.Substring(index, newIndex - index);
                        if (localization.ContainsKey(id))
                        {
                            Debug.LogError("ParsingUtil : Localization key already present. Key : " + id + ". File : " + filename + ".");
                        }
                        else
                        {
                            localization.Add(id, word);
                        }
                    }

                    break;
                }
                else
                {
                    Debug.LogError("ParsingUtil Error : Wrong character is being loaded.");
                }
                
                index = newIndex + 1;
            }
        }

        return localization;
    }
}