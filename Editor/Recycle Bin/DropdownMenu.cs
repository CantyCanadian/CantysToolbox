///====================================================================================================
///
///     DropdownMenu by
///     - CantyCanadian
///     - JPBotelho
///
///====================================================================================================

#if UNITY_EDITOR

using UnityEditor;

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