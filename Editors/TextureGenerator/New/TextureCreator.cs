using System;
using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEditor;
using UnityEngine;

public class TextureCreator : EditorWindow
{
    public static GUIStyle CenterLabel { get; private set; } = null;
    public static GUIStyle BorderlessButton { get; private set; } = null;

    private bool m_WindowLocked = false;

    private Vector2 m_ImageTabScroll = Vector2.zero;
    private Vector2 m_ImageTreeScroll = Vector2.zero;
    private Vector2 m_ImageDataScroll = Vector2.zero;
    private int m_CurrentTab = 0;
    private int m_CurrentNode = 0;
    private List<ImageTabContainer> m_ImageTabs;

    private Texture2D m_RightTextureBuffer = null;

    private Color m_DefaultBackgroundColor;

    private class ImageTabContainer
    {
        public List<TextureCreatorComponentContainerBase> Components;

        public string ImageTabName { get; private set; }

        public void Rename(string newName)
        {
            ImageTabName = newName;
        }

        public ImageTabContainer(string name)
        {
            ImageTabName = name;
            Components = new List<TextureCreatorComponentContainerBase>();
        }
    }

    [MenuItem("Tool/Texture Creator", false, 1)]
    public static void ShowWindow()
    {
        EditorWindow textureWindow = GetWindow<TextureCreator>(true, "Texture Creator", true);
        textureWindow.maxSize = new Vector2(400.0f, 400.0f);
        textureWindow.minSize = textureWindow.maxSize;
    }

    private void OnRenameDone(string newName)
    {
        if (newName != string.Empty || newName != "")
        {
            m_ImageTabs[m_CurrentTab].Rename(newName);
        }

        m_WindowLocked = false;
    }

    private void OnInputTypeSelected(TextureCreatorComponentContainerInputTypes? newType)
    {
        if (newType != null)
        {
            switch (newType.Value)
            {
                case TextureCreatorComponentContainerInputTypes.Texture:
                {
                    m_ImageTabs[m_CurrentTab].Components.Add(new TextureCreatorComponentContainerInputTexture());
                }
                break;
            }
        }

        m_WindowLocked = false;
    }

    private void OnModifierTypeSelected(TextureCreatorComponentContainerModifierTypes? newType)
    {
        if (newType != null)
        {
            switch (newType.Value)
            {
                case TextureCreatorComponentContainerModifierTypes.Offset:
                {
                    m_ImageTabs[m_CurrentTab].Components.Add(new TextureCreatorComponentContainerModifierOffset());
                }
                    break;
            }
        }

        m_WindowLocked = false;
    }

    private void OnGUI()
    {
        if (BorderlessButton == null)
        {
            BorderlessButton = GUI.skin.button;
            BorderlessButton.padding = new RectOffset(0, 0, 0, 0);
        }

        if (CenterLabel == null)
        {
            CenterLabel = GUIUtil.CenterStyle;
            CenterLabel.contentOffset = new Vector2(0.0f, -2.0f);
        }

        m_DefaultBackgroundColor = GUI.backgroundColor;

        EditorGUI.BeginDisabledGroup(m_WindowLocked);

        GUILayout.Space(5.0f);

        // Image Tabs

        m_ImageTabScroll = GUIUtil.BeginUpperHorizontalScrollView(new Rect(0.0f, 0.0f, 400.0f, 40.0f), m_ImageTabScroll, new Rect(0.0f, 0.0f, m_ImageTabs.Count * 130.0f + 32.0f, 30.0f));
        GUILayout.BeginHorizontal(GUILayout.Height(20.0f), GUILayout.ExpandWidth(true));
        {
            GUILayout.Space(5.0f);

            List<string> names = new List<string>();
            foreach(ImageTabContainer tab in m_ImageTabs)
            {
                names.Add(tab.ImageTabName);
            }

            // Fun Unity Fact : GUI draw order is as the code suggest, line by line. GUI input order, however, is reversed.
            // To fix this... 'feature' and have two buttons overlapping, we must draw the foreground button twice.
            // First to force the input into working with it, second to draw its image above the background button. Fun.
            if (m_CurrentTab != 0)
            {
                if (GUI.Button(new Rect(m_CurrentTab * 130.0f + 7.5f, 8.0f, 15.0f, 15.0f), EditorGUIUtility.IconContent("CollabDeleted Icon"), BorderlessButton))
                {
                    m_ImageTabs.RemoveAt(m_CurrentTab);
                    m_CurrentTab--;
                }

                if (GUI.Button(new Rect(m_CurrentTab * 130.0f + 117.5f, 8.0f, 15.0f, 15.0f), EditorGUIUtility.IconContent("CollabEdit Icon"), BorderlessButton))
                {
                    EditorWindow popupWindow = GetWindow<TextureCreatorRenamePopup>(true, "Rename", true);
                    popupWindow.position = new Rect(100.0f, 100.0f, 150.0f, 75.0f);
                    ((TextureCreatorRenamePopup)popupWindow).Initialize(OnRenameDone, m_ImageTabs[m_CurrentTab].ImageTabName);
                    m_WindowLocked = true;
                    popupWindow.ShowPopup();
                }
            }

            m_CurrentTab = GUILayout.Toolbar(m_CurrentTab, names.ToArray(), BorderlessButton, GUI.ToolbarButtonSize.Fixed, GUILayout.Width(m_ImageTabs.Count * 130.0f), GUILayout.Height(20.0f));

            if (m_CurrentTab != 0)
            {
                if (GUI.Button(new Rect(m_CurrentTab * 130.0f + 7.5f, 8.0f, 15.0f, 15.0f), EditorGUIUtility.IconContent("CollabDeleted Icon"), BorderlessButton))
                {
                    m_ImageTabs.RemoveAt(m_CurrentTab);
                    m_CurrentTab--;
                }

                if (GUI.Button(new Rect(m_CurrentTab * 130.0f + 117.5f, 8.0f, 15.0f, 15.0f), EditorGUIUtility.IconContent("CollabEdit Icon"), BorderlessButton))
                {
                    EditorWindow popupWindow = GetWindow<TextureCreatorRenamePopup>(true, "Rename", true);
                    popupWindow.position = new Rect(100.0f, 100.0f, 150.0f, 75.0f);
                    popupWindow.minSize = new Vector2(150.0f, 100.0f);
                    popupWindow.maxSize = popupWindow.minSize;
                    ((TextureCreatorRenamePopup)popupWindow).Initialize(OnRenameDone, m_ImageTabs[m_CurrentTab].ImageTabName);
                    m_WindowLocked = true;
                    popupWindow.ShowPopup();
                }
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("CollabCreate Icon"), BorderlessButton, GUILayout.Width(20.0f), GUILayout.Height(20.0f)))
            {
                m_ImageTabs.Add(new ImageTabContainer("New Image Tab"));
            }
        }
        GUILayout.EndHorizontal();
        GUI.EndScrollView();

        GUILayout.Space(13.0f);
        GUIUtil.DrawSeparatorLine(Color.gray, 2, 4);

        GUILayout.BeginHorizontal();
        {
            GUIUtil.BeginLeftSideVerticalScrollView(new Rect(0.0f, 48.0f, 150.0f, 350.0f), m_ImageTreeScroll, new Rect(0.0f, 48.0f, 137.0f, 350.0f));
            {
                GUILayout.BeginVertical(GUILayout.Width(150.0f));
                {
                    if (m_ImageTabs[m_CurrentTab].Components.Count == 0)
                    {
                        GUI.backgroundColor = new Color(1.0f, 0.3f, 0.3f, 1.0f);
                        if (GUILayout.Button("+", BorderlessButton, GUILayout.Width(135.0f), GUILayout.Height(20.0f)))
                        {
                            EditorWindow popupWindow = GetWindow<TextureCreatorInputPopup>(true, "Input Node", true);
                            popupWindow.position = new Rect(100.0f, 100.0f, 150.0f, 75.0f);
                            popupWindow.minSize = new Vector2(200.0f, 100.0f);
                            popupWindow.maxSize = popupWindow.minSize;
                            ((TextureCreatorInputPopup)popupWindow).Initialize(OnInputTypeSelected);
                            m_WindowLocked = true;
                            popupWindow.ShowPopup();
                        }
                        GUI.backgroundColor = m_DefaultBackgroundColor;
                    }
                    else
                    {
                        for(int i = 0; i < m_ImageTabs[m_CurrentTab].Components.Count; ++i)
                        {
                            if (m_CurrentNode == i)
                            {
                                GUI.color = Color.gray;
                            }

                            switch (m_ImageTabs[m_CurrentTab].Components[i].ContainerType)
                            {
                                case TextureCreatorComponentContainerTypes.Input:
                                {
                                    GUI.backgroundColor = new Color(1.0f, 0.3f, 0.3f, 1.0f);
                                    if (GUILayout.Button(m_ImageTabs[m_CurrentTab].Components[i].ContainerName, BorderlessButton, GUILayout.Width(133.0f), GUILayout.Height(20.0f)))
                                    {
                                        m_CurrentNode = i;
                                        m_RightTextureBuffer = null;
                                    }
                                    GUI.backgroundColor = m_DefaultBackgroundColor;
                                }
                                break;

                                case TextureCreatorComponentContainerTypes.Modifier:
                                {
                                    if (GUILayout.Button(m_ImageTabs[m_CurrentTab].Components[i].ContainerName, BorderlessButton, GUILayout.Width(133.0f), GUILayout.Height(20.0f)))
                                    {
                                        m_CurrentNode = i;
                                        m_RightTextureBuffer = null;
                                        }
                                }
                                break;

                                case TextureCreatorComponentContainerTypes.ModifierNeedingInput:
                                {
                                    TextureCreatorComponentContainerPairBase pair = m_ImageTabs[m_CurrentTab].Components[i] as TextureCreatorComponentContainerPairBase;

                                    if (pair != null)
                                    {
                                        if (GUILayout.Button(pair.ContainerName, BorderlessButton, GUILayout.Width(65.0f), GUILayout.Height(20.0f)))
                                        {
                                            m_CurrentNode = i;
                                            m_RightTextureBuffer = null;
                                        }
                                        GUI.backgroundColor = new Color(1.0f, 0.3f, 0.3f, 1.0f);
                                        GUILayout.Button(pair.PairedInput.ContainerName, BorderlessButton, GUILayout.Width(65.0f), GUILayout.Height(20.0f));
                                        GUI.backgroundColor = m_DefaultBackgroundColor;
                                    }
                                }
                                break;
                            }

                            if (m_CurrentNode == i)
                            {
                                GUI.color = m_DefaultBackgroundColor;
                            }
                        }

                        if (GUILayout.Button("+", BorderlessButton, GUILayout.Width(133.0f), GUILayout.Height(20.0f)))
                        {
                            EditorWindow popupWindow = GetWindow<TextureCreatorModifierPopup>(true, "Input Node", true);
                            popupWindow.position = new Rect(100.0f, 100.0f, 150.0f, 75.0f);
                            popupWindow.minSize = new Vector2(200.0f, 100.0f);
                            popupWindow.maxSize = popupWindow.minSize;
                            ((TextureCreatorModifierPopup)popupWindow).Initialize(OnModifierTypeSelected);
                            m_WindowLocked = true;
                            popupWindow.ShowPopup();
                        }
                    }
                }
                GUILayout.EndVertical();
            }
            GUI.EndScrollView();

            GUILayout.BeginVertical(GUILayout.Width(250.0f));
            {
                if (m_ImageTabs[m_CurrentTab].Components.Count > 0)
                {
                    TextureCreatorComponentContainerBase component = m_ImageTabs[m_CurrentTab].Components[m_CurrentNode];
                    GUILayout.Label(component.ContainerName, CenterLabel, GUILayout.Width(230.0f), GUILayout.Height(20.0f));

                    bool dirty = false;
                    if (m_RightTextureBuffer == null)
                    {
                        dirty = true;
                    }
                    else
                    {
                        for (int i = 0; i <= m_CurrentNode; i++)
                        {
                            dirty = dirty || m_ImageTabs[m_CurrentTab].Components[i].IsDirty;
                        }
                    }

                    if (dirty)
                    {
                        for (int i = 0; i <= m_CurrentNode; i++)
                        {
                            m_RightTextureBuffer = m_ImageTabs[m_CurrentTab].Components[i].Invoke(m_RightTextureBuffer);
                        }
                    }

                    if (m_RightTextureBuffer != null)
                    {
                        bool largestWidth = m_RightTextureBuffer.width > m_RightTextureBuffer.height;
                        float largestCoeff = largestWidth ? (float)m_RightTextureBuffer.height / (float)m_RightTextureBuffer.width : (float)m_RightTextureBuffer.width / (float)m_RightTextureBuffer.height;

                        float smallestValue = 150.0f * largestCoeff;
                        EditorGUI.DrawPreviewTexture(new Rect(200.0f, 237.0f, largestWidth ? 150.0f : smallestValue, largestWidth ? smallestValue : 150.0f), m_RightTextureBuffer);
                    }

                    m_ImageDataScroll = GUILayout.BeginScrollView(m_ImageDataScroll, false, true, GUILayout.Width(240.0f), GUILayout.Height(150.0f));
                    {
                        component.OnGUI(218.0f);
                    }
                    GUILayout.EndScrollView();

                    GUIUtil.DrawSeparatorLine(Color.gray, 2, 4);
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
    }

    private void Awake()
    {
        m_ImageTabs = new List<ImageTabContainer>();
        m_ImageTabs.Add(new ImageTabContainer("Main"));
    }
}

public class TextureCreatorRenamePopup : EditorWindow
{
    private Action<string> m_RenameCallback;
    private string m_Name;

    public void Initialize(Action<string> callback, string originalName)
    {
        m_RenameCallback = callback;
        m_Name = originalName;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            GUILayout.FlexibleSpace();

            GUILayout.Label("Renaming tab to :");
            m_Name = EditorGUILayout.TextField(m_Name);

            if (GUILayout.Button("Rename"))
            {
                m_RenameCallback.Invoke(m_Name);
                Close();
            }

            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnDestroy()
    {
        m_RenameCallback.Invoke(string.Empty);
    }
}

public class TextureCreatorInputPopup : EditorWindow
{
    private Action<TextureCreatorComponentContainerInputTypes?> m_InputCallback;
    private TextureCreatorComponentContainerInputTypes m_Type;

    public void Initialize(Action<TextureCreatorComponentContainerInputTypes?> callback)
    {
        m_InputCallback = callback;
        m_Type = TextureCreatorComponentContainerInputTypes.Texture;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            GUILayout.FlexibleSpace();

            GUILayout.Label("Creating input node of type :");
            m_Type = (TextureCreatorComponentContainerInputTypes)EditorGUILayout.EnumPopup(m_Type);

            if (GUILayout.Button("Create"))
            {
                m_InputCallback.Invoke(m_Type);
                Close();
            }

            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnDestroy()
    {
        m_InputCallback.Invoke(null);
    }
}

public class TextureCreatorModifierPopup : EditorWindow
{
    private Action<TextureCreatorComponentContainerModifierTypes?> m_ModifierCallback;
    private TextureCreatorComponentContainerModifierTypes m_Type;

    public void Initialize(Action<TextureCreatorComponentContainerModifierTypes?> callback)
    {
        m_ModifierCallback = callback;
        m_Type = TextureCreatorComponentContainerModifierTypes.Offset;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            GUILayout.FlexibleSpace();

            GUILayout.Label("Creating input node of type :");
            m_Type = (TextureCreatorComponentContainerModifierTypes)EditorGUILayout.EnumPopup(m_Type);

            if (GUILayout.Button("Create"))
            {
                m_ModifierCallback.Invoke(m_Type);
                Close();
            }

            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnDestroy()
    {
        m_ModifierCallback.Invoke(null);
    }
}