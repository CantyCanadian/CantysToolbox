///====================================================================================================
///
///     TextureShifting by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Canty.Managers;

namespace Canty.Editors
{
    /// <summary>
    /// Opens a tool that allows the user to apply a texture with a mask on top of another texture.
    /// </summary>
    public class TextureShifting : TextureGeneratorBase<TextureShifting>
    {
        private Vector2Int m_Offset = Vector2Int.zero;

        [MenuItem("Tool/Texture Generation/Shifting")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData("Texture Shifting");
        }

        protected override string GetTopName()
        {
            return "Texture Shifting";
        }

        protected override string GetHelpTooltipText()
        {
            return "This tool allows the user to shift an image. " +
                   "Mostly useful in cases where you don't want to pull Photoshop to do it. " +
                   "Make sure to set the image's Wrap Mode as this tool simply shifts the UVs." +
                   "Can only be used on textures that are set to read/write. " +
                   "Please make sure that the result size is equal to your image size.";
        }

        protected override ComponentBoxData[] GetTextureBoxesData()
        {
            return new[] { new ComponentBoxData("Image", ComponentBoxData.ComponentBoxType.Texture), new ComponentBoxData("Options", ComponentBoxData.ComponentBoxType.Custom, CustomEditorOption) };
        }

        protected override Color ApplyMath(int x, int y)
        {
            Color result = Color.black;
            if (m_ComponentBoxes["Image"].Texture != null)
            {
                result = m_ComponentBoxes["Image"].Texture.GetPixel(x + m_Offset.x, y + m_Offset.y);
            }

            return result;
        }

        private void CustomEditorOption(float boxWidth)
        {
            GUILayout.Space(20.0f);

            GUILayout.Label("Offset", GUILayout.Width(boxWidth));

            GUILayout.BeginHorizontal(GUILayout.Width(boxWidth));
            {
                m_Offset.x = EditorGUILayout.IntField("", m_Offset.x, GUILayout.Width((boxWidth / 2.0f) - 2.0f));
                m_Offset.y = EditorGUILayout.IntField("", m_Offset.y, GUILayout.Width((boxWidth / 2.0f) - 2.0f));
            }
            GUILayout.EndHorizontal();
        }
    }
}