using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Texture2DExtension
{
    /// <summary>
    /// Sets every value of a Texture2D to a specified color.
    /// </summary>
    public static Texture2D Fill(this Texture2D texture, Color color)
    {
        return Fill(texture, (Color32) color);
    }

    /// <summary>
    /// Sets every value of a Texture2D to a specified color.
    /// </summary>
    public static Texture2D Fill(this Texture2D texture, Color32 color)
    {
        Color32[] colorArray = new Color32[texture.width * texture.height];
        for (int i = 0; i < colorArray.Length; i++)
        {
            colorArray[i] = color;
        }

        texture.SetPixels32(colorArray);
        texture.Apply();
        return texture;
    }
}
