#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

/// <summary>
/// Display a serialized property in Unity's editor window.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute {}

/// Attribute taken from It3ration on Unity Answers.
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

#endif