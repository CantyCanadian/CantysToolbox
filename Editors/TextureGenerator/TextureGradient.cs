///====================================================================================================
///
///     TextureGradient by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Canty.Managers;

namespace Canty.Editors
{
    /// <summary>
    /// Opens a tool that allows the user to quickly generate a gradient texture.
    /// </summary>
    public class TextureGradient : TextureGeneratorBase<TextureGradient>
    {
        public enum TextureGradientTypes
        {
            Linear,
            Radial
        }

        private TextureGradientTypes m_CurrentType = TextureGradientTypes.Linear;
        private float m_RotationAngle = 0.0f;
        private float m_Radius = 0.5f;
        private float m_Spread = 1.0f;

        [MenuItem("Tool/Texture Generation/Gradient", false, 1)]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData("Gradient Texture Generator");
        }

        protected override string GetTopName()
        {
            return "Gradient Texture Generator";
        }

        protected override string GetHelpTooltipText()
        {
            return "Generate a simple gradient texture using this tool. " +
                   "You get to select whether you want it radial or linear. " +
                   "There isn't much to explain here, minus a warning that your textures both in and out should match in size for the best results and that the textures should be set to read/write.";
        }

        protected override ComponentBoxData[] GetTextureBoxesData()
        {
            return new[] { new ComponentBoxData("Grad 1", ComponentBoxData.ComponentBoxType.TextureColor), new ComponentBoxData("Grad 2", ComponentBoxData.ComponentBoxType.TextureColor), new ComponentBoxData("Options", ComponentBoxData.ComponentBoxType.Custom, CustomEditorOptions) };
        }

        protected override Color ApplyMath(int x, int y)
        {
            Color result = Color.black;

            Color box1 = m_ComponentBoxes["Grad 1"].IsColor ? m_ComponentBoxes["Grad 1"].Color : m_ComponentBoxes["Grad 1"].Texture ? m_ComponentBoxes["Grad 1"].Texture.GetPixel(x, y) : Color.black;
            Color box2 = m_ComponentBoxes["Grad 2"].IsColor ? m_ComponentBoxes["Grad 2"].Color : m_ComponentBoxes["Grad 2"].Texture ? m_ComponentBoxes["Grad 2"].Texture.GetPixel(x, y) : Color.black;

            float largest = m_ResultSize.x > m_ResultSize.y ? m_ResultSize.x : m_ResultSize.y;
            largest /= 2.0f;

            Vector2 pos = new Vector2(x, y);

            switch (m_CurrentType)
            {
                case TextureGradientTypes.Linear:
                    Vector2 newPosAt0 = pos.MinusScalar(largest).Rotate(-m_RotationAngle).AddScalar(largest);
                    float spreaded = largest * m_Spread;

                    if (newPosAt0.x <= largest - spreaded)
                    {
                        result = box1;
                    }
                    else if (newPosAt0.x <= largest + spreaded)
                    {
                        result = Color.Lerp(box1, box2, (newPosAt0.x - (largest - spreaded)) / (spreaded * 2.0f));
                    }
                    else
                    {
                        result = box2;
                    }
                    break;

                case TextureGradientTypes.Radial:
                {
                    float dist = Vector2.Distance(pos.MinusScalar(largest), Vector2.zero);

                    if (dist <= largest * m_Radius)
                    {
                        result = box1;
                    }
                    else if (dist - (largest * m_Radius) <=  (largest * m_Spread))
                    {
                        result = Color.Lerp(box1, box2, (dist - (largest * m_Radius)) / (largest * m_Spread));
                    }
                    else
                    {
                        result = box2;
                    }
                }
                break;
            }

            return result;
        }

        private void CustomEditorOptions(float boxWidth)
        {
            if (GUILayout.Button("Swap Tex", GUILayout.Width(boxWidth), GUILayout.Height(20.0f)))
            {
                Texture2D temp = m_ComponentBoxes["Grad 1"].Texture;
                m_ComponentBoxes["Grad 1"].Texture = m_ComponentBoxes["Grad 2"].Texture;
                m_ComponentBoxes["Grad 2"].Texture = temp;
            }

            GUILayout.Label("Type", GUILayout.Width(boxWidth));
            m_CurrentType = (TextureGradientTypes)EditorGUILayout.EnumPopup("", m_CurrentType, GUILayout.Width(boxWidth));

            GUILayout.Space(3.0f);

            if (m_CurrentType == TextureGradientTypes.Linear)
            {
                GUILayout.Label("Angle", GUILayout.Width(boxWidth));
                m_RotationAngle = EditorGUILayout.FloatField("", m_RotationAngle, GUILayout.Width(boxWidth));
                m_RotationAngle = Mathf.Clamp(m_RotationAngle, 0.0f, 180.0f);

                GUILayout.Space(3.0f);

                GUILayout.Label("Spread", GUILayout.Width(boxWidth));
                m_Spread = EditorGUILayout.FloatField("", m_Spread, GUILayout.Width(boxWidth));
                m_Spread = Mathf.Clamp(m_Spread, 0.0f, 2.0f);
            }
            else
            {
                GUILayout.Label("Radius", GUILayout.Width(boxWidth));
                m_Radius = EditorGUILayout.FloatField("", m_Radius, GUILayout.Width(boxWidth));
                m_Radius = Mathf.Clamp(m_Radius, 0.0f, 1.0f);

                GUILayout.Space(3.0f);

                GUILayout.Label("Spread", GUILayout.Width(boxWidth));
                m_Spread = EditorGUILayout.FloatField("", m_Spread, GUILayout.Width(boxWidth));
                m_Spread = Mathf.Clamp(m_Spread, 0.0f, 5.0f);
            }
        }

        protected override float GetResultBoxYPos()
        {
            return 238.0f;
        }

        protected override float[] GetSaveableValues()
        {
            return new [] { (float)m_CurrentType, m_RotationAngle, m_Spread, m_Radius };
        }

        protected override void SetSaveableValues(float[] values)
        {
            if (values.Length == 4)
            {
                m_CurrentType = (TextureGradientTypes)values[0];
                m_RotationAngle = values[1];
                m_Spread = values[2];
                m_Radius = values[3];
            }
        }
    }
}