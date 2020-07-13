using UnityEditor;
using UnityEngine;

public static class GUIUtil
{
    public static GUIStyle TitleStyle
    {
        get
        {
            GUIStyle result = new GUIStyle(GUI.skin.GetStyle("Label"));
            result.alignment = TextAnchor.MiddleCenter;
            result.clipping = TextClipping.Clip;
            result.fontSize = 24;

            return result;
        }
    }

    public static GUIStyle CenterStyle
    {
        get
        {
            GUIStyle result = new GUIStyle(GUI.skin.GetStyle("Label"));
            result.alignment = TextAnchor.MiddleCenter;

            return result;
        }
    }

    public static void DrawSeparatorLine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2.0f;
        r.x -= 4;
        r.width += 8;
        EditorGUI.DrawRect(r, color);
    }

    /// <summary>
    /// Same as GUI.BeginScrollView, but the bar is on top and is can only change in width.
    /// Don't forget to close it with GUI.EndScrollView().
    /// </summary>
    public static Vector2 BeginLeftSideVerticalScrollView(Rect position, Vector2 scrollPosition, Rect viewRect)
    {
        scrollPosition = GUI.BeginScrollView(new Rect(position.x, position.y, 13.0f, position.height), scrollPosition, new Rect(position.x, position.y, 0.0f, viewRect.height), false, true);
        GUI.EndScrollView();

        GUI.BeginScrollView(new Rect(position.x + 13.0f, position.y, position.width - 13.0f, position.height), scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);

        return scrollPosition;
    }

    /// <summary>
    /// Same as GUI.BeginScrollView, but the bar is on top and is can only change in width.
    /// Don't forget to close it with GUI.EndScrollView().
    /// </summary>
    public static Vector2 BeginUpperHorizontalScrollView(Rect position, Vector2 scrollPosition, Rect viewRect)
    {
        scrollPosition = GUI.BeginScrollView(new Rect(position.x, position.y, position.width, 13.0f), scrollPosition, new Rect(position.x, position.y, viewRect.width, 0.0f), true, false);
        GUI.EndScrollView();

        GUI.BeginScrollView(new Rect(position.x, position.y + 13.0f, position.width, position.height - 13.0f), scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);

        return scrollPosition;
    }
}
