///====================================================================================================
///
///     TextureMerger by
///     - CantyCanadian
///
///====================================================================================================

using UnityEditor;
using UnityEngine;
using Canty.Managers;

namespace Canty.Editors
{
    public class TextureGenerator : EditorWindow
    {
        private Vector2Int m_ResultSize = new Vector2Int(512, 512);

        private Texture2D m_Red;
        private Texture2D m_Green;
        private Texture2D m_Blue;
        private Texture2D m_Result;

        private Rect m_Box = new Rect(90.0f, 185.0f, 125.0f, 125.0f);

        [MenuItem("Tool/Texture Generator")]
        public static void ShowWindow()
        {
            EditorWindow mergerWindow = GetWindow<TextureGenerator>();
            mergerWindow.maxSize = new Vector2(300.0f, 360.0f);
            mergerWindow.minSize = mergerWindow.maxSize;
        }

        private void OnGUI()
        {
            // Very sorry about this mess. I did a lot of CSS in my past...
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal(GUILayout.Width(300.0f));
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Result Size", GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Width(300.0f));
                {
                    GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical(GUILayout.Width(100.0f));
                    {
                        m_ResultSize.x = EditorGUILayout.IntField(m_ResultSize.x);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(100.0f));
                    {
                        m_ResultSize.y = EditorGUILayout.IntField(m_ResultSize.y);
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(10.0f));
                {
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Width(300.0f), GUILayout.Height(125.0f));
                {
                    GUILayout.FlexibleSpace();

                    GUILayout.Box(" ", GUILayout.Width(200.0f), GUILayout.Height(200.0f));

                    if (m_Result != null)
                    {
                        GUI.DrawTexture(m_Box, m_Result);
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(4.0f));
                {
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void Awake()
        {
            m_ResultSize = new Vector2Int(PlayerPrefs.GetInt("TEXTUREMERGER_RESULTSIZE_X", 512), PlayerPrefs.GetInt("TEXTUREMERGER_RESULTSIZE_Y", 512));

            string redPath = PlayerPrefs.GetString("TEXTUREMERGER_REDTEXPATH", string.Empty);
            string greenPath = PlayerPrefs.GetString("TEXTUREMERGER_GREENTEXPATH", string.Empty);
            string bluePath = PlayerPrefs.GetString("TEXTUREMERGER_BLUETEXPATH", string.Empty);

            if (redPath != string.Empty)
            {
                m_Red = AssetDatabase.LoadAssetAtPath<Texture2D>(redPath);
            }

            if (greenPath != string.Empty)
            {
                m_Green = AssetDatabase.LoadAssetAtPath<Texture2D>(greenPath);
            }

            if (bluePath != string.Empty)
            {
                m_Blue = AssetDatabase.LoadAssetAtPath<Texture2D>(bluePath);
            }
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt("TEXTUREMERGER_RESULTSIZE_X", m_ResultSize.x);
            PlayerPrefs.SetInt("TEXTUREMERGER_RESULTSIZE_Y", m_ResultSize.y);

            PlayerPrefs.SetString("TEXTUREMERGER_REDTEXPATH", AssetDatabase.GetAssetPath(m_Red));
            PlayerPrefs.SetString("TEXTUREMERGER_GREENTEXPATH", AssetDatabase.GetAssetPath(m_Green));
            PlayerPrefs.SetString("TEXTUREMERGER_BLUETEXPATH", AssetDatabase.GetAssetPath(m_Blue));
        }
    }
}