using System;
using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEditor;
using UnityEngine;

public abstract class TextureGeneratorBase<T> : EditorWindow where T : EditorWindow
{
    private Vector2Int m_ResultSize = new Vector2Int(512, 512);

    private Dictionary<string, TextureColorContainer> m_ImageBoxes = new Dictionary<string, TextureColorContainer>();
    private Texture2D m_Result;

    private Rect m_Box;

    private struct TextureColorContainer
    {
        public TextureColorContainer(bool isColor, Texture2D texture, Color color)
        {
            IsColor = isColor;
            Texture = texture;
            Color = color;
        }

        public bool IsColor;
        public Texture2D Texture;
        public Color Color;
    }

    protected static void SetDefaultTextureGeneratorWindowData()
    {
        EditorWindow mergerWindow = GetWindow<T>();
        mergerWindow.maxSize = new Vector2(300.0f, 360.0f);
        mergerWindow.minSize = mergerWindow.maxSize;
    }

    protected abstract string GetTopName();
    protected abstract string GetHelpTooltipText();
    protected abstract string[] GetTextureBoxesNames();
    protected abstract Texture2D ApplyMathToTexture();

    private void OnGUI()
    {
        // Very sorry about this mess. I did a lot of CSS in my past...
        GUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal(GUILayout.Width(300.0f));
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(GetTopName(), GUIUtil.CenterStyle);
                GUILayout.FlexibleSpace();

                GUILayout.Box(EditorUtil.IconContent("_Help", GetHelpTooltipText()));
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

                foreach (string boxName in GetTextureBoxesNames())
                {
                    if (!m_ImageBoxes.ContainsKey(boxName))
                    {
                        m_ImageBoxes.Add(boxName, new TextureColorContainer(false, null, Color.black));
                    }

                    TextureColorContainer container = m_ImageBoxes[boxName];

                    GUILayout.BeginVertical();
                    {
                        // TEXTURE INPUT BOX
                        GUILayout.BeginHorizontal(GUILayout.Width(70.0f));
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(boxName, GUILayout.ExpandWidth(true));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        container.Texture = (Texture2D)EditorGUILayout.ObjectField(container.Texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70), GUILayout.ExpandWidth(true));

                        try
                        {
                            if (container.Texture != null)
                            {
                                container.Texture.GetPixel(0, 0);
                            }
                        }
                        catch (UnityException e)
                        {
                            if (e.Message.StartsWith("Texture '" + container.Texture.name + "' is not readable"))
                            {
                                Debug.LogError("Please enable read/write on texture [" + container.Texture.name + "]");
                                container.Texture = null;
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();

                    m_ImageBoxes[boxName] = container;
                }
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

                            //if (m_Red != null)
                            //{
                            //    int pixelX = (int)(((float)m_Red.width / m_ResultSize.x) * x);
                            //    int pixelY = (int)(((float)m_Red.height / m_ResultSize.y) * y);

                            //    Color pixel = m_Red.GetPixel(pixelX, pixelY);

                            //    result.r = pixel.r * pixel.a;
                            //}

                            //if (m_Green != null)
                            //{
                            //    int pixelX = (int)(((float)m_Green.width / m_ResultSize.x) * x);
                            //    int pixelY = (int)(((float)m_Green.height / m_ResultSize.y) * y);

                            //    Color pixel = m_Green.GetPixel(pixelX, pixelY);

                            //    result.g = pixel.g * pixel.a;
                            //}

                            //if (m_Blue != null)
                            //{
                            //    int pixelX = (int)(((float)m_Blue.width / m_ResultSize.x) * x);
                            //    int pixelY = (int)(((float)m_Blue.height / m_ResultSize.y) * y);

                            //    Color pixel = m_Blue.GetPixel(pixelX, pixelY);

                            //    result.b = pixel.b * pixel.a;
                            //}

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
        string type = typeof(T).ToString();

        PlayerPrefs.SetInt(type + "BOXCOUNT", m_ImageBoxes.Count);

        int boxIndex = 0;
        foreach (KeyValuePair<string, TextureColorContainer> tcc in m_ImageBoxes)
        {
            PlayerPrefs.SetString(type + "_BOXNAME_" + boxIndex, tcc.Key);
            PlayerPrefsUtil.SetBool(type + "_ISCOLOR_" + boxIndex, tcc.Value.IsColor);
            PlayerPrefs.SetString(type + "_TEXPATH_" + boxIndex, AssetDatabase.GetAssetPath(tcc.Value.Texture));
            PlayerPrefsUtil.SetColor(type + "_COLOR_" + boxIndex, tcc.Value.Color);

            boxIndex++;
        }


        // Allows data to be kept over multiple sessions.
        m_ResultSize = new Vector2Int(PlayerPrefs.GetInt(type + "_RESULTSIZE_X", 512), PlayerPrefs.GetInt(type + "_RESULTSIZE_Y", 512));

        for (int i = 0; i < PlayerPrefs.GetInt(type + "BOXCOUNT"); i++)
        {

        }

        string redPath = PlayerPrefs.GetString("TEXTUREMERGER_REDTEXPATH", string.Empty);
        string greenPath = PlayerPrefs.GetString("TEXTUREMERGER_GREENTEXPATH", string.Empty);
        string bluePath = PlayerPrefs.GetString("TEXTUREMERGER_BLUETEXPATH", string.Empty);

        if (redPath != string.Empty)
        {
            Texture2D red = AssetDatabase.LoadAssetAtPath<Texture2D>(redPath);

            if (red != null)
            {
                m_Red = red;
            }
        }

        if (greenPath != string.Empty)
        {
            Texture2D green = AssetDatabase.LoadAssetAtPath<Texture2D>(greenPath);

            if (green != null)
            {
                m_Green = green;
            }
        }

        if (bluePath != string.Empty)
        {
            Texture2D blue = AssetDatabase.LoadAssetAtPath<Texture2D>(bluePath);

            if (blue != null)
            {
                m_Blue = blue;
            }
        }
    }

    private void OnDestroy()
    {
        string type = typeof(T).ToString();

        PlayerPrefs.SetInt(type + "_RESULTSIZE_X", m_ResultSize.x);
        PlayerPrefs.SetInt(type + "_RESULTSIZE_Y", m_ResultSize.y);

        PlayerPrefs.SetInt(type + "BOXCOUNT", m_ImageBoxes.Count);

        int boxIndex = 0;
        foreach (KeyValuePair<string, TextureColorContainer> tcc in m_ImageBoxes)
        {
            PlayerPrefs.SetString(type + "_BOXNAME_" + boxIndex, tcc.Key);
            PlayerPrefsUtil.SetBool(type + "_ISCOLOR_" + boxIndex, tcc.Value.IsColor);
            PlayerPrefs.SetString(type + "_TEXPATH_" + boxIndex, AssetDatabase.GetAssetPath(tcc.Value.Texture));
            PlayerPrefsUtil.SetColor(type + "_COLOR_" + boxIndex, tcc.Value.Color);

            boxIndex++;
        }
    }
}
