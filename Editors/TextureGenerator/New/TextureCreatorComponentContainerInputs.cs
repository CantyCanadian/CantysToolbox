using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TextureCreatorComponentContainerInputTypes
{
    Texture
}

public class TextureCreatorComponentContainerInputTexture : TextureCreatorComponentContainerBase
{
    public enum ScalingTypes
    {
        None,
        NearestNeighbor
    }

    private Texture2D m_Texture = Texture2D.blackTexture;

    private bool m_OverrideSize = false;
    private Vector2Int m_OverridenSize = Vector2Int.zero;
    private ScalingTypes m_ScalingType = ScalingTypes.None;

    public override void OnGUI(float width)
    {
        int hash = m_Texture.GetHashCode();
        m_Texture = (Texture2D) EditorGUILayout.ObjectField(m_Texture, typeof(Texture2D), false, GUILayout.Width(width));

        if (hash != m_Texture.GetHashCode())
        {
            if (!m_Texture.isReadable)
            {
                Debug.LogWarning("Texture Creator : Given texture is not readable.");
                m_Texture = Texture2D.blackTexture;
            }
            else if (m_Texture.format != TextureFormat.RGBA32 && m_Texture.format != TextureFormat.BGRA32 && m_Texture.format != TextureFormat.RGB24)
            {
                Debug.Log(m_Texture.format);
                Debug.LogWarning("Texture Creator : Given texture must be in RGBA32 or BGRA32 format.");
                m_Texture = Texture2D.blackTexture;
            }
        }

        GUILayout.Space(8.0f);

        bool overrideSize = m_OverrideSize;
        m_OverrideSize = GUILayout.Toggle(m_OverrideSize, "Override Size", GUILayout.Width(width));
        if (overrideSize != m_OverrideSize)
        {
            m_OverridenSize = new Vector2Int(m_Texture.width, m_Texture.height);
        }

        GUILayout.Space(8.0f);

        Vector2Int overridenSize = m_OverridenSize;
        ScalingTypes scaling = m_ScalingType;
        if (m_OverrideSize)
        {
            GUILayout.Label("New Size :");
            
            GUILayout.BeginHorizontal(GUILayout.Width(width));
            {
                m_OverridenSize.x = EditorGUILayout.IntField(m_OverridenSize.x);
                m_OverridenSize.y = EditorGUILayout.IntField(m_OverridenSize.y);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(8.0f);

            GUILayout.Label("Scaling Algorithm :");
            m_ScalingType = (ScalingTypes)EditorGUILayout.EnumPopup(m_ScalingType, GUILayout.Width(width));
        }

        IsDirty = hash != m_Texture.GetHashCode() || overridenSize != m_OverridenSize || overrideSize != m_OverrideSize || scaling != m_ScalingType;
    }

    public override Texture2D Invoke(Texture2D input)
    {
        if (m_OverrideSize)
        {
            Texture2D result = new Texture2D(m_OverridenSize.x, m_OverridenSize.y, m_Texture.format, false);

            if (m_OverridenSize.x <= 0 || m_OverridenSize.y <= 0)
            {
                return result;
            }

            switch (m_ScalingType)
            {
                case ScalingTypes.None:
                    return None(result);

                case ScalingTypes.NearestNeighbor:
                    return NearestNeighbor(result);

                default:
                    return result;
            }
        }
        else
        {
            Texture2D result = new Texture2D(m_Texture.width, m_Texture.height, m_Texture.format, false);
            result.SetPixels32(m_Texture.GetPixels32());
            result.Apply();
            return result;
        }
    }

    private Texture2D None(Texture2D result)
    {
        Color32[] pixels = m_Texture.GetPixels32();
        Color32[] resultPixels = new Color32[result.width * result.height];

        for (int y = 0; y < result.height; y++)
        {
            if (y >= m_Texture.height)
            {
                continue;
            }

            for (int x = 0; x < result.width; x++)
            {
                if (x >= m_Texture.width)
                {
                    continue;
                }

                resultPixels[y * result.width + x] = pixels[y * m_Texture.width + x];
            }
        }

        result.SetPixels32(resultPixels);
        result.Apply();
        return result;
    }

    private Texture2D NearestNeighbor(Texture2D result)
    {
        float xCoeff = (float)m_Texture.width / (float)result.width;
        float yCoeff = (float)m_Texture.height / (float)result.height;

        Color32[] pixels = m_Texture.GetPixels32();
        Color32[] resultPixels = new Color32[result.width * result.height];

        for (int y = 0; y < result.height; y++)
        {
            for (int x = 0; x < result.width; x++)
            {
                resultPixels[y * result.width + x] = pixels[Mathf.FloorToInt(y * yCoeff) * m_Texture.width + Mathf.FloorToInt(x * xCoeff)];
            }
        }

        result.SetPixels32(resultPixels);
        result.Apply();
        return result;
    }

    /// TODO : Add more texture filtering options.

    //private Texture2D Bilinear(Texture2D result)
    //{
    //    float xCoeff = (float)m_Texture.width / (float)result.width;
    //    float yCoeff = (float)m_Texture.height / (float)result.height;

    //    Color32[] pixels = m_Texture.GetPixels32();
    //    Color32[] resultPixels = new Color32[result.width * result.height];

    //    for (int y = 0; y < result.height; y++)
    //    {
    //        int fixYPlus1 = Mathf.Min(y + 1, result.height - 1);

    //        for (int x = 0; x < result.width; x++)
    //        {
    //            int fixXPlus1 = Mathf.Min(x + 1, result.width - 1);

    //            Color32 qCenter = pixels[Mathf.FloorToInt(y * yCoeff) * m_Texture.width + Mathf.FloorToInt(x * xCoeff)];
    //            Color32 q11 = pixels[Mathf.FloorToInt(Mathf.Max(0, y - 1) * yCoeff) * m_Texture.width + Mathf.FloorToInt(Mathf.Max(0, x - 1) * xCoeff)];
    //            Color32 q21 = pixels[Mathf.FloorToInt(Mathf.Max(0, y - 1) * yCoeff) * m_Texture.width + Mathf.FloorToInt(Mathf.Min(m_Texture.width - 1, fixXPlus1) * xCoeff)];
    //            Color32 q12 = pixels[Mathf.FloorToInt(Mathf.Min(m_Texture.height - 1, fixYPlus1) * yCoeff) * m_Texture.width + Mathf.FloorToInt(Mathf.Max(0, x - 1) * xCoeff)];
    //            Color32 q22 = pixels[Mathf.FloorToInt(Mathf.Min(m_Texture.height - 1, fixYPlus1) * yCoeff) * m_Texture.width + Mathf.FloorToInt(Mathf.Min(m_Texture.width - 1, fixXPlus1) * xCoeff)];

    //            byte r = (byte)Mathf.RoundToInt((qCenter.r + q11.r + q21.r + q12.r + q22.r) / 5.0f);
    //            byte g = (byte)Mathf.RoundToInt((qCenter.g + q11.g + q21.g + q12.g + q22.g) / 5.0f);
    //            byte b = (byte)Mathf.RoundToInt((qCenter.b + q11.b + q21.b + q12.b + q22.b) / 5.0f);
    //            byte a = (byte)Mathf.RoundToInt((qCenter.a + q11.a + q21.a + q12.a + q22.a) / 5.0f);

    //            resultPixels[y * result.width + x] = new Color32(r, g, b, a);
    //        }
    //    }

    //    result.SetPixels32(resultPixels);
    //    result.Apply();
    //    return result;
    //}

    public TextureCreatorComponentContainerInputTexture() : base(TextureCreatorComponentContainerTypes.Input, "Input Texture") { }
}
