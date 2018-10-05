///====================================================================================================
///
///     PreferenceEditor by
///     - CantyCanadian
///     - JPBotelho
///
///====================================================================================================

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Canty.Editor.RecycleBin
{
	[CustomEditor(typeof(RecycleBinPreferences))]
	public class PreferenceEditor : UnityEditor.Editor
	{
        public static GUIStyle Skin = new GUIStyle();

        private SerializedProperty m_All;
        private SerializedProperty m_None;
        private SerializedProperty m_Name;
        private SerializedProperty m_Search;

		private bool m_Settings; //Foldout
        private RecycleBinPreferences m_Preferences;

        private bool m_ShowSubfolders = true;
        private bool m_ShowDate = true;

        private string m_RecycleBin;

		public void OnEnable()
		{
			m_RecycleBin = RecycleBinFunctions.RecycleBinPath;

			m_ShowSubfolders = EditorPrefs.GetBool("RECYCLEBIN_SHOW");
			m_ShowDate = EditorPrefs.GetBool("RECYCLEBIN_DATE");

			m_Preferences = (RecycleBinPreferences)target;

			Skin.alignment = TextAnchor.MiddleCenter;
			Skin.fontStyle = FontStyle.Bold;

			m_All = serializedObject.FindProperty("SaveAll");
			m_None = serializedObject.FindProperty("SaveNone");
			m_Name = serializedObject.FindProperty("FolderName");
			m_Search = serializedObject.FindProperty("Search");

			RecycleBinFunctions.RefreshSearch("");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.LabelField("Recycle Bin", EditorStyles.centeredGreyMiniLabel);

			m_Settings = EditorGUILayout.Foldout(m_Settings, "Settings");

			if (m_Settings)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.Space();

				EditorGUILayout.PropertyField(m_Name, new GUIContent("Folder Name"));

				EditorGUILayout.Space();

				EditorGUILayout.LabelField("Path:");
				EditorGUILayout.LabelField(m_RecycleBin);

				EditorGUILayout.Space();

				//Draw everything besides script name and deleted file list. This way there's no need to reimplement arrays of strings for the extensions.
				DrawPropertiesExcluding(serializedObject, new string[] { "m_Script", "trash" });

				EditorGUILayout.Space();

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				EditorGUILayout.PropertyField(m_All, new GUIContent("Save All"));
				EditorGUILayout.PropertyField(m_None, new GUIContent("Discard All"));

				EditorGUILayout.EndVertical();

				EditorGUILayout.Space();
				EditorGUILayout.Space();

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			List<TrashFile> list = m_Preferences.Trash;

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Trash", Skin);

			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();

			//Search field
			EditorGUILayout.PropertyField(m_Search, new GUIContent("Search"));

			EditorGUILayout.Space();

			//View folder content/data fields
			m_ShowSubfolders = EditorGUILayout.Toggle(new GUIContent("View Folder Content"), m_ShowSubfolders);
			m_ShowDate = EditorGUILayout.Toggle(new GUIContent("View Date"), m_ShowDate);


			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				RecycleBinFunctions.RefreshSearch(m_Preferences.Search);
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			//Draws files and directories.
			for (int i = 0; i < list.Count; i++)
			{
				if (!FileFunctions.IsDirectory(list[i].path))
				{
					DrawFile(list[i].path, true, true);
				}
				else
				{
					DrawFolderRecursive(new DirectoryInfo(list[i].path), true);
				}

				EditorGUILayout.Space();
			}

			EditorGUILayout.Space();

			//Draws Delete / Restore All 
			DrawButtons();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorPrefs.SetBool("show", m_ShowSubfolders);
			EditorPrefs.SetBool("date", m_ShowDate);

			serializedObject.ApplyModifiedProperties();
		}

        /// <summary>
        /// Draws a file onto the editor.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <param name="box">Draw parameter bounding box.</param>
        /// <param name="button">Draw parameter delete and restore buttons.</param>
        private void DrawFile(string path, bool box, bool button)
		{
			FileInfo info = new FileInfo(path);

			if (!box)
			{
                EditorGUILayout.BeginVertical();
                GUILayout.BeginHorizontal();

                GUILayout.Label(" File", GUILayout.Width(50.0f));
                GUILayout.Label(": " + Path.GetFileName(path), GUILayout.ExpandWidth(true));

				if (button)
				{
					if (GUILayout.Button("Delete"))
					{
						Delete(new FileInfo(path));
					}

					if (GUILayout.Button("Restore"))
					{
						Restore(new FileInfo(path));
					}
				}

                GUILayout.EndHorizontal();

                if (m_ShowDate)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Space(67.0f);
                    GUILayoutOption date = GUILayout.Width(120);
                    GUILayout.Label(RecycleBinFunctions.FormatDate(info.LastAccessTime), date);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
            }
			else
			{
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(" File", GUILayout.Width(50.0f));
                GUILayout.Label(": " + Path.GetFileName(path), GUILayout.ExpandWidth(true));

				GUILayoutOption option = GUILayout.Width(65);

				if (GUILayout.Button(new GUIContent("Delete"), option))
				{
					Delete(new FileInfo(path));
				}
				if (GUILayout.Button(new GUIContent("Restore"), option))
				{
					Restore(new FileInfo(path));
				}

				EditorGUILayout.EndHorizontal();

                if (m_ShowDate)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Space(63.0f);
                    GUILayoutOption date = GUILayout.Width(120);
                    GUILayout.Label(RecycleBinFunctions.FormatDate(info.LastAccessTime), date);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
            }
		}

        /// <summary>
        /// Draws delete, restore and open buttons.
        /// </summary>
		private void DrawButtons()
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Delete All"))
			{
				if (EditorUtility.DisplayDialog("Delete Trash?", "Are you sure you want to complete this action?", "Yes", "No"))
				{
					RecycleBinFunctions.ClearRecycleBinDirectory();
				}
			}

			if (GUILayout.Button("Restore All"))
			{
				if (EditorUtility.DisplayDialog("Restore Trash?", "Are you sure you want to complete this action?", "Yes", "No"))
				{
					RecycleBinFunctions.CopyFilesFromBinToAssetsFolder();
				}
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			if (GUILayout.Button("Open Trash"))
			{
				FileFunctions.OpenFolder(RecycleBinFunctions.GetRecycleBinAndCreateIfNull());
			}

			EditorGUILayout.EndVertical();
		}

        /// <summary>
        /// Draws a folder in the editor and allows a recursive drawing of subfolders.
        /// </summary>
        /// <param name="info">Info regarding the file directory.</param>
        /// <param name="buttons">Draw delete and restore buttons.</param>
		private void DrawFolderRecursive(DirectoryInfo info, bool buttons)
		{
			if (Directory.Exists(info.FullName))
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				EditorGUILayout.BeginHorizontal();

                GUILayout.Label(" Folder", GUILayout.Width(50.0f));
                GUILayout.Label(": " + info.Name, GUILayout.ExpandWidth(true));

				if (buttons)
				{
					GUILayoutOption option = GUILayout.Width(65);

					if (GUILayout.Button(new GUIContent("Delete"), option))
					{
						Delete(info);
					}

					if (GUILayout.Button(new GUIContent("Restore"), option))
					{
						Restore(info);
					}
				}

				EditorGUILayout.EndHorizontal();

                if (m_ShowDate)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Space(63.0f);
                    GUILayoutOption date = GUILayout.Width(120);
                    GUILayout.Label(RecycleBinFunctions.FormatDate(info.LastAccessTime), date);

                    EditorGUILayout.EndHorizontal();
                }

                if (m_ShowSubfolders)
				{
					if (Directory.Exists(info.FullName)) //Doesnt Draw Subfolders if parent was deleted, calling GetFiles or GetDirectories on an invalid folder throws exceptions
					{
						foreach (FileInfo file in info.GetFiles())
						{
							EditorGUI.indentLevel++;

							EditorGUILayout.BeginHorizontal();
							GUILayout.Space(30);
							DrawFile(file.FullName, false, false);
							EditorGUILayout.EndHorizontal();
							EditorGUI.indentLevel--;

							GUILayout.Space(5);
						}

						foreach (DirectoryInfo dir in info.GetDirectories())
						{
							EditorGUILayout.BeginHorizontal();
							GUILayout.Space(30);
							DrawFolderRecursive(dir, false);
							EditorGUILayout.EndHorizontal();

							GUILayout.Space(5);
						}
					}
				}
				EditorGUI.indentLevel--;

				EditorGUILayout.EndVertical();
			}
		}

        /// <summary>
        /// Delete a file using its directory info.
        /// </summary>
        /// <param name="info">File's directory info.</param>
        private void Delete(DirectoryInfo info)
		{
			if (EditorUtility.DisplayDialog("Recycle Bin", "Delete " + info.Name + "?", "Yes", "No"))
			{
				FileUtil.DeleteFileOrDirectory(info.FullName);

				RecycleBinFunctions.RefreshSearch("");
			}
		}

        /// <summary>
        /// Delete a file using its file info.
        /// </summary>
        /// <param name="info">File's info.</param>
        private void Delete(FileInfo info)
		{
			if (EditorUtility.DisplayDialog("Recycle Bin", "Delete " + info.Name + "?", "Yes", "No"))
			{
				FileUtil.DeleteFileOrDirectory(info.FullName);

				RecycleBinFunctions.RefreshSearch("");
			}
		}

        /// <summary>
        /// Restore a file using its directory info.
        /// </summary>
        /// <param name="info">File's directory info.</param>
        private void Restore(DirectoryInfo info)
		{
			if (EditorUtility.DisplayDialog("Recycle Bin", "Restore " + info.Name + "?", "Yes", "No"))
			{
				FileUtil.CopyFileOrDirectory(info.FullName, Path.Combine(Application.dataPath, info.Name));
				FileUtil.DeleteFileOrDirectory(info.FullName);

				AssetDatabase.Refresh();
				RecycleBinFunctions.RefreshSearch("");
			}
		}

        /// <summary>
        /// Restore a file using its file info.
        /// </summary>
        /// <param name="info">File's info.</param>
        private void Restore(FileInfo info)
        {
            if (EditorUtility.DisplayDialog("Recycle Bin", "Restore " + info.Name + "?", "Yes", "No"))
            {
                FileUtil.CopyFileOrDirectory(info.FullName, Path.Combine(Application.dataPath, info.Name));
                FileUtil.DeleteFileOrDirectory(info.FullName);

                AssetDatabase.Refresh();
                RecycleBinFunctions.RefreshSearch("");
            }
        }
    }
}

#endif