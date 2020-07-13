///====================================================================================================
///
///     TextureOverlay by
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
    public class TextureOverlay : TextureGeneratorBase<TextureOverlay>
    {
        [MenuItem("Tool/Texture Generation/Masking")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData("Texture Masking");
        }

        protected override string GetTopName()
        {
            return "Texture Masking";
        }

        protected override string GetHelpTooltipText()
        {
            return "Applies the overlay image onto the source image using the mask image for the alpha. " +
                "Perfect for quickly merging two images together in a specific way. " +
                "The mask uses alpha as its masking method, not greyscale. Be careful. " +
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
            return new[] { new ComponentBoxData("Base", ComponentBoxData.ComponentBoxType.TextureColor), new ComponentBoxData("Overlay", ComponentBoxData.ComponentBoxType.Texture), new ComponentBoxData("Mask", ComponentBoxData.ComponentBoxType.TextureColor) };
        }

        protected override Color ApplyMath(int x, int y)
        {
            TextureColorContainer baseContainer = m_ComponentBoxes["Base"];
            TextureColorContainer overlayContainer = m_ComponentBoxes["Overlay"];
            TextureColorContainer maskContainer = m_ComponentBoxes["Mask"];

            Color result = Color.black;
            Color overlay = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            if (baseContainer.IsColor)
            {
                result = baseContainer.Color;
            }
            else
            {
                if (baseContainer.Texture != null)
                {
                    result = baseContainer.Texture.GetPixel(x, y);
                }
            }

            if (overlayContainer.Texture != null)
            {
                overlay = overlayContainer.Texture.GetPixel(x, y);
            }

            if (maskContainer.IsColor)
            {
                overlay.a *= maskContainer.Color.a;
            }
            else
            {
                if (maskContainer.Texture != null)
                {
                    overlay.a *= maskContainer.Texture.GetPixel(x, y).a;
                }
            }

            result = Color.Lerp(result, overlay, overlay.a);

            return result;
        }
    }
}