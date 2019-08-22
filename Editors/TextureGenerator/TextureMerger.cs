///====================================================================================================
///
///     TextureMerger by
///     - CantyCanadian
///
///====================================================================================================

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Canty.Editors
{
    /// <summary>
    /// Opens a tool that allows the user to merge three grayscale textures into one RGB textures, reducing the amount of textures needed to be loaded in a shader since all the info can be compressed without loss this way.
    /// </summary>
    public class TextureMerger : TextureGeneratorBase<TextureMerger>
    {
        [MenuItem("Tool/Texture Generation/Merger")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData();
        }

        override protected string GetTopName()
        {
            return "RGB to Greyscale Merger";
        }

        override protected string GetHelpTooltipText()
        {
            return "Compress your textures using this tool. " +
                "Instead of loading 3 grayscale textures in memory for your shader, merge them into a single image. " +
                "Each color of the new image represents one of your old grayscale texture. " +
                "If the result and your 3 sources are of the same size, there will be no data loss in the conversion. " +
                "\n\n" +
                "Note : " +
                "\n-Each textures must have read/write enabled to be used by this tool. " +
                "\n-Make sure that the sizes don't differ by much since there is no resizing algorithm at play here. Any size change will probably look like crap. " +
                "\n-Limit of 4096x4096, and even then, the tool will take a long time to generate a result. Use large sizes at your own risk. " +
                "\n-To remove a texture, click on the texture's square and press delete. An empty square will simply set that color to 0.";
        }

        override protected TextureBoxData[] GetTextureBoxesData()
        {
            return new[] { new TextureBoxData("Red", ComponentBoxData.ComponentBoxType.TextureColor), new TextureBoxData("Green", ComponentBoxData.ComponentBoxType.TextureColor), new TextureBoxData("Blue", ComponentBoxData.ComponentBoxType.TextureColor) };
        }

        override protected Color ApplyMath(int x, int y, Dictionary<string, TextureColorContainer> containers)
        {
            Color result = Color.black;

            if (containers["Red"].IsColor)
            {
                result.r = containers["Red"].Color;
            }
            else
            {
                if (containers["Red"].Texture != null)
                {
                    result.r = containers["Red"].Texture.GetPixel(x, y);
                }
            }

            if (containers["Green"].IsColor)
            {
                result.g = containers["Green"].Color;
            }
            else
            {
                if (containers["Green"].Texture != null)
                {
                    result.g = containers["Green"].Texture.GetPixel(x, y);
                }
            }

            if (containers["Blue"].IsColor)
            {
                result.b = containers["Blue"].Color;
            }
            else
            {
                if (containers["Blue"].Texture != null)
                {
                    result.b = containers["Blue"].Texture.GetPixel(x, y);
                }
            }

            return result;
        }
    }
}