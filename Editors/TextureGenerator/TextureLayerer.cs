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
    public class TextureLayerer : EditorWindow
    {
        private Vector2Int m_ResultSize = new Vector2Int(512, 512);

        private Texture2D m_Base;
        private Texture2D m_Overlay;
        private Texture2D m_Result;

        private Rect m_Box = new Rect(90.0f, 185.0f, 125.0f, 125.0f);

        [MenuItem("Tool/Texture Generation/Layerer")]
        public static void ShowWindow()
        {
            EditorWindow mergerWindow = GetWindow<TextureLayerer>();
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
                    GUILayout.Space(50.0f);
                    GUILayout.Label("Grayscale to RGB Texture Merger", GUILayout.ExpandWidth(true));

                    GUILayout.Box(EditorUtil.IconContent("_Help", "Compress your textures using this tool. Instead of loading 3 grayscale textures in memory for your shader, merge them into a single image. Each color of the new image represents one of your old grayscale texture. If the result and your 3 sources are of the same size, there will be no data loss in the conversion. \n\nNote : \n-Each textures must have read/write enabled to be used by this tool. \n-Make sure that the sizes don't differ by much since there is no resizing algorithm at play here. Any size change will probably look like crap. \n-Limit of 4096x4096, and even then, the tool will take a long time to generate a result. Use large sizes at your own risk. \n-To remove a texture, click on the texture's square and press delete. An empty square will simply set that color to 0."));
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

                    // WIDTH - HEIGHT INPUT BOXES
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
                        // RED TEXTURE INPUT BOX
                        GUILayout.BeginHorizontal(GUILayout.Width(70.0f));
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("Base", GUILayout.ExpandWidth(true));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        m_Base = (Texture2D)EditorGUILayout.ObjectField(m_Base, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70), GUILayout.ExpandWidth(true));

                        try
                        {
                            if (m_Base != null)
                            {
                                m_Base.GetPixel(0, 0);
                            }
                        }
                        catch (UnityException e)
                        {
                            if (e.Message.StartsWith("Texture '" + m_Base.name + "' is not readable"))
                            {
                                Debug.LogError("Please enable read/write on texture [" + m_Base.name + "]");
                                m_Base = null;
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical();
                    {
                        // BLUE TEXTURE INPUT BOX
                        GUILayout.BeginHorizontal(GUILayout.Width(70.0f));
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("Overlay", GUILayout.ExpandWidth(true));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        m_Overlay = (Texture2D)EditorGUILayout.ObjectField(m_Overlay, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70), GUILayout.ExpandWidth(true));

                        try
                        {
                            if (m_Overlay != null)
                            {
                                m_Overlay.GetPixel(0, 0);
                            }
                        }
                        catch (UnityException e)
                        {
                            if (e.Message.StartsWith("Texture '" + m_Overlay.name + "' is not readable"))
                            {
                                Debug.LogError("Please enable read/write on texture [" + m_Overlay.name + "]");
                                m_Overlay = null;
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

                    // RESULT DISPLAY BOX
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

                    // GENERATE RESULT BUTTON
                    if (GUILayout.Button("Generate Result", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    {
                        if (m_ResultSize.x > 4096)
                        {
                            m_ResultSize.x = 4096;
                            Debug.LogWarning("TextureMerger : Result width or height cannot go over 4096. Capping the value.");
                        }

                        if (m_ResultSize.y > 4096)
                        {
                            m_ResultSize.y = 4096;
                            Debug.LogWarning("TextureMerger : Result width or height cannot go over 4096. Capping the value.");
                        }

                        m_Result = new Texture2D(m_ResultSize.x, m_ResultSize.y);

                        if (m_ResultSize.x > m_ResultSize.y)
                        {
                            float difference = (float)m_ResultSize.y / m_ResultSize.x;

                            float value = 125.0f * difference;
                            m_Box = new Rect(90.0f, 189.0f + ((125.0f - value) / 2.0f), 125.0f, value);
                        }
                        else if (m_ResultSize.x < m_ResultSize.y)
                        {
                            float difference = (float)m_ResultSize.x / m_ResultSize.y;

                            float value = 125.0f * difference;
                            m_Box = new Rect(90.0f + ((125.0f - value) / 2.0f), 189.0f, value, 125.0f);
                        }
                        else
                        {
                            m_Box = new Rect(90.0f, 189.0f, 125.0f, 125.0f);
                        }

                        for (int x = 0; x < m_ResultSize.x; x++)
                        {
                            for (int y = 0; y < m_ResultSize.y; y++)
                            {
                                Color result = new Color(0.0f, 0.0f, 0.0f, 1.0f);

                                if (m_Base != null)
                                {
                                    int pixelX = (int)(((float)m_Base.width / m_ResultSize.x) * x);
                                    int pixelY = (int)(((float)m_Base.height / m_ResultSize.y) * y);

                                    Color pixel = m_Base.GetPixel(pixelX, pixelY);

                                    result = pixel;
                                }

                                if (m_Overlay != null)
                                {
                                    int pixelX = (int)(((float)m_Overlay.width / m_ResultSize.x) * x);
                                    int pixelY = (int)(((float)m_Overlay.height / m_ResultSize.y) * y);

                                    Color pixel = m_Overlay.GetPixel(pixelX, pixelY);

                                    if (pixel.a > 0.0f)
                                    {
                                        result = pixel;
                                    }
                                }

                                m_Result.SetPixel(x, y, result);
                            }
                        }

                        m_Result.Apply();
                    }

                    // SAVE RESULT BUTTON
                    if (GUILayout.Button("Save Result", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    {
                        if (m_Result != null)
                        {
                            string path = FileBrowserUtil.SaveFilePanel("Save RGB Texture", Application.dataPath, "RGBTexture", ExtensionFilter.GetExtensionFilters("png", "jpg"));

                            if (path.Length > 0)
                            {
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

                                if (EditorUtil.IsAbsolutePathARelativePath(path))
                                {
                                    string relativePath = EditorUtil.AbsoluteToRelativePath(path);
                                    AssetDatabase.ImportAsset(relativePath);
                                    AssetDatabase.Refresh();
                                    EditorApplication.ExecuteMenuItem("Window/Project");
                                    Selection.activeObject = AssetDatabase.LoadAssetAtPath(relativePath, typeof(object));
                                }
                            }
                        }
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void Awake()
        {
            m_ResultSize = new Vector2Int(PlayerPrefs.GetInt("TEXTURELAYERER_RESULTSIZE_X", 512), PlayerPrefs.GetInt("TEXTURELAYERER_RESULTSIZE_Y", 512));

            string basePath = PlayerPrefs.GetString("TEXTURELAYERER_BASETEXPATH", string.Empty);
            string overlayPath = PlayerPrefs.GetString("TEXTURELAYERER_OVERLAYTEXPATH", string.Empty);

            if (basePath != string.Empty)
            {
                m_Base = AssetDatabase.LoadAssetAtPath<Texture2D>(basePath);
            }

            if (overlayPath != string.Empty)
            {
                m_Overlay = AssetDatabase.LoadAssetAtPath<Texture2D>(overlayPath);
            }
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt("TEXTURELAYERER_RESULTSIZE_X", m_ResultSize.x);
            PlayerPrefs.SetInt("TEXTURELAYERER_RESULTSIZE_Y", m_ResultSize.y);

            PlayerPrefs.SetString("TEXTURELAYERER_BASETEXPATH", AssetDatabase.GetAssetPath(m_Base));
            PlayerPrefs.SetString("TEXTURELAYERER_OVERLAYTEXPATH", AssetDatabase.GetAssetPath(m_Overlay));
        }
    }
}