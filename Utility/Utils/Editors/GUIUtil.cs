using UnityEngine;

public static class GUIUtil
{
    public static GUIStyle TitleStyle
    {
        get
        {
            if (m_CenterStyle == null)
            {
                m_CenterStyle = GUI.skin.GetStyle("Label");
                m_CenterStyle.alignment = TextAnchor.MiddleCenter;
                m_CenterStyle.clipping = TextClipping.Clip;
                m_CenterStyle.fontSize = 24;
            }

            return m_CenterStyle;
        }
    }

    private static GUIStyle m_CenterStyle = null;
}
