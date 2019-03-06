///====================================================================================================
///
///     SerializableDictionaryPropertyDrawer by
///     - CantyCanadian
///		- azixMcAze
///
///====================================================================================================

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace Canty.Serializable
{
    [CustomPropertyDrawer(typeof(SerializableDictionaryBase), true)]
    public class SerializableDictionaryPropertyDrawer : PropertyDrawer
    {
        const string KeysFieldName = "m_Keys";
        const string ValuesFieldName = "m_Values";
        protected const float IndentWidth = 15f;

        static GUIContent s_IconPlus = EditorUtil.IconContent("Toolbar Plus", "Add entry");
        static GUIContent s_IconMinus = EditorUtil.IconContent("Toolbar Minus", "Remove entry");

        static GUIContent s_WarningIconConflict = EditorUtil.IconContent("console.warnicon.sml", "Conflicting key, this entry will be lost");

        static GUIContent s_WarningIconOther = EditorUtil.IconContent("console.infoicon.sml", "Conflicting key");
        static GUIContent s_WarningIconNull = EditorUtil.IconContent("console.warnicon.sml", "Null key, this entry will be lost");
        static GUIStyle s_ButtonStyle = GUIStyle.none;
        static GUIContent s_TempContent = new GUIContent();


        class ConflictState
        {
            public object ConflictKey = null;
            public object ConflictValue = null;
            public int ConflictIndex = -1;
            public int ConflictOtherIndex = -1;
            public bool ConflictKeyPropertyExpanded = false;
            public bool ConflictValuePropertyExpanded = false;
            public float ConflictLineHeight = 0f;
        }

        struct PropertyIdentity
        {
            public PropertyIdentity(SerializedProperty property)
            {
                this.Instance = property.serializedObject.targetObject;
                this.PropertyPath = property.propertyPath;
            }

            public UnityEngine.Object Instance;
            public string PropertyPath;
        }

        static Dictionary<PropertyIdentity, ConflictState> s_ConflictStateDict = new Dictionary<PropertyIdentity, ConflictState>();

        enum Action
        {
            None,
            Add,
            Remove
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            Action buttonAction = Action.None;
            int buttonActionIndex = 0;

            SerializedProperty keyArrayProperty = property.FindPropertyRelative(KeysFieldName);
            SerializedProperty valueArrayProperty = property.FindPropertyRelative(ValuesFieldName);

            ConflictState conflictState = GetConflictState(property);

            if (conflictState.ConflictIndex != -1)
            {
                keyArrayProperty.InsertArrayElementAtIndex(conflictState.ConflictIndex);
                SerializedProperty keyProperty = keyArrayProperty.GetArrayElementAtIndex(conflictState.ConflictIndex);
                SetPropertyValue(keyProperty, conflictState.ConflictKey);
                keyProperty.isExpanded = conflictState.ConflictKeyPropertyExpanded;

                valueArrayProperty.InsertArrayElementAtIndex(conflictState.ConflictIndex);
                SerializedProperty valueProperty = valueArrayProperty.GetArrayElementAtIndex(conflictState.ConflictIndex);
                SetPropertyValue(valueProperty, conflictState.ConflictValue);
                valueProperty.isExpanded = conflictState.ConflictValuePropertyExpanded;
            }

            float buttonWidth = s_ButtonStyle.CalcSize(s_IconPlus).x;

            Rect labelPosition = position;
            labelPosition.height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                labelPosition.xMax -= s_ButtonStyle.CalcSize(s_IconPlus).x;
            }

            EditorGUI.PropertyField(labelPosition, property, label, false);
            
            if (property.isExpanded)
            {
                Rect buttonPosition = position;
                buttonPosition.xMin = buttonPosition.xMax - buttonWidth;
                buttonPosition.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.BeginDisabledGroup(conflictState.ConflictIndex != -1);
                if (GUI.Button(buttonPosition, s_IconPlus, s_ButtonStyle))
                {
                    buttonAction = Action.Add;
                    buttonActionIndex = keyArrayProperty.arraySize;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel++;
                Rect linePosition = position;
                linePosition.y += EditorGUIUtility.singleLineHeight;
                linePosition.xMax -= buttonWidth;

                foreach (EnumerationEntry entry in EnumerateEntries(keyArrayProperty, valueArrayProperty))
                {
                    SerializedProperty keyProperty = entry.keyProperty;
                    SerializedProperty valueProperty = entry.valueProperty;
                    int i = entry.index;

                    float lineHeight = DrawKeyValueLine(keyProperty, valueProperty, linePosition, i);

                    buttonPosition = linePosition;
                    buttonPosition.x = linePosition.xMax;
                    buttonPosition.height = EditorGUIUtility.singleLineHeight;
                    if (GUI.Button(buttonPosition, s_IconMinus, s_ButtonStyle))
                    {
                        buttonAction = Action.Remove;
                        buttonActionIndex = i;
                    }

                    if (i == conflictState.ConflictIndex && conflictState.ConflictOtherIndex == -1)
                    {
                        Rect iconPosition = linePosition;
                        iconPosition.size = s_ButtonStyle.CalcSize(s_WarningIconNull);
                        GUI.Label(iconPosition, s_WarningIconNull);
                    }
                    else if (i == conflictState.ConflictIndex)
                    {
                        Rect iconPosition = linePosition;
                        iconPosition.size = s_ButtonStyle.CalcSize(s_WarningIconConflict);
                        GUI.Label(iconPosition, s_WarningIconConflict);
                    }
                    else if (i == conflictState.ConflictOtherIndex)
                    {
                        Rect iconPosition = linePosition;
                        iconPosition.size = s_ButtonStyle.CalcSize(s_WarningIconOther);
                        GUI.Label(iconPosition, s_WarningIconOther);
                    }

                    linePosition.y += lineHeight;
                }

                EditorGUI.indentLevel--;
            }

            if (buttonAction == Action.Add)
            {
                keyArrayProperty.InsertArrayElementAtIndex(buttonActionIndex);
                valueArrayProperty.InsertArrayElementAtIndex(buttonActionIndex);
            }
            else if (buttonAction == Action.Remove)
            {
                DeleteArrayElementAtIndex(keyArrayProperty, buttonActionIndex);
                DeleteArrayElementAtIndex(valueArrayProperty, buttonActionIndex);
            }

            conflictState.ConflictKey = null;
            conflictState.ConflictValue = null;
            conflictState.ConflictIndex = -1;
            conflictState.ConflictOtherIndex = -1;
            conflictState.ConflictLineHeight = 0f;
            conflictState.ConflictKeyPropertyExpanded = false;
            conflictState.ConflictValuePropertyExpanded = false;

            bool escape = false;
            foreach (EnumerationEntry entry1 in EnumerateEntries(keyArrayProperty, valueArrayProperty))
            {
                SerializedProperty keyProperty1 = entry1.keyProperty;
                int i = entry1.index;
                object keyProperty1Value = GetPropertyValue(keyProperty1);

                if (keyProperty1Value == null)
                {
                    SerializedProperty valueProperty1 = entry1.valueProperty;
                    SaveProperty(keyProperty1, valueProperty1, i, -1, conflictState);
                    DeleteArrayElementAtIndex(valueArrayProperty, i);
                    DeleteArrayElementAtIndex(keyArrayProperty, i);

                    break;
                }

                foreach (EnumerationEntry entry2 in EnumerateEntries(keyArrayProperty, valueArrayProperty, i + 1))
                {
                    SerializedProperty keyProperty2 = entry2.keyProperty;
                    int j = entry2.index;
                    object keyProperty2Value = GetPropertyValue(keyProperty2);

                    if (ComparePropertyValues(keyProperty1Value, keyProperty2Value))
                    {
                        SerializedProperty valueProperty2 = entry2.valueProperty;
                        SaveProperty(keyProperty2, valueProperty2, j, i, conflictState);
                        DeleteArrayElementAtIndex(keyArrayProperty, j);
                        DeleteArrayElementAtIndex(valueArrayProperty, j);

                        escape = true;
                        break;
                    }
                }

                if (escape)
                {
                    break;
                }
            }

            EditorGUI.EndProperty();
        }

        static float DrawKeyValueLine(SerializedProperty keyProperty, SerializedProperty valueProperty, Rect linePosition, int index)
        {
            bool keyCanBeExpanded = CanPropertyBeExpanded(keyProperty);
            bool valueCanBeExpanded = CanPropertyBeExpanded(valueProperty);

            if (!keyCanBeExpanded && valueCanBeExpanded)
            {
                return DrawKeyValueLineExpand(keyProperty, valueProperty, linePosition);
            }
            else
            {
                string keyLabel = keyCanBeExpanded ? ("Key " + index.ToString()) : "";
                string valueLabel = valueCanBeExpanded ? ("Value " + index.ToString()) : "";

                return DrawKeyValueLineSimple(keyProperty, valueProperty, keyLabel, valueLabel, linePosition);
            }
        }

        static float DrawKeyValueLineSimple(SerializedProperty keyProperty, SerializedProperty valueProperty, string keyLabel, string valueLabel, Rect linePosition)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            float labelWidthRelative = labelWidth / linePosition.width;

            float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
            Rect keyPosition = linePosition;
            keyPosition.height = keyPropertyHeight;
            keyPosition.width = labelWidth - IndentWidth;
            EditorGUIUtility.labelWidth = keyPosition.width * labelWidthRelative;
            EditorGUI.PropertyField(keyPosition, keyProperty, TempContent(keyLabel), true);

            float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
            Rect valuePosition = linePosition;
            valuePosition.height = valuePropertyHeight;
            valuePosition.xMin += labelWidth;
            EditorGUIUtility.labelWidth = valuePosition.width * labelWidthRelative;
            EditorGUI.indentLevel--;
            EditorGUI.PropertyField(valuePosition, valueProperty, TempContent(valueLabel), true);
            EditorGUI.indentLevel++;

            EditorGUIUtility.labelWidth = labelWidth;

            return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
        }

        static float DrawKeyValueLineExpand(SerializedProperty keyProperty, SerializedProperty valueProperty, Rect linePosition)
        {
            float labelWidth = EditorGUIUtility.labelWidth;

            float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
            Rect keyPosition = linePosition;
            keyPosition.height = keyPropertyHeight;
            keyPosition.width = labelWidth - IndentWidth;
            EditorGUI.PropertyField(keyPosition, keyProperty, GUIContent.none, true);

            float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
            Rect valuePosition = linePosition;
            valuePosition.height = valuePropertyHeight;
            EditorGUI.PropertyField(valuePosition, valueProperty, GUIContent.none, true);

            EditorGUIUtility.labelWidth = labelWidth;

            return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
        }

        static bool CanPropertyBeExpanded(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Quaternion:
                    return true;
                default:
                    return false;
            }
        }

        static void SaveProperty(SerializedProperty keyProperty, SerializedProperty valueProperty, int index, int otherIndex, ConflictState conflictState)
        {
            conflictState.ConflictKey = GetPropertyValue(keyProperty);
            conflictState.ConflictValue = GetPropertyValue(valueProperty);

            float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
            float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
            float lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);

            conflictState.ConflictLineHeight = lineHeight;
            conflictState.ConflictIndex = index;
            conflictState.ConflictOtherIndex = otherIndex;
            conflictState.ConflictKeyPropertyExpanded = keyProperty.isExpanded;
            conflictState.ConflictValuePropertyExpanded = valueProperty.isExpanded;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float propertyHeight = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                SerializedProperty keysProperty = property.FindPropertyRelative(KeysFieldName);
                SerializedProperty valuesProperty = property.FindPropertyRelative(ValuesFieldName);

                foreach (EnumerationEntry entry in EnumerateEntries(keysProperty, valuesProperty))
                {
                    SerializedProperty keyProperty = entry.keyProperty;
                    SerializedProperty valueProperty = entry.valueProperty;
                    float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
                    float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
                    float lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);
                    propertyHeight += lineHeight;
                }

                ConflictState conflictState = GetConflictState(property);

                if (conflictState.ConflictIndex != -1)
                {
                    propertyHeight += conflictState.ConflictLineHeight;
                }
            }

            return propertyHeight;
        }

        static ConflictState GetConflictState(SerializedProperty property)
        {
            ConflictState conflictState;
            PropertyIdentity propId = new PropertyIdentity(property);
            if (!s_ConflictStateDict.TryGetValue(propId, out conflictState))
            {
                conflictState = new ConflictState();
                s_ConflictStateDict.Add(propId, conflictState);
            }

            return conflictState;
        }

        static Dictionary<SerializedPropertyType, PropertyInfo> s_serializedPropertyValueAccessorsDict;

        static SerializableDictionaryPropertyDrawer()
        {
            Dictionary<SerializedPropertyType, string> serializedPropertyValueAccessorsNameDict =
                new Dictionary<SerializedPropertyType, string>()
                {
                    {SerializedPropertyType.Integer, "intValue"},
                    {SerializedPropertyType.Boolean, "boolValue"},
                    {SerializedPropertyType.Float, "floatValue"},
                    {SerializedPropertyType.String, "stringValue"},
                    {SerializedPropertyType.Color, "colorValue"},
                    {SerializedPropertyType.ObjectReference, "objectReferenceValue"},
                    {SerializedPropertyType.LayerMask, "intValue"},
                    {SerializedPropertyType.Enum, "intValue"},
                    {SerializedPropertyType.Vector2, "vector2Value"},
                    {SerializedPropertyType.Vector3, "vector3Value"},
                    {SerializedPropertyType.Vector4, "vector4Value"},
                    {SerializedPropertyType.Rect, "rectValue"},
                    {SerializedPropertyType.ArraySize, "intValue"},
                    {SerializedPropertyType.Character, "intValue"},
                    {SerializedPropertyType.AnimationCurve, "animationCurveValue"},
                    {SerializedPropertyType.Bounds, "boundsValue"},
                    {SerializedPropertyType.Quaternion, "quaternionValue"},
                };

            Type serializedPropertyType = typeof(SerializedProperty);

            s_serializedPropertyValueAccessorsDict = new Dictionary<SerializedPropertyType, PropertyInfo>();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            foreach (KeyValuePair<SerializedPropertyType, string> kvp in serializedPropertyValueAccessorsNameDict)
            {
                PropertyInfo propertyInfo = serializedPropertyType.GetProperty(kvp.Value, flags);
                s_serializedPropertyValueAccessorsDict.Add(kvp.Key, propertyInfo);
            }
        }

        static GUIContent TempContent(string text)
        {
            s_TempContent.text = text;
            return s_TempContent;
        }

        static void DeleteArrayElementAtIndex(SerializedProperty arrayProperty, int index)
        {
            SerializedProperty property = arrayProperty.GetArrayElementAtIndex(index);
            
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                property.objectReferenceValue = null;
            }

            arrayProperty.DeleteArrayElementAtIndex(index);
        }

        public static object GetPropertyValue(SerializedProperty p)
        {
            PropertyInfo propertyInfo;
            if (s_serializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out propertyInfo))
            {
                return propertyInfo.GetValue(p, null);
            }
            else
            {
                if (p.isArray)
                {
                    return GetPropertyValueArray(p);
                }
                else
                {
                    return GetPropertyValueGeneric(p);
                }
            }
        }

        static void SetPropertyValue(SerializedProperty p, object v)
        {
            PropertyInfo propertyInfo;
            if (s_serializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out propertyInfo))
            {
                propertyInfo.SetValue(p, v, null);
            }
            else
            {
                if (p.isArray)
                {
                    SetPropertyValueArray(p, v);
                }
                else
                {
                    SetPropertyValueGeneric(p, v);
                }
            }
        }

        static object GetPropertyValueArray(SerializedProperty property)
        {
            object[] array = new object[property.arraySize];
            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty item = property.GetArrayElementAtIndex(i);
                array[i] = GetPropertyValue(item);
            }

            return array;
        }

        static object GetPropertyValueGeneric(SerializedProperty property)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            SerializedProperty iterator = property.Copy();
            if (iterator.Next(true))
            {
                SerializedProperty end = property.GetEndProperty();
                do
                {
                    string name = iterator.name;
                    object value = GetPropertyValue(iterator);
                    dict.Add(name, value);
                } while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);
            }

            return dict;
        }

        static void SetPropertyValueArray(SerializedProperty property, object v)
        {
            object[] array = (object[]) v;
            property.arraySize = array.Length;
            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty item = property.GetArrayElementAtIndex(i);
                SetPropertyValue(item, array[i]);
            }
        }

        static void SetPropertyValueGeneric(SerializedProperty property, object v)
        {
            Dictionary<string, object> dict = (Dictionary<string, object>) v;
            SerializedProperty iterator = property.Copy();
            if (iterator.Next(true))
            {
                SerializedProperty end = property.GetEndProperty();
                do
                {
                    string name = iterator.name;
                    SetPropertyValue(iterator, dict[name]);
                } while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);
            }
        }

        static bool ComparePropertyValues(object value1, object value2)
        {
            if (value1 is Dictionary<string, object> && value2 is Dictionary<string, object>)
            {
                Dictionary<string, object> dict1 = (Dictionary<string, object>) value1;
                Dictionary<string, object> dict2 = (Dictionary<string, object>) value2;
                return CompareDictionaries(dict1, dict2);
            }
            else
            {
                return object.Equals(value1, value2);
            }
        }

        static bool CompareDictionaries(Dictionary<string, object> dictionary1, Dictionary<string, object> dictionary2)
        {
            if (dictionary1.Count != dictionary2.Count)
                return false;

            foreach (KeyValuePair<string, object> kvp1 in dictionary1)
            {
                string key1 = kvp1.Key;
                object value1 = kvp1.Value;

                object value2;
                if (!dictionary2.TryGetValue(key1, out value2))
                    return false;

                if (!ComparePropertyValues(value1, value2))
                    return false;
            }

            return true;
        }

        struct EnumerationEntry
        {
            public SerializedProperty keyProperty;
            public SerializedProperty valueProperty;
            public int index;

            public EnumerationEntry(SerializedProperty keyProperty, SerializedProperty valueProperty, int index)
            {
                this.keyProperty = keyProperty;
                this.valueProperty = valueProperty;
                this.index = index;
            }
        }

        static IEnumerable<EnumerationEntry> EnumerateEntries(SerializedProperty keyArrayProperty, SerializedProperty valueArrayProperty, int startIndex = 0)
        {
            if (keyArrayProperty.arraySize > startIndex)
            {
                int index = startIndex;
                SerializedProperty keyProperty = keyArrayProperty.GetArrayElementAtIndex(startIndex);
                SerializedProperty valueProperty = valueArrayProperty.GetArrayElementAtIndex(startIndex);
                SerializedProperty endProperty = keyArrayProperty.GetEndProperty();

                do
                {
                    yield return new EnumerationEntry(keyProperty, valueProperty, index);
                    index++;
                } while (keyProperty.Next(false) && valueProperty.Next(false) &&
                         !SerializedProperty.EqualContents(keyProperty, endProperty));
            }
        }
    }

    [CustomPropertyDrawer(typeof(SerializableDictionaryBase.Storage), true)]
    public class SerializableDictionaryStoragePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.Next(true);
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property.Next(true);
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}

#endif