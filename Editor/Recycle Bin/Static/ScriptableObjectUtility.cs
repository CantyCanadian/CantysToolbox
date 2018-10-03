#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

// Part of Recycle Bin by JPBotelho on Github : https://github.com/JPBotelho/Recycle-Bin

namespace Canty.Editor.RecycleBin
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static T CreateAsset<T>(out string finalPath) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/Trash Can" + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            finalPath = assetPathAndName;
            return asset;
        }

        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}

#endif