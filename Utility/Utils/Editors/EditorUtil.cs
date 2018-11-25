﻿///====================================================================================================
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
        private static System.Type ProjectWindowType = typeof(EditorWindow).Assembly.GetType(" UnityEditor.ProjectBrowser");

        private static EditorWindow projectWindow = null;

        /// <summary>
        /// Small editor hack, allowing selected asset to be flagged as being renamed.
        /// </summary>
        public static void StartRenameSelectedAsset()
        {
            if (projectWindow == null)
            {
                projectWindow = EditorWindow.GetWindow(ProjectWindowType);
            }

            if (projectWindow != null)
            {
                Event f2Event = new Event();
                f2Event.keyCode = KeyCode.F2;
                f2Event.type = EventType.KeyDown;
                projectWindow.SendEvent(f2Event);
            }
        }

        /// <summary>
        /// Returns the path of an asset, but without including the asset name in the path itself.
        /// </summary>
        public static string GetAssetFolderPath(Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);

            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            return path;
        }

        public static GameObject CreateMenuItemGameObject(MenuCommand menuCommand, string name)
        {
            GameObject menuItemObject = new GameObject(name);

            GameObjectUtility.SetParentAndAlign(menuItemObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(menuItemObject, "Create " + menuItemObject.name);
            Selection.activeObject = menuItemObject;

            return menuItemObject;
        }
    }
}

#endif