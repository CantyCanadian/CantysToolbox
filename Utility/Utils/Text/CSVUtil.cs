///====================================================================================================
///
///     CSVUtil by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class CSVUtil
    {
        /// <summary>
        /// Parse CSV file to extract a single column. Assumes column 0 is the data key.
        /// </summary>
        /// <param name="file">TextAsset file.</param>
        /// <param name="columns">Extracted column.</param>
        /// <returns>Dictionary containing extracted data.</returns>
        public static Dictionary<string, List<string>> LoadSingleColumn(TextAsset file, int column)
        {
            return LoadMultipleColumnsInternal(file.text, new int[] { column });
        }

        /// <summary>
        /// Parse CSV file to extract a single column. Assumes column 0 is the data key.
        /// </summary>
        /// <param name="path">Path where the file should be.</param>
        /// <param name="filename">Filename without extention (it'll add it automatically).</param>
        /// <param name="columns">Extracted column.</param>
        /// <returns>Dictionary containing extracted data.</returns>
        public static Dictionary<string, List<string>> LoadSingleColumn(string path, string filename, int column)
        {
            return LoadMultipleColumnsInternal(ReadDataFile(path, filename), new int[] { column });
        }

        /// <summary>
        /// Parse CSV file to extract multiple columns. Assumes column 0 is the data key.
        /// </summary>
        /// <param name="file">TextAsset file.</param>
        /// <param name="columns">Extracted columns.</param>
        /// <returns>Dictionary containing extracted data.</returns>
        public static Dictionary<string, List<string>> LoadMultipleColumns(TextAsset file, int[] columns)
        {
            return LoadMultipleColumnsInternal(file.text, columns);
        }

        /// <summary>
        /// Parse CSV file to extract multiple columns. Assumes column 0 is the data key.
        /// </summary>
        /// <param name="path">Path where the file should be.</param>
        /// <param name="filename">Filename without extention (it'll add it automatically).</param>
        /// <param name="columns">Extracted columns.</param>
        /// <returns>Dictionary containing extracted data.</returns>
        public static Dictionary<string, List<string>> LoadMultipleColumns(string path, string filename, int[] columns)
        {
            return LoadMultipleColumnsInternal(ReadDataFile(path, filename), columns);
        }

        /// <summary>
        /// Parse CSV file to extract all columns. Assumes column 0 is the data key.
        /// </summary>
        /// <param name="file">TextAsset object.</param>
        /// <returns>Dictionary containing extracted data.</returns>
        public static Dictionary<string, List<string>> LoadAllColumns(TextAsset file)
        {
            return LoadAllColumnsInternal(file.text);
        }

        /// <summary>
        /// Parse CSV file to extract all columns. Assumes column 0 is the data key.
        /// </summary>
        /// <param name="path">Path where the file should be.</param>
        /// <param name="filename">Filename without extention (it'll add it automatically).</param>
        /// <returns>Dictionary containing extracted data.</returns>
        public static Dictionary<string, List<string>> LoadAllColumns(string path, string filename)
        {
            return LoadAllColumnsInternal(ReadDataFile(path, filename));
        }

        private static Dictionary<string, List<string>> LoadMultipleColumnsInternal(string data, int[] columns)
        {
            Dictionary<string, List<string>> finalLocalization = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> localization = LoadAllColumnsInternal(data);

            foreach (KeyValuePair<string, List<string>> pair in localization)
            {
                List<string> results = new List<string>();

                foreach (int i in columns)
                {
                    results.Add(localization[pair.Key][i]);
                }

                finalLocalization.Add(pair.Key, results);
            }

            return finalLocalization;
        }


        private static Dictionary<string, List<string>> LoadAllColumnsInternal(string data)
        {
            Dictionary<string, List<string>> localization = new Dictionary<string, List<string>>();

            string[] file = data.Split('\n');
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
                    int newIndex = line.IndexOfAny(new char[] {',', '"', '\r'}, index);

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

        private static string ReadDataFile(string path, string filename)
        {
            string finalFilename = path + filename + ".csv";

            System.IO.FileStream readStream = System.IO.File.OpenRead(finalFilename);

            if (readStream == null)
            {
                Debug.LogError("ParsingUtil Error : Filename is invalid. Will not load file [" + filename + "].");
            }

            readStream.Dispose();

            return System.IO.File.ReadAllText(finalFilename);
        }
    }
}