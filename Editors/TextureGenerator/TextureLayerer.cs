///====================================================================================================
///
///     TextureLayerer by
///     - CantyCanadian
///
///====================================================================================================

using UnityEditor;
using UnityEngine;
using Canty.Managers;

namespace Canty.Editors
{
    /// <summary>
    /// Opens a tool that allows the user to apply a texture with a mask on top of another texture.
    /// </summary>
    public class TextureLayerer : TextureGeneratorBase<TextureLayerer>
    {
        [MenuItem("Tool/Texture Generation/Overlay")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData();
        }

        override protected string GetTopName()
        {
            return "Texture Overlay";
        }

        override protected string GetHelpTooltipText()
        {
            return "Applies the overlay image onto the source image using the mask image for the alpha." +
                "Perfect for quickly merging two images together in a specific way." +
                "The mask uses alpha as its masking method, not greyscale. Be careful." +
                "If the result and your sources are of the same size, there will be no data loss in the conversion. " +
                "\n\n" +
                "Note : " +
                "\n-Each textures must have read/write enabled to be used by this tool. " +
                "\n-Make sure that the sizes don't differ by much since there is no resizing algorithm at play here. Any size change will probably look like crap. " +
                "\n-Limit of 4096x4096, and even then, the tool will take a long time to generate a result. Use large sizes at your own risk. " +
                "\n-To remove a texture, click on the texture's square and press delete. An empty square will simply set that color to 0.";
        }

        override protected TextureBoxData[] GetTextureBoxesData()
        {
            return new[] { new TextureBoxData("Base", ComponentBoxData.ComponentBoxType.TextureColor), new TextureBoxData("Overlay", ComponentBoxData.ComponentBoxType.Texture), new TextureBoxData("Mask", ComponentBoxData.ComponentBoxType.TextureColor) };
        }

        override protected Color ApplyMath(int x, int y, Dictionary<string, TextureColorContainer> containers)
        {
            Color result = containers["Base"].IsColor ? containers["Base"].Color : containers["Base"].Texture.GetPixel(x, y);

            Color overlay = containers["Overlay"].Texture.GetPixel(x, y);
            overlay.a *= containers["Mask"].IsColor ? containers["Mask"].Color : containers["Mask"].Texture.GetPixel(x, y);

            return result;
        }
    }
}