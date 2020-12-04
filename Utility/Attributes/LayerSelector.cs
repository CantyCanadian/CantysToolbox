using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Canty
{
    public class LayerSelector : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LayerSelector))]
    class LayerSelectorEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                {
                    property.intValue = EditorGUI.LayerField(position, label, property.intValue);
                }
                break;

                case SerializedPropertyType.String:
                {
                    property.stringValue = LayerMask.LayerToName(EditorGUI.LayerField(position, label, LayerMask.NameToLayer(property.stringValue)));
                }
                break;
            }
        }
    }
#endif
}