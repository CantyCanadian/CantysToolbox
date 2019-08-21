///====================================================================================================
///
///     TextureMerger by
///     - CantyCanadian
///
///====================================================================================================

using UnityEditor;
using UnityEngine;

namespace Canty.Editors
{
    /// <summary>
    /// Opens a tool that allows the user to merge three grayscale textures into one RGB textures, reducing the amount of textures needed to be loaded in a shader since all the info can be compressed without loss this way.
    /// </summary>
    public class TextureMerger : TextureGeneratorBase<TextureMerger>
    {
        [MenuItem("Tool/Texture Generation/Grayscale Merger")]
        public static void ShowWindow()
        {
            SetDefaultTextureGeneratorWindowData();
        }
    }
}