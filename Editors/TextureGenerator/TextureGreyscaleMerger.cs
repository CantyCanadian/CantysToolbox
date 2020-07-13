///====================================================================================================
///
///     TextureMerger by
///     - CantyCanadian
///
///====================================================================================================

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Component = System.ComponentModel.Component;

namespace Canty.Editors
{
    /// <summary>
    /// Opens a tool that allows the user to merge three grayscale textures into one RGB textures, reducing the amount of textures needed to be loaded in a shader since all the info can be compressed without loss this way.
    /// </summary>
    public class TextureGreyscaleMerger : TextureGeneratorBase<TextureGreyscaleMerger>
    {
        [MenuItem("Tool/Texture Generation/GS Merger")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData("RGB to Greyscale Texture Merger");
        }

        protected override string GetTopName()
        {
            return "RGB to Greyscale Texture Merger";
        }

        protected override string GetHelpTooltipText()
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

        protected override ComponentBoxData[] GetTextureBoxesData()
        {
            return new[] { new ComponentBoxData("Red", ComponentBoxData.ComponentBoxType.TextureColor), new ComponentBoxData("Green", ComponentBoxData.ComponentBoxType.TextureColor), new ComponentBoxData("Blue", ComponentBoxData.ComponentBoxType.TextureColor) };
        }

        protected override Color ApplyMath(int x, int y)
        {
            Color result = Color.black;

            if (m_ComponentBoxes["Red"].IsColor)
            {
                result.r = m_ComponentBoxes["Red"].Color.r;
            }
            else
            {
                if (m_ComponentBoxes["Red"].Texture != null)
                {
                    result.r = m_ComponentBoxes["Red"].Texture.GetPixel(x, y).r;
                }
            }

            if (m_ComponentBoxes["Green"].IsColor)
            {
                result.g = m_ComponentBoxes["Green"].Color.g;
            }
            else
            {
                if (m_ComponentBoxes["Green"].Texture != null)
                {
                    result.g = m_ComponentBoxes["Green"].Texture.GetPixel(x, y).g;
                }
            }

            if (m_ComponentBoxes["Blue"].IsColor)
            {
                result.b = m_ComponentBoxes["Blue"].Color.b;
            }
            else
            {
                if (m_ComponentBoxes["Blue"].Texture != null)
                {
                    result.b = m_ComponentBoxes["Blue"].Texture.GetPixel(x, y).b;
                }
            }

            return result;
        }
    }
}