///====================================================================================================
///
///     TextureNoise by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections;
using System.Collections.Generic;
using Canty;
using Canty.Editors;
using UnityEditor;
using UnityEngine;

namespace Canty.Editors
{
    public class TextureNoise : TextureGeneratorBase<TextureNoise>
    {
        private Vector2Int m_Origin = Vector2Int.zero;
        private float m_Scale = 1.0f;

        public enum NoiseTypes
        {
            Perlin,
            White
        }

        [MenuItem("Tool/Texture Generation/Noise", false, 1)]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData("Noise Texture Generator");
        }

        protected override string GetTopName()
        {
            return "Noise Texture Generator";
        }

        protected override string GetHelpTooltipText()
        {
            return "Generate a noise texture using this tool. " +
                   "Simply set the sampling origin and the scale. " +
                   "In order to get different results, make sure to change the values between your textures. " +
                   "Perlin Noise isn't random, it gives the same value if you sample at the same place.";
        }

        protected override ComponentBoxData[] GetTextureBoxesData()
        {
            return new[] {new ComponentBoxData("Color 1", ComponentBoxData.ComponentBoxType.Color), new ComponentBoxData("Color 2", ComponentBoxData.ComponentBoxType.Color), new ComponentBoxData("Options", ComponentBoxData.ComponentBoxType.Custom, CustomEditorOptions)};
        }

        protected override Color ApplyMath(int x, int y)
        {
            float xCoord = m_Origin.x + x / (float) m_ResultSize.x * m_Scale;
            float yCoord = m_Origin.y + y / (float) m_ResultSize.y * m_Scale;
            float noise = Mathf.PerlinNoise(xCoord, yCoord);

            return Color.Lerp(m_ComponentBoxes["Color 1"].Color, m_ComponentBoxes["Color 2"].Color, noise);
        }

        private void CustomEditorOptions(float boxWidth)
        {
            GUILayout.Space(20.0f);

            GUILayout.Label("Origin", GUILayout.Width(boxWidth));

            GUILayout.BeginHorizontal(GUILayout.Width(boxWidth));
            {
                m_Origin.x = EditorGUILayout.IntField("", m_Origin.x, GUILayout.Width((boxWidth / 2.0f) - 2.0f));
                m_Origin.y = EditorGUILayout.IntField("", m_Origin.y, GUILayout.Width((boxWidth / 2.0f) - 2.0f));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(3.0f);

            GUILayout.Label("Scale", GUILayout.Width(boxWidth));
            m_Scale = EditorGUILayout.FloatField("", m_Scale, GUILayout.Width(boxWidth));
        }

        protected override float[] GetSaveableValues()
        {
            return new[] {m_Origin.x, m_Origin.y, m_Scale};
        }

        protected override void SetSaveableValues(float[] values)
        {
            if (values.Length == 3)
            {
                m_Origin.x = (int) values[0];
                m_Origin.y = (int) values[1];
                m_Scale = values[2];
            }
        }
    }
}