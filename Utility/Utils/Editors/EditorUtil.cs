///====================================================================================================
///
///     EditorUtil by
///     - CantyCanadian
///
///====================================================================================================

#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Canty
{
    public static class EditorUtil
    {
        /// <summary>
        /// Returns the path of an asset.
        /// </summary>
        public static string GetAssetFolderPath(Object asset, bool includeAssetName = false)
        {
            string path = AssetDatabase.GetAssetPath(asset);

            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                if (!includeAssetName)
                {
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
                }
            }

            return path;
        }

        /// <summary>
        /// Shortcut method to create a GameObject in the world for the purpose of custom create utility functions.
        /// This is assumed to be used from inside a function with the MenuItem attribute, which adds a MenuCommand argument that can be passed through.
        /// </summary>
        public static GameObject CreateGameObjectInWorld(MenuCommand menuCommand, string name)
        {
            GameObject menuItemObject = new GameObject(name);

            GameObjectUtility.SetParentAndAlign(menuItemObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(menuItemObject, "Create " + menuItemObject.name);
            Selection.activeObject = menuItemObject;

            return menuItemObject;
        }

        /// <summary>
        /// Shortcut method to create a ScriptableObject from within the resource browser.
        /// </summary>
        public static void CreateScriptableObject<T>() where T : ScriptableObject
        {
            T scriptableObject = ScriptableObject.CreateInstance<T>();
            
            ProjectWindowUtil.CreateAsset(scriptableObject, "New" + typeof(T).ToString() + ".asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Shortcut method to obtain an icon object with built-in tooltip.
        /// </summary>
        public static GUIContent IconContent(string name, string tooltip)
        {
            GUIContent builtinIcon = EditorGUIUtility.IconContent(name);
            return new GUIContent(builtinIcon.image, tooltip);
        }
    }
}

#endif