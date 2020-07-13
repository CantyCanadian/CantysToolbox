using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TextureCreatorComponentContainerModifierTypes
{
    Offset
}

public class TextureCreatorComponentContainerModifierOffset : TextureCreatorComponentContainerBase
{
    public enum OffsetTypes
    {
        None,
        Clamp,
        Repeat,
        Mirror
    }

    private Vector2Int m_Offset = Vector2Int.zero;
    private OffsetTypes m_OffsetType = OffsetTypes.None;
    private Vector2Int m_Margin = Vector2Int.zero;

    public override void OnGUI(float width)
    {
        Vector2Int offset = m_Offset;
        GUILayout.Label("Offset :", GUILayout.Width(width));
        GUILayout.BeginHorizontal(GUILayout.Width(width));
        {
            m_Offset.x = EditorGUILayout.IntField("", m_Offset.x, GUILayout.Width(width / 2.0f - 2.0f));
            GUILayout.FlexibleSpace();
            m_Offset.y = EditorGUILayout.IntField("", m_Offset.y, GUILayout.Width(width / 2.0f - 2.0f));
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(8.0f);

        OffsetTypes type = m_OffsetType;
        GUILayout.Label("Offset Type :");
        m_OffsetType = (OffsetTypes)EditorGUILayout.EnumPopup(m_OffsetType, GUILayout.Width(width));

        GUILayout.Space(8.0f);

        GUILayout.Label("Margin between repeats :");

        Vector2Int margin = m_Margin;
        GUILayout.BeginHorizontal(GUILayout.Width(width));
        {
            m_Margin.x = EditorGUILayout.IntField("", m_Margin.x, GUILayout.Width(width / 2.0f - 2.0f));
            GUILayout.FlexibleSpace();
            m_Margin.y = EditorGUILayout.IntField("", m_Margin.y, GUILayout.Width(width / 2.0f - 2.0f));
        }
        GUILayout.EndHorizontal();

        IsDirty = offset != m_Offset || type != m_OffsetType || margin != m_Margin;
    }

    public override Texture2D Invoke(Texture2D input)
    {
        Color32[] pixels = input.GetPixels32();

        // X
        if (m_Offset.x > 0)
        {
            for (int y = 0; y < input.height; y++)
            {
                for (int x = input.width - 1; x >= 0; x--)
                {
                    if (x - m_Offset.x < 0 || x - m_Offset.x >= input.width)
                    {
                        pixels[y * input.width + x] = Color.black;
                    }
                    else
                    {
                        pixels[y * input.width + x] = pixels[y * input.width + x - m_Offset.x];
                    }
                }
            }
        }
        else
        {
            for (int y = 0; y < input.height; y++)
            {
                for (int x = 0; x < input.width; x++)
                {
                    if (x - m_Offset.x < 0 || x - m_Offset.x >= input.width)
                    {
                        pixels[y * input.width + x] = Color.black;
                    }
                    else
                    {
                        pixels[y * input.width + x] = pixels[y * input.width + x - m_Offset.x];
                    }
                }
            }
        }

        // Y
        if (m_Offset.y > 0)
        {
            for (int y = 0; y < input.height; y++)
            {
                for (int x = 0; x < input.width; x++)
                {
                    if (y + m_Offset.y < 0 || y + m_Offset.y >= input.height)
                    {
                        pixels[y * input.width + x] = Color.black;
                    }
                    else
                    {
                        pixels[y * input.width + x] = pixels[(y + m_Offset.y) * input.width + x];
                    }
                }
            }
        }
        else
        {
            for (int y = input.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < input.width; x++)
                {
                    if (y + m_Offset.y < 0 || y + m_Offset.y >= input.height)
                    {
                        pixels[y * input.width + x] = Color.black;
                    }
                    else
                    {
                        pixels[y * input.width + x] = pixels[(y + m_Offset.y) * input.width + x];
                    }
                }
            }
        }

        // Offset Type
        for (int y = 0; y < input.height; y++)
        {
            int flipY = input.height - y - 1;

            for (int x = 0; x < input.width; x++)
            {
                if (x >= m_Offset.x && x < input.width + m_Offset.x && y >= m_Offset.y && y < input.height + m_Offset.y)
                {
                    continue;
                }

                switch (m_OffsetType)
                {
                    case OffsetTypes.Clamp:
                        {
                            //if (x < m_Offset.x)
                            //{
                            //    if (y < m_Offset.y)
                            //    {
                            //        pixels[flipY * input.width + x] = pixels[(input.height + m_Offset.y) * input.width + m_Offset.x];
                            //    }
                            //    else
                            //    {
                            //        pixels[flipY * input.width + x] = pixels[y * input.width + m_Offset.x];
                            //    }
                            //}
                            //else if (y < m_Offset.y)
                            //{
                            //    pixels[flipY * input.width + x] = pixels[(input.height - m_Offset.y - 1) * input.width + x];
                            //}

                            //if (x >= input.width + m_Offset.x)
                            //{
                            //    if (y >= input.height + m_Offset.y)
                            //    {
                            //        pixels[flipY * input.width + x] = pixels[Mathf.Abs(m_Offset.y) * input.width + (input.width + m_Offset.x)];
                            //    }
                            //    else
                            //    {
                            //        pixels[flipY * input.width + x] = pixels[y * input.width + (input.width + m_Offset.x - 1)];
                            //    }
                            //}
                            //else if (y >= input.height + m_Offset.y)
                            //{
                            //    pixels[flipY * input.width + x] = pixels[Mathf.Abs(m_Offset.y) * input.width + x];
                            //}



                            break;
                        }
                }
            }
        }

        input.SetPixels32(pixels);
        input.Apply();
        return input;
    }

    public TextureCreatorComponentContainerModifierOffset() : base(TextureCreatorComponentContainerTypes.Modifier, "Texture Offset") { }
}
