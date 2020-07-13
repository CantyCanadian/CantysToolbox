///====================================================================================================
///
///     DisplayInspectorAttribute by
///     - Deadcows
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;
using UnityEditor;

namespace Canty
{
    /// <summary>
    /// Displays an object's inspector from inside another object's inspector.
    /// </summary>
    public class DisplayInspectorAttribute : PropertyAttribute
    {
        public bool DisplayScript;

        public DisplayInspectorAttribute(bool displayScriptField = true)
        {
            DisplayScript = displayScriptField;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DisplayInspectorAttribute))]
    public class DisplayInspectorAttributeDrawer : PropertyDrawer
    {
        private DisplayInspectorAttribute Instance
        {
            get { return m_Instance ?? (m_Instance = attribute as DisplayInspectorAttribute); }
        }
        private DisplayInspectorAttribute m_Instance;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelPosition = position;
            labelPosition.height = EditorGUIUtility.singleLineHeight;

            if (Instance.DisplayScript || property.objectReferenceValue == null)
            {
                position.height = EditorGUI.GetPropertyHeight(property);
                EditorGUI.PropertyField(position, property);
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, "");
                position.y += EditorGUI.GetPropertyHeight(property) + 4;
            }

            if (!Instance.DisplayScript)
            {
                property.isExpanded = true;
            }

            if (property.objectReferenceValue != null && property.isExpanded)
            {
                float startY = position.y;
                float startX = position.x;

                SerializedProperty propertyObject = new SerializedObject(property.objectReferenceValue).GetIterator();
                propertyObject.Next(true);
                propertyObject.NextVisible(false);

                int distance = Instance.DisplayScript ? 10 : 20;
                float xPos = position.x + distance;
                float width = position.width - distance;

                while (propertyObject.NextVisible(propertyObject.isExpanded))
                {
                    position.x = xPos + 10 * propertyObject.depth;
                    position.width = width - 10 * propertyObject.depth;

                    position.height = propertyObject.isExpanded ? 16 : EditorGUI.GetPropertyHeight(propertyObject);
                    EditorGUI.PropertyField(position, propertyObject);
                    position.y += propertyObject.isExpanded ? 20 : EditorGUI.GetPropertyHeight(propertyObject) + 2;
                }

                if (!Instance.DisplayScript)
                {
                    Rect bgRect = new Rect(position);
                    bgRect.y = startY;
                    bgRect.x = startX + 2;
                    bgRect.height = position.y - startY - 5;
                    bgRect.width = 10;
                    DrawColouredRect(bgRect, new Color(.6f, .6f, .8f, .5f));
                }

                if (GUI.changed)
                {
                    propertyObject.serializedObject.ApplyModifiedProperties();
                }
            }

            if (GUI.changed)
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
            {
                return base.GetPropertyHeight(property, label);
            }

            float height = Instance.DisplayScript ? EditorGUI.GetPropertyHeight(property) + 2 : 0;

            if (!property.isExpanded)
            {
                return height - 2;
            }

            SerializedProperty propertyObject = new SerializedObject(property.objectReferenceValue).GetIterator();
            propertyObject.Next(true);
            propertyObject.NextVisible(true);

            while (propertyObject.NextVisible(propertyObject.isExpanded))
            {
                height += propertyObject.isExpanded ? 20 : EditorGUI.GetPropertyHeight(propertyObject) + 2;
            }

            return height;
        }

        private void DrawColouredRect(Rect rect, Color color)
        {
            Color defaultBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(rect, "");
            GUI.backgroundColor = defaultBackgroundColor;
        }
    }
#endif
}