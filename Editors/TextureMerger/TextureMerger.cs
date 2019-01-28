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
    public class TextureMerger : EditorWindow
    {
        private Vector2Int m_ResultSize = new Vector2Int(512, 512);

        private Texture2D m_Red;
        private Texture2D m_Green;
        private Texture2D m_Blue;
        private Texture2D m_Result;

        private Rect m_Box = new Rect(90.0f, 185.0f, 125.0f, 125.0f);

        [MenuItem("Tool/Texture Merger")]
        public static void ShowWindow()
        {
            EditorWindow mergerWindow = GetWindow<TextureMerger>();
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
                    GUILayout.Label("Grayscale to RGB Texture Merger", GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(10.0f));
                {
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Width(300.0f));
                {
                    GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical(GUILayout.Width(100.0f));
                    {
                        GUILayout.Label("Result Width", GUILayout.ExpandWidth(true));
                        m_ResultSize.x = EditorGUILayout.IntField(m_ResultSize.x);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(100.0f));
                    {
                        GUILayout.Label("Result Height", GUILayout.ExpandWidth(true));
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

                GUILayout.BeginHorizontal(GUILayout.Width(300.0f));
                {
                    GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal(GUILayout.Width(70.0f));
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("Red", GUILayout.ExpandWidth(true));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        m_Red = (Texture2D)EditorGUILayout.ObjectField(m_Red, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70), GUILayout.ExpandWidth(true));

                        try
                        {
                            if (m_Red != null)
                            {
                                m_Red.GetPixel(0, 0);
                            }
                        }
                        catch (UnityException e)
                        {
                            if (e.Message.StartsWith("Texture '" + m_Red.name + "' is not readable"))
                            {
                                Debug.LogError("Please enable read/write on texture [" + m_Red.name + "]");
                                m_Red = null;
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal(GUILayout.Width(70.0f));
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("Green", GUILayout.ExpandWidth(true));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        m_Green = (Texture2D)EditorGUILayout.ObjectField(m_Green, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70), GUILayout.ExpandWidth(true));

                        try
                        {
                            if (m_Green != null)
                            {
                                m_Green.GetPixel(0, 0);
                            }
                        }
                        catch (UnityException e)
                        {
                            if (e.Message.StartsWith("Texture '" + m_Green.name + "' is not readable"))
                            {
                                Debug.LogError("Please enable read/write on texture [" + m_Green.name + "]");
                                m_Green = null;
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal(GUILayout.Width(70.0f));
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("Blue", GUILayout.ExpandWidth(true));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        m_Blue = (Texture2D)EditorGUILayout.ObjectField(m_Blue, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70), GUILayout.ExpandWidth(true));

                        try
                        {
                            if (m_Blue != null)
                            {
                                m_Blue.GetPixel(0, 0);
                            }
                        }
                        catch (UnityException e)
                        {
                            if (e.Message.StartsWith("Texture '" + m_Blue.name + "' is not readable"))
                            {
                                Debug.LogError("Please enable read/write on texture [" + m_Blue.name + "]");
                                m_Blue = null;
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(5.0f));
                {
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Width(300.0f), GUILayout.Height(125.0f));
                {
                    GUILayout.Space(86.0f);

                    GUILayout.Box(" ", GUILayout.Width(135.0f), GUILayout.Height(135.0f));

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

                GUILayout.BeginHorizontal(GUILayout.Width(300.0f), GUILayout.Height(20.0f), GUILayout.ExpandWidth(true));
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Generate Result", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    {
                        m_Result = new Texture2D(m_ResultSize.x, m_ResultSize.y);

                        if (m_ResultSize.x > m_ResultSize.y)
                        {
                            float difference = (float)m_ResultSize.y / m_ResultSize.x;

                            float value = 125.0f * difference;
                            m_Box = new Rect(90.0f, 185.0f + ((125.0f - value) / 2.0f), 125.0f, value);
                        }
                        else
                        {
                            float difference = (float)m_ResultSize.x / m_ResultSize.y;

                            float value = 125.0f * difference;
                            m_Box = new Rect(90.0f + ((125.0f - value) / 2.0f), 185.0f, value, 125.0f);
                        }

                        for (int x = 0; x < m_ResultSize.x; x++)
                        {
                            for (int y = 0; y < m_ResultSize.y; y++)
                            {
                                Color result = new Color(0.0f, 0.0f, 0.0f, 1.0f);

                                if (m_Red != null)
                                {
                                    int pixelX = (int)(((float)m_Red.width / m_ResultSize.x) * x);
                                    int pixelY = (int)(((float)m_Red.height / m_ResultSize.y) * y);

                                    Color pixel = m_Red.GetPixel(pixelX, pixelY);

                                    result.r = pixel.r * pixel.a;
                                }

                                if (m_Green != null)
                                {
                                    int pixelX = (int)(((float)m_Green.width / m_ResultSize.x) * x);
                                    int pixelY = (int)(((float)m_Green.height / m_ResultSize.y) * y);

                                    Color pixel = m_Green.GetPixel(pixelX, pixelY);

                                    result.g = pixel.g * pixel.a;
                                }

                                if (m_Blue != null)
                                {
                                    int pixelX = (int)(((float)m_Blue.width / m_ResultSize.x) * x);
                                    int pixelY = (int)(((float)m_Blue.height / m_ResultSize.y) * y);

                                    Color pixel = m_Blue.GetPixel(pixelX, pixelY);

                                    result.b = pixel.b * pixel.a;
                                }

                                m_Result.SetPixel(x, y, result);
                            }
                        }

                        m_Result.Apply();
                    }

                    if (GUILayout.Button("Save Result", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    {
                        string path = FileBrowserManager.SaveFilePanel("Save RGB Texture", Application.dataPath, "RGBTexture", ExtensionFilter.GetImageFileFilter());

                        byte[] file;

                        if (path.EndsWith("png"))
                        {
                            file = m_Result.EncodeToPNG();
                        }
                        else if (path.EndsWith("jpg"))
                        {
                            file = m_Result.EncodeToJPG();
                        }
                        else
                        {
                            Debug.LogError("TextureMerger : Unknown extension provided.");
                            return;
                        }

                        System.IO.File.WriteAllBytes(path, file);
                    }

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