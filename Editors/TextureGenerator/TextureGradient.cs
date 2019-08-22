///====================================================================================================
///
///     TextureGradient by
///     - CantyCanadian
///
///====================================================================================================

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
        private float m_Offset = 0.0f;
        private float m_Radius = 0.5f;
        private float m_Pinch = 0.0f;

        [MenuItem("Tool/Texture Generation/Gradient")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData();
        }

        override protected string GetTopName()
        {
            return "Gradient Texture Generator";
        }

        override protected string GetHelpTooltipText()
        {
            return "";
        }

        override protected TextureBoxData[] GetTextureBoxesData()
        {
            return new[] { new TextureBoxData("Color 1", ComponentBoxData.ComponentBoxType.Color), new TextureBoxData("Color 2", ComponentBoxData.ComponentBoxType.Color), new TextureBoxData("Options", ComponentBoxData.ComponentBoxType.Custom, CustomEditorOptions) };
        }

        override protected Color ApplyMath(int x, int y, Dictionary<string, TextureColorContainer> containers)
        {
            Color result = containers["Base"].IsColor ? containers["Base"].Color : containers["Base"].Texture.GetPixel(x, y);

            Color overlay = containers["Overlay"].Texture.GetPixel(x, y);
            overlay.a *= containers["Mask"].IsColor ? containers["Mask"].Color : containers["Mask"].Texture.GetPixel(x, y);

            return result;
        }

        private void CustomEditorOptions(System.Action<Dictionary<string, TextureColorContainer>> containers)
        {
            m_CurrentType = EditorGUILayout.EnumPopup("Type : ", m_CurrentType);

            if (m_CurrentType == TextureGradientTypes.Linear)
            {
                m_RotationAngle = EditorGUILayout.FloatField("Angle : ", m_RotationAngle);
                m_RotationAngle = Mathf.Clamp(m_RotationAngle, -360.0f, 360.0f);

                m_Pinch = EditorGUILayout.FloatField("Pinch : ", m_RotationAngle);
                m_Pinch = Mathf.Clamp(m_Pinch, 0.0f, 1.0f);
            }
            else
            {
                m_Radius = EditorGUILayout.FloatField("Radius : ", m_Radius);
                m_Radius = Mathf.Clamp(m_Radius, 0.0f, 1.0f);

                m_Pinch = EditorGUILayout.FloatField("Pinch : ", m_Pinch);
                m_Pinch = Mathf.Clamp(m_Pinch, 0.0f, 1.0f);
            }



            // COLOR BOX
            GUILayout.BeginHorizontal(GUILayout.Width(70.0f));
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(boxName, GUILayout.ExpandWidth(true));
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            container.Color = EditorGUILayout.ColorField(container.Color, GUILayout.Width(70), GUILayout.Height(70));
        }
    }
}