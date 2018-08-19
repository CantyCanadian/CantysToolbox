#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

// Part of Recycle Bin by JPBotelho on Github : https://github.com/JPBotelho/Recycle-Bin

namespace JPBotelho
{
	public static class RecycleBinFunctions
	{
		public static string RecycleBinPath
		{
			get { return GetRecycleBinAndCreateIfNull(); }
		}

		public static RecycleBinPreferences RecycleBinPreferences
		{
			get { return GetRecycleBinPreferences(); }
		}

		public static string ProjectFolder
		{
			get { return GetProjectFolder(); }
		}

        /// <summary>
        /// Returns the recycle bin folder. Create folder if it doesn't exist.
        /// </summary>
        /// <returns>Expected recycle bin path.</returns>
        public static string GetRecycleBinAndCreateIfNull()
		{
			string expectedRecycleBinPath = Path.Combine(ProjectFolder, RecycleBinPreferences.FolderName);

			if (!Directory.Exists(expectedRecycleBinPath))
			{
				return CreateRecycleBinDirectory();
			}

			return expectedRecycleBinPath;
		}

		/// <summary>
		/// Clears the trash directory.
		/// </summary>
		public static void ClearRecycleBinDirectory()
		{
			string folderPath = RecycleBinPath;
			FileFunctions.ClearDirectory(new DirectoryInfo(folderPath));
			//This is here for the sake of it, might just set preferences.trash to an empty list.
			RefreshSearch("");
		}

		/// <summary>
		/// Copies a File / Directory at a given path to the trash folder if eligible.
		/// </summary>
		public static void DeleteAndCopyToRecycleBin(FileInfo file)
		{
			//The input parameter is relative to the project folder e.g.: /Assets/MyScript.cs
			string assetPath = Path.Combine(ProjectFolder, file.FullName); 

			DirectoryInfo recycleBinDirectory = new DirectoryInfo(RecycleBinPath);

			if (FileFunctions.IsDirectory(assetPath))
			{
				FileFunctions.CopyFileOrDirectory(assetPath, recycleBinDirectory);
			}
			else
			{
				if (RecycleBinPreferences.IsEligibleToSave(file))
				{
					FileFunctions.CopyFileOrDirectory(assetPath, recycleBinDirectory);
				}
			}

			FileUtil.DeleteFileOrDirectory(assetPath);

			RefreshSearch("");
		}
        
        /// <summary>
        /// Copies all the files in the recycle bin to the assets folder.
        /// </summary>
        public static void CopyFilesFromBinToAssetsFolder()
		{
			List<string> paths = new List<string>();

			paths.AddRange(Directory.GetFiles(RecycleBinPath));
			paths.AddRange(Directory.GetDirectories(RecycleBinPath));

			for (int i = 0; i < paths.Count; i++)
			{
				string assetPath = Path.Combine(Application.dataPath, Path.GetFileName(paths[i]));

				FileUtil.CopyFileOrDirectory(paths[i], assetPath);
				FileUtil.DeleteFileOrDirectory(paths[i]);
				AssetDatabase.Refresh();

				RefreshSearch("");
			}
		}

		/// <summary>
		/// Refreshes the trash search. If search string is empty (""), all objects are displayed
		/// </summary>
		/// <param name="search">Search term.</param>
		public static void RefreshSearch(string search)
		{
			RecycleBinPreferences.Trash = SearchFilesInRecycleBin(search);
			RecycleBinPreferences.Trash.Sort(new ComparerByDate());
		}
       
        
        /// <summary>
        /// Formats dates to custom standard (DD/MM/YYYY).
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDate(System.DateTime date)
		{
			string minute = date.Minute.ToString();

			if (minute.ToCharArray().Length == 1)
				minute = "0" + minute.ToCharArray()[0];

			return date.Day + "/" + date.Month + "/" + date.Year + "   " + date.Hour + ":" + minute;
		}

        /// <summary>
        /// Returns the Preferences scriptable asset file.
        /// </summary>
        /// <returns>The Preferences asset file.</returns>
        private static RecycleBinPreferences GetRecycleBinPreferences()
        {
            List<RecycleBinPreferences> preferences = ScriptableObjectUtility.FindAssetsByType<RecycleBinPreferences>();

            //Multiple instances?
            if (preferences.Count >= 1)
            {
                return preferences[0];
            }
            else
            {
                string unusedReturnsPath;
                return RecycleBinPreferences.Create(out unusedReturnsPath);
            }
        }

        /// <summary>
        /// Gets all files in the recycle bin.
        /// </summary>
        /// <param name="search">Searches for files that contain this search. Case insensitive.</param>
        /// <returns>List with all the files in the bin.</returns>
        private static List<TrashFile> SearchFilesInRecycleBin(string search)
        {
            List<TrashFile> trashFiles = new List<TrashFile>();

            List<string> recycleBinMembers = FileFunctions.GetFilesAndDirectories(new DirectoryInfo(RecycleBinPath));

            foreach (string s in recycleBinMembers)
            {
                // "" returns everything. It's the default case.
                if (search == "" || s.ToLower().Contains(search.ToLower()))
                {
                    trashFiles.Add(new TrashFile(s));
                }
            }

            return trashFiles;
        }

        /// <summary>
        /// Creates a recycle bin folder.
        /// </summary>
        private static string CreateRecycleBinDirectory()
        {
            string finalPath = Path.Combine(ProjectFolder, RecycleBinPreferences.FolderName);

            Directory.CreateDirectory(Path.Combine(ProjectFolder, RecycleBinPreferences.FolderName));

            return finalPath;
        }

        /// <summary>
        /// Returns full path to the project folder.
        /// </summary>
        /// <returns>Full path.</returns>
        static string GetProjectFolder()
        {
            // -7 > Removes /Assets
            return Application.dataPath.Remove(Application.dataPath.ToCharArray().Length - 7);
        }
    }
}

#endif