#if UNITY_EDITOR

using UnityEngine;
using System.Collections.Generic;
using System.IO;

// Part of Recycle Bin by JPBotelho on Github : https://github.com/JPBotelho/Recycle-Bin
namespace Canty.Editor.RecycleBin
{
    public class RecycleBinPreferences : ScriptableObject
    {
        public List<string> IncludeExtensions = new List<string>()
        {
            ".cs",
            ".js",
            ".shader",
            ".mat",
            ".anim",
            ".unity",
            ".obj",
            ".fbx",
            ".dae",
            ".asset",
            ".prefab"
        };

        public List<string> ExcludeExtensions = new List<string>()
        {
            ".meta"
        };

        public List<TrashFile> Trash = new List<TrashFile>();


        [HideInInspector]
        public bool SaveAll = true;
        [HideInInspector]
        public bool SaveNone = false;
        [HideInInspector]
        public string FolderName = "Trash";
        [HideInInspector]
        public string Search = "";


        public bool IsEligibleToSave(FileInfo file)
        {
            if (SaveAll && !ExcludeExtensions.Contains(file.Extension) || IncludeExtensions.Contains(file.Extension) && !SaveNone)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static RecycleBinPreferences Create(out string path)
        {
            return ScriptableObjectUtility.CreateAsset<RecycleBinPreferences>(out path);
        }
    }
}

#endif