///====================================================================================================
///
///     TextureMixing by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Canty.Managers;
using Random = System.Random;

namespace Canty.Editors
{
    /// <summary>
    /// Opens a tool that allows the user to quickly generate a gradient texture.
    /// </summary>
    public class TextureMixing : TextureGeneratorBase<TextureMixing>
    {
        public enum TextureMixingType
        {
            Replace,
            AlphaMultiplied,
            Multiply,
            Dissolve,
            Screen,
            Overlay,
            SoftLight,
            Divide,
            Add,
            Substract,
            Difference,
            Darken,
            Lighten
        }

        private TextureMixingType m_CurrentType = TextureMixingType.Replace;
        private TextureMixingType m_LastType = TextureMixingType.Replace;

        private Vector2Int m_Offset;

        private float m_DissolveThreshold = 0.5f;

        [MenuItem("Tool/Texture Generation/Blending")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData("Texture Blending");
        }

        protected override string GetTopName()
        {
            return "Texture Blending";
        }

        protected override string GetHelpTooltipText()
        {
            return "Applies the blended image onto the source image using the specified blending type. " +
                   "If the result and your sources are of the same size, there will be no data loss in the conversion. " +
                   "\n\n" +
                   "Note : " +
                   "\n-Each textures must have read/write enabled to be used by this tool. " +
                   "\n-Make sure that the sizes don't differ by much since there is no resizing algorithm at play here. Any size change will probably look like crap. " +
                   "\n-Limit of 4096x4096, and even then, the tool will take a long time to generate a result. Use large sizes at your own risk. " +
                   "\n-To remove a texture, click on the texture's square and press delete. An empty square will simply set that color to 0.";
        }

        protected override ComponentBoxData[] GetTextureBoxesData()
        {
            return new[] { new ComponentBoxData("Image 1", ComponentBoxData.ComponentBoxType.TextureColor), new ComponentBoxData("Image 2", ComponentBoxData.ComponentBoxType.TextureColor), new ComponentBoxData("Options", ComponentBoxData.ComponentBoxType.Custom, CustomEditorOptions) };
        }

        protected override Color ApplyMath(int x, int y)
        {
            Color result = Color.black;

            Color box1 = m_ComponentBoxes["Image 1"].IsColor ? m_ComponentBoxes["Image 1"].Color : m_ComponentBoxes["Image 1"].Texture ? m_ComponentBoxes["Image 1"].Texture.GetPixel(x, y) : Color.black;
            Color box2 = Color.black;
            if (m_ComponentBoxes["Image 2"].IsColor)
            {
                box2 = m_ComponentBoxes["Image 2"].Color;
            }
            else
            {
                if (m_ComponentBoxes["Image 2"].Texture != null)
                {
                    if (x - m_Offset.x < 0 || y - m_Offset.y < 0 ||
                        x - m_Offset.x >= m_ComponentBoxes["Image 2"].Texture.width || y - m_Offset.y >= m_ComponentBoxes["Image 2"].Texture.height)
                    {
                        box2 = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                    }
                    else
                    {
                        box2 = m_ComponentBoxes["Image 2"].Texture.GetPixel(x - m_Offset.x, y - m_Offset.y);
                    }
                }
                else
                {
                    box2 = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }

            switch (m_CurrentType)
            {
                case TextureMixingType.Replace:
                    result = box2 == new Color(0.0f, 0.0f, 0.0f, 0.0f) ? box1 : box2;
                    break;

                case TextureMixingType.AlphaMultiplied:
                    result = Color.Lerp(box1, box2, box2.a);
                    break;

                case TextureMixingType.Multiply:
                    result = box2 == new Color(0.0f, 0.0f, 0.0f, 0.0f) ? box1 : box1 * box2;
                    break;

                case TextureMixingType.Dissolve:
                    float random = UnityEngine.Random.value >= m_DissolveThreshold ? 1.0f : 0.0f;
                    result = Color.Lerp(box1, box2, random);
                    break;

                case TextureMixingType.Screen:
                    result = Color.white - (Color.white - box1) * (Color.white - box2);
                    break;

                case TextureMixingType.Overlay:
                    if (box1.r + box1.b + box1.g < 1.5f)
                    {
                        result = 2.0f * box1 * box2;
                    }
                    else
                    {
                        result = Color.white - 2.0f * (Color.white - box1) * (Color.white - box2);
                    }
                    break;

                case TextureMixingType.SoftLight:
                    if (box2.r + box2.b + box2.g <= 1.5f)
                    {
                        result = box1 - (Color.white - 2.0f * box2) * box1 * (Color.white - box1);
                    }
                    else
                    {
                        if (box1.r + box1.b + box1.g <= 0.75f)
                        {
                            result = ((16.0f * box1 - 12.0f * Color.white) * box1 + 4.0f * Color.white) * box1;
                        }
                        else
                        {
                            result = new Color(Mathf.Sqrt(box1.r), Mathf.Sqrt(box1.g), Mathf.Sqrt(box1.b), 1.0f);
                        }

                        result = box1 + (2.0f * Color.white - Color.white) * (result * box1 - box1);
                    }
                    break;

                case TextureMixingType.Divide:
                    Color boxFixed = new Color(Mathf.Max(0.001f, box2.r), Mathf.Max(0.001f, box2.g), Mathf.Max(0.001f, box2.b), Mathf.Max(0.001f, box2.a));
                    result = new Color(box1.r / boxFixed.r, box1.g / boxFixed.g, box1.b / boxFixed.b, 1.0f);
                    break;

                case TextureMixingType.Add:
                    result = box1 + box2;
                    break;

                case TextureMixingType.Substract:
                    result = box1 - box2;
                    break;

                case TextureMixingType.Difference:
                    float r = box1.r > box2.r ? box1.r - box2.r : box2.r - box1.r;
                    float g = box1.g > box2.g ? box1.g - box2.g : box2.g - box1.g;
                    float b = box1.b > box2.b ? box1.b - box2.b : box2.b - box1.b;
                    result = new Color(r, g, b, 1.0f);
                    break;

                case TextureMixingType.Darken:
                    result = new Color(Mathf.Min(box1.r, box2.r), Mathf.Min(box1.g, box2.g), Mathf.Min(box1.b, box2.b), 1.0f);
                    break;

                case TextureMixingType.Lighten:
                    result = new Color(Mathf.Max(box1.r, box2.r), Mathf.Max(box1.g, box2.g), Mathf.Max(box1.b, box2.b), 1.0f);
                    break;
            }

            return result;
        }

        private void CustomEditorOptions(float boxWidth)
        {
            if (GUILayout.Button("Swap Tex", GUILayout.Width(boxWidth), GUILayout.Height(20.0f)))
            {
                Texture2D temp = m_ComponentBoxes["Image 1"].Texture;
                m_ComponentBoxes["Image 1"].Texture = m_ComponentBoxes["Image 2"].Texture;
                m_ComponentBoxes["Image 2"].Texture = temp;
            }

            GUILayout.Label("Type", GUILayout.Width(boxWidth));
            m_CurrentType = (TextureMixingType)EditorGUILayout.EnumPopup("", m_CurrentType, GUILayout.Width(boxWidth));

            if (m_CurrentType != m_LastType)
            {
                m_LastType = m_CurrentType;
                m_Result = null;
            }

            GUILayout.Space(3.0f);

            GUILayout.Label("Image 2 Offset", GUILayout.Width(boxWidth));

            GUILayout.BeginHorizontal(GUILayout.Width(boxWidth));
            {
                m_Offset.x = EditorGUILayout.IntField("", m_Offset.x, GUILayout.Width((boxWidth / 2.0f) - 2.0f));
                m_Offset.y = EditorGUILayout.IntField("", m_Offset.y, GUILayout.Width((boxWidth / 2.0f) - 2.0f));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(3.0f);

            if (m_CurrentType == TextureMixingType.Dissolve)
            {
                GUILayout.Label("Threshold", GUILayout.Width(boxWidth));
                m_DissolveThreshold = EditorGUILayout.FloatField("", m_DissolveThreshold, GUILayout.Width(boxWidth));
                m_DissolveThreshold = Mathf.Clamp(m_DissolveThreshold, 0.0f, 1.0f);
            }
        }

        protected override float GetResultBoxYPos()
        {
            switch (m_CurrentType)
            {
                case TextureMixingType.Dissolve:
                    return 242.0f;

                default:
                    return 230.0f;
            }
            
        }

        protected override float[] GetSaveableValues()
        {
            return new[] { (float)m_CurrentType };
        }

        protected override void SetSaveableValues(float[] values)
        {
            if (values.Length == 1)
            {
                m_CurrentType = (TextureMixingType)values[0];
            }
        }
    }
}