using UnityEngine;

public static class GUIUtil
{
    public static GUIStyle TitleStyle
    {
        get
        {
            if (m_TitleStyle == null)
            {
                m_TitleStyle = GUI.skin.GetStyle("Label");
                m_TitleStyle.alignment = TextAnchor.MiddleCenter;
                m_TitleStyle.clipping = TextClipping.Clip;
                m_TitleStyle.fontSize = 24;
            }

            return m_TitleStyle;
        }
    }

    public static GUIStyle CenterStyle
    {
        get
        {
            if (m_CenterStyle == null)
            {
                m_CenterStyle = GUI.skin.GetStyle("Label");
                m_CenterStyle.alignment = TextAnchor.MiddleCenter;
            }

            return m_CenterStyle;
        }
    }

    private static GUIStyle m_TitleStyle = null;
    private static GUIStyle m_CenterStyle = null;
}
