#if UNITY_EDITOR

using UnityEditor;

// Part of Recycle Bin by JPBotelho on Github : https://github.com/JPBotelho/Recycle-Bin
namespace Canty.Editor.RecycleBin
{
	public class DropdownMenu : EditorWindow
	{
		[MenuItem("Window/Recycle Bin/Show")]
		public static void ShowWindow()
		{
			string path = AssetDatabase.GetAssetPath(RecycleBinFunctions.RecycleBinPreferences);

			if (!string.IsNullOrEmpty(path))
			{
				Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
			}
			else
			{
				RecycleBinPreferences.Create(out path);

				Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
			}

			RecycleBinFunctions.RefreshSearch("");
		}

		[MenuItem("Window/Recycle Bin/Open Folder")]
		public static void OpenTrash()
		{
			FileFunctions.OpenFolder(RecycleBinFunctions.GetRecycleBinAndCreateIfNull());
		}
	}
}

#endif