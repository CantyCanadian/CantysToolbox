///====================================================================================================
///
///     ConditionalFieldAttribute by
///     - Deadcows
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Canty
{
    /// <summary>
    /// Hides the passed in field upon the result of a boolean.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        private readonly string m_PropertyToCheck;
        private readonly bool m_Invert;
        private readonly object m_CompareValue;

        public ConditionalFieldAttribute(string propertyToCheck, bool invert = false, object compareValue = null)
        {
            m_PropertyToCheck = propertyToCheck;
            m_Invert = invert;
            m_CompareValue = compareValue;
        }

#if UNITY_EDITOR
        public bool CheckBehaviourPropertyVisible(MonoBehaviour behaviour, string propertyName)
        {
            if (string.IsNullOrEmpty(m_PropertyToCheck))
            {
                return true;
            }

            SerializedObject so = new SerializedObject(behaviour);
            SerializedProperty property = so.FindProperty(propertyName);

            return CheckPropertyVisible(property);
        }

        public bool CheckPropertyVisible(SerializedProperty property)
        {
            SerializedProperty conditionProperty = FindRelativeProperty(property, m_PropertyToCheck);
            if (conditionProperty == null)
            {
                return true;
            }

            bool isBoolMatch = conditionProperty.propertyType == SerializedPropertyType.Boolean && conditionProperty.boolValue;
            string compareStringValue = m_CompareValue != null ? m_CompareValue.ToString().ToUpper() : "NULL";

            if (isBoolMatch && compareStringValue == "FALSE")
            {
                isBoolMatch = false;
            }

            string conditionPropertyStringValue = AsStringValue(conditionProperty).ToUpper();
            bool objectMatch = compareStringValue == conditionPropertyStringValue;

            bool notVisible = !isBoolMatch && !objectMatch;
            return m_Invert ? notVisible : !notVisible;
        }

        private SerializedProperty FindRelativeProperty(SerializedProperty property, string toGet)
        {
            if (property.depth == 0)
            {
                return property.serializedObject.FindProperty(toGet);
            }

            string path = property.propertyPath.Replace(".Array.data[", "[");
            string[] elements = path.Split('.');

            SerializedProperty nestedProperty = NestedPropertyOrigin(property, elements);

            // if nested property is null = we hit an array property
            if (nestedProperty == null)
            {
                string cleanPath = path.Substring(0, path.IndexOf('['));
                SerializedProperty arrayProp = property.serializedObject.FindProperty(cleanPath);

                if (m_WarningsPool.Contains(arrayProp.exposedReferenceValue))
                {
                    return null;
                }

                UnityEngine.Object target = arrayProp.serializedObject.targetObject;
                string who = string.Format("Property <color=brown>{0}</color> in object <color=brown>{1}</color> caused: ", arrayProp.name, target.name);

                Debug.LogWarning(who + "Array fields is not supported by [ConditionalFieldAttribute]", target);
                m_WarningsPool.Add(arrayProp.exposedReferenceValue);

                return null;
            }

            return nestedProperty.FindPropertyRelative(toGet);
        }

        // For [Serialized] types with [Conditional] fields
        private SerializedProperty NestedPropertyOrigin(SerializedProperty property, string[] elements)
        {
            SerializedProperty parent = null;

            for (int i = 0; i < elements.Length - 1; i++)
            {
                string element = elements[i];
                int index = -1;
                if (element.Contains("["))
                {
                    index = Convert.ToInt32(element.Substring(element.IndexOf("[", StringComparison.Ordinal)).Replace("[", "").Replace("]", ""));
                    element = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));
                }

                parent = i == 0 ? property.serializedObject.FindProperty(element) : parent.FindPropertyRelative(element);

                if (index >= 0)
                {
                    parent = parent.GetArrayElementAtIndex(index);
                }
            }

            return parent;
        }

        private string AsStringValue(SerializedProperty prop)
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.String:
                    return prop.stringValue;

                case SerializedPropertyType.Character:
                case SerializedPropertyType.Integer:
                    if (prop.type == "char")
                    {
                        return Convert.ToChar(prop.intValue).ToString();
                    }
                    return prop.intValue.ToString();

                case SerializedPropertyType.ObjectReference:
                    return prop.objectReferenceValue != null ? prop.objectReferenceValue.ToString() : "null";

                case SerializedPropertyType.Boolean:
                    return prop.boolValue.ToString();

                case SerializedPropertyType.Enum:
                    return prop.enumNames[prop.enumValueIndex];

                default:
                    return string.Empty;
            }
        }

        //This pool is used to prevent spamming with warning messages
        //One message per property
        private readonly HashSet<object> m_WarningsPool = new HashSet<object>();
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldAttributeDrawer : PropertyDrawer
    {
        private ConditionalFieldAttribute Attribute
        {
            get { return m_Attribute ?? (m_Attribute = attribute as ConditionalFieldAttribute); }
        }

        private ConditionalFieldAttribute m_Attribute;

        private bool m_ToShow = true;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            m_ToShow = Attribute.CheckPropertyVisible(property);

            // -2 because the editor seem to add 2 pixels as a default height even when set to 0.
            return m_ToShow ? EditorGUI.GetPropertyHeight(property) : -2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_ToShow)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
#endif
}