///====================================================================================================
///
///     TextureGeneratorBase by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEditor;
using UnityEngine;

public abstract class TextureGeneratorBase<T> : EditorWindow where T : EditorWindow
{
    protected Vector2Int m_ResultSize = new Vector2Int(512, 512);
    protected Dictionary<string, TextureColorContainer> m_ComponentBoxes = new Dictionary<string, TextureColorContainer>();

    private Texture2D m_Result;
    private Rect m_Box;

    protected struct ComponentBoxData
    {
        public enum ComponentBoxType
        {
            TextureColor,
            Texture,
            Color,
            Custom
        }

        public ComponentBoxData(string name, ComponentBoxType type, System.Func<float, TextureColorContainer> boxCreationCallback = null)
        {
            BoxName = name;
            BoxType = type;
            BoxCreationCallback = boxCreationCallback;
        }

        public string BoxName;
        public ComponentBoxType BoxType;
        public System.Func<float, TextureColorContainer> BoxCreationCallback;
    }

    public class TextureColorContainer
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

    protected static void SetDefaultTextureGeneratorWindowData(string tabName)
    {
        EditorWindow mergerWindow = GetWindow<T>(true, tabName);
        mergerWindow.maxSize = new Vector2(300.0f, 400.0f);
        mergerWindow.minSize = mergerWindow.maxSize;
    }

    protected abstract string GetTopName();
    protected abstract string GetHelpTooltipText();
    protected abstract ComponentBoxData[] GetTextureBoxesData();
    protected abstract Color ApplyMath(int x, int y);
    protected virtual float[] GetSaveableValues() { return new float[0]; }
    protected virtual void SetSaveableValues(float[] values) { }

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
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginArea(new Rect(280.0f, 0.0f, 20.0f, 20.0f));
            {
                GUILayout.Box(EditorUtil.IconContent("_Help", GetHelpTooltipText()));
            }
            GUILayout.EndArea();

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
                GUILayout.Space(17.0f);

                int count = GetTextureBoxesData().Length;

                float boxWidth = (300.0f - (10.0f * (count + 1))) / count;
                foreach (ComponentBoxData boxInfo in GetTextureBoxesData())
                {
                    if (!m_ComponentBoxes.ContainsKey(boxInfo.BoxName))
                    {
                        m_ComponentBoxes.Add(boxInfo.BoxName, new TextureColorContainer(false, null, Color.black));
                    }

                    TextureColorContainer container = m_ComponentBoxes[boxInfo.BoxName];

                    GUILayout.BeginVertical(GUILayout.Width(boxWidth), GUILayout.Height(120.0f));
                    {
                        switch (boxInfo.BoxType)
                        {
                            case ComponentBoxData.ComponentBoxType.TextureColor:
                                container.IsColor = GUILayout.Toggle(container.IsColor, "Use Color");
                                container = container.IsColor ? PrepareColorArea(boxInfo.BoxName, boxWidth, container) : PrepareTextureArea(boxInfo.BoxName, boxWidth, container);
                                break;

                            case ComponentBoxData.ComponentBoxType.Texture:
                                GUILayout.Space(18.0f);
                                container = PrepareTextureArea(boxInfo.BoxName, boxWidth, container);
                                
                                break;

                            case ComponentBoxData.ComponentBoxType.Color:
                                GUILayout.Space(18.0f);
                                container = PrepareColorArea(boxInfo.BoxName, boxWidth, container);
                                GUILayout.Space(2.0f);
                                break;

                            case ComponentBoxData.ComponentBoxType.Custom:
                                container = boxInfo.BoxCreationCallback(boxWidth);
                                break;
                        }
                    }
                    GUILayout.EndVertical();

                    m_ComponentBoxes[boxInfo.BoxName] = container;
                }
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
                        m_Box = new Rect(91.0f, 227.0f + ((125.0f - value) / 2.0f), 125.0f, value);
                    }
                    else if (m_ResultSize.x < m_ResultSize.y)
                    {
                        float difference = (float)m_ResultSize.x / m_ResultSize.y;

                        float value = 125.0f * difference;
                        m_Box = new Rect(91.0f + ((125.0f - value) / 2.0f), 227.0f, value, 125.0f);
                    }
                    else
                    {
                        m_Box = new Rect(91.0f, 227.0f, 125.0f, 125.0f);
                    }

                    for (int x = 0; x < m_ResultSize.x; x++)
                    {
                        for (int y = 0; y < m_ResultSize.y; y++)
                        {
                            m_Result.SetPixel(x, y, ApplyMath(x, y));
                        }
                    }

                    m_Result.Apply();
                }

                // SAVE RESULT BUTTON
                if (GUILayout.Button("Save Result", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    if (m_Result != null)
                    {
                        string path = FileBrowserUtil.SaveFilePanel("Save Texture", Application.dataPath, "Texture", ExtensionFilter.GetExtensionFilters("png", "jpg"));

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
                                Debug.LogError("TextureGeneratorBase : Unknown extension provided.");
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

    private TextureColorContainer PrepareTextureArea(string boxName, float boxWidth, TextureColorContainer container)
    {
        GUILayout.BeginHorizontal(GUILayout.Width(boxWidth));
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(boxName);
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(boxWidth));
        {
            GUILayout.FlexibleSpace();
            container.Texture = (Texture2D) EditorGUILayout.ObjectField(container.Texture, typeof(Texture2D), false, GUILayout.Width(Mathf.Min(boxWidth, 70.0f)), GUILayout.Height(Mathf.Min(boxWidth, 70.0f)));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

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

        GUILayout.BeginHorizontal(GUILayout.Width(boxWidth), GUILayout.Height(15.0f));
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("^", GUILayout.Width(15.0f)))
            {
                container.Texture = m_Result;
            }

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(2.0f);

        return container;
    }

    private TextureColorContainer PrepareColorArea(string boxName, float boxWidth, TextureColorContainer container)
    {
        GUILayout.BeginHorizontal(GUILayout.Width(boxWidth));
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(boxName);
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(boxWidth), GUILayout.Height(100.0f));
        {
            GUILayout.FlexibleSpace();
            container.Color = EditorGUILayout.ColorField(container.Color, GUILayout.Width(Mathf.Min(boxWidth, 70.0f)));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        return container;
    }

    private void Awake()
    {
        string type = typeof(T).ToString();

        // Allows data to be kept over multiple sessions.
        m_ResultSize = new Vector2Int(PlayerPrefs.GetInt(type + "_RESULTSIZE_X", 512), PlayerPrefs.GetInt(type + "_RESULTSIZE_Y", 512));

        string[] boxes = PlayerPrefsUtil.GetStringArray(type + "_BOXKEYS", string.Empty);

        foreach(string box in boxes)
        {
            string texturePath = PlayerPrefs.GetString(type + "_TEXPATH_" + box, string.Empty);
            Texture2D texture = null;

            if (texturePath != string.Empty)
            {
                Texture2D pathResult = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

                if (pathResult != null)
                {
                    texture = pathResult;
                }
            }

            TextureColorContainer container = new TextureColorContainer(
                PlayerPrefsUtil.GetBool(type + "_ISCOLOR_" + box, false),
                texture,
                PlayerPrefsUtil.GetColor(type + "_COLOR_" + box, Color.black));

            m_ComponentBoxes.Add(box, container);
        }

        SetSaveableValues(PlayerPrefsUtil.GetFloatArray(type + "_SAVEABLES"));
    }

    private void OnDestroy()
    {
        string type = typeof(T).ToString();

        PlayerPrefs.SetInt(type + "_RESULTSIZE_X", m_ResultSize.x);
        PlayerPrefs.SetInt(type + "_RESULTSIZE_Y", m_ResultSize.y);

        PlayerPrefsUtil.SetStringArray(type + "_BOXKEYS", m_ComponentBoxes.ExtractKeys().ToArray());
        
        foreach (KeyValuePair<string, TextureColorContainer> tcc in m_ComponentBoxes)
        {
            PlayerPrefsUtil.SetBool(type + "_ISCOLOR_" + tcc.Key, tcc.Value.IsColor);
            PlayerPrefs.SetString(type + "_TEXPATH_" + tcc.Key, AssetDatabase.GetAssetPath(tcc.Value.Texture));
            PlayerPrefsUtil.SetColor(type + "_COLOR_" + tcc.Key, tcc.Value.Color);
        }

        PlayerPrefsUtil.SetFloatArray(type + "_SAVEABLES", GetSaveableValues());
    }
}
