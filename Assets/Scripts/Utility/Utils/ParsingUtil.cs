using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParsingUtil
{
    public static Dictionary<string, string> LoadCSV_IDColumn(string filename, int column)
    {
        if (column == -1)
        {
            Debug.LogError("ParsingUtil Error : Invalid column number provided. Will not load file [" + filename + "].");
            return null;
        }

        string finalFilename = "Assets/Assets/Localization/" + filename + ".csv";

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

    public static List<List<string>> LoadCSV_IDMultipleColumn(string filename, int[] columns)
    {
        string finalFilename = "Assets/Assets/Localization/" + filename + ".csv";

        System.IO.FileStream readStream = System.IO.File.OpenRead(finalFilename);

        if (readStream == null)
        {
            Debug.LogError("ParsingUtil Error : Filename is invalid. Will not load file [" + filename + "].");
        }

        readStream.Dispose();

        List<List<string>> finalLocalization = new List<List<string>>();

        string data = System.IO.File.ReadAllText(finalFilename);

        foreach(int i in columns)
        {
            Dictionary<string, string> localization = LoadCSV_PartialIDColumn(data, i, filename);

            foreach(KeyValuePair<string, string> pair in localization)
            {
                bool success = false;

                for(int j = 0; j < finalLocalization.Count; j++)
                {
                    if (finalLocalization[j][0] == pair.Key)
                    {
                        finalLocalization[j].Add(pair.Value);
                        success = true;
                    }
                }

                if (success == false)
                {
                    List<string> newList = new List<string>();
                    newList.Add(pair.Key);
                    newList.Add(pair.Value);
                    finalLocalization.Add(newList);
                }
            }
        }

        return finalLocalization;
    }

    public static List<List<string>> LoadCSV_AllColumn(string filename)
    {
        string finalFilename = "Assets/Assets/Localization/" + filename + ".csv";

        System.IO.FileStream readStream = System.IO.File.OpenRead(finalFilename);

        if (readStream == null)
        {
            Debug.LogError("ParsingUtil Error : Filename is invalid. Will not load file [" + filename + "].");
        }

        readStream.Dispose();

        List<List<string>> localization = new List<List<string>>();

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

            localization.Add(row);
        }

        return localization;
    }

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