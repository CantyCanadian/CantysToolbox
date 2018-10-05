///====================================================================================================
///
///     SerializableCallbackDrawer by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Canty.Serializable
{
    [CustomPropertyDrawer(typeof(SerializableCallbackBase), true)]
    public class SerializableCallbackDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect positionRect, SerializedProperty property, GUIContent label)
        {
            // Indent label
            label.text = " " + label.text;

            GUI.Box(positionRect, "", "flow overlay box");
            positionRect.y += 4;

            // Using BeginProperty / EndProperty on the parent property means that prefab override logic works on the entire property.
            EditorGUI.BeginProperty(positionRect, label, property);

            // Draw label
            Rect position = EditorGUI.PrefixLabel(positionRect, GUIUtility.GetControlID(FocusType.Passive), label);
            Rect targetRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            // Get target
            SerializedProperty targetProp = property.FindPropertyRelative("m_Target");
            object target = targetProp.objectReferenceValue;
            EditorGUI.PropertyField(targetRect, targetProp, GUIContent.none);

            if (target != null)
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;

                // Get method name
                SerializedProperty methodProperty = property.FindPropertyRelative("m_MethodName");
                string methodName = methodProperty.stringValue;

                // Get args
                SerializedProperty argumentProperties = property.FindPropertyRelative("m_Arguments");
                Type[] argumentTypes = GetArgumentTypes(argumentProperties);

                // Get dynamic
                SerializedProperty dynamicProperties = property.FindPropertyRelative("m_Dynamic");
                bool dynamic = dynamicProperties.boolValue;

                // Get active method
                MethodInfo activeMethod = GetMethod(target, methodName, argumentTypes);

                GUIContent methodlabel = new GUIContent("n/a");
                if (activeMethod != null)
                {
                    methodlabel = new GUIContent(PrettifyMethod(activeMethod));
                }
                else if (!string.IsNullOrEmpty(methodName))
                {
                    methodlabel = new GUIContent("Missing (" + PrettifyMethod(methodName, argumentTypes) + ")");
                }

                Rect methodRect = new Rect(positionRect.x, targetRect.max.y + EditorGUIUtility.standardVerticalSpacing,
                    positionRect.width, EditorGUIUtility.singleLineHeight);

                // Method select button
                position = EditorGUI.PrefixLabel(methodRect, GUIUtility.GetControlID(FocusType.Passive),
                    new GUIContent(dynamic ? "Method (dynamic)" : "Method"));
                if (EditorGUI.DropdownButton(position, methodlabel, FocusType.Keyboard))
                {
                    MethodSelector(property);
                }

                if (activeMethod != null && !dynamic)
                {
                    // Arguments
                    ParameterInfo[] activeParameters = activeMethod.GetParameters();
                    Rect argumentRect = new Rect(positionRect.x,
                        methodRect.max.y + EditorGUIUtility.standardVerticalSpacing, positionRect.width,
                        EditorGUIUtility.singleLineHeight);

                    string[] types = new string[argumentProperties.arraySize];
                    for (int i = 0; i < types.Length; i++)
                    {
                        SerializedProperty argumentProperty =
                            argumentProperties.FindPropertyRelative("Array.data[" + i + "]");
                        GUIContent argLabel = new GUIContent(ObjectNames.NicifyVariableName(activeParameters[i].Name));

                        EditorGUI.BeginChangeCheck();
                        switch ((Argument.ArgumentType) argumentProperty.FindPropertyRelative("Type").enumValueIndex)
                        {
                            case Argument.ArgumentType.Bool:
                                EditorGUI.PropertyField(argumentRect,
                                    argumentProperty.FindPropertyRelative("BoolValue"), argLabel);
                                break;
                            case Argument.ArgumentType.Int:
                                EditorGUI.PropertyField(argumentRect, argumentProperty.FindPropertyRelative("IntValue"),
                                    argLabel);
                                break;
                            case Argument.ArgumentType.Float:
                                EditorGUI.PropertyField(argumentRect,
                                    argumentProperty.FindPropertyRelative("FloatValue"), argLabel);
                                break;
                            case Argument.ArgumentType.String:
                                EditorGUI.PropertyField(argumentRect,
                                    argumentProperty.FindPropertyRelative("StringValue"), argLabel);
                                break;
                            case Argument.ArgumentType.Object:
                                EditorGUI.PropertyField(argumentRect,
                                    argumentProperty.FindPropertyRelative("ObjectValue"), argLabel);
                                break;
                        }

                        if (EditorGUI.EndChangeCheck())
                        {
                            property.FindPropertyRelative("m_Dirty").boolValue = true;
                        }

                        argumentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }

                EditorGUI.indentLevel = indent;
            }
            else
            {
                Rect helpBoxRect = new Rect(positionRect.x + 8,
                    targetRect.max.y + EditorGUIUtility.standardVerticalSpacing, positionRect.width - 16,
                    EditorGUIUtility.singleLineHeight);
                string msg = "Call not set. Execution will be slower.";
                EditorGUI.LabelField(helpBoxRect, new GUIContent(msg, msg), "helpBox");
            }

            // Set indent back to what it was
            EditorGUI.EndProperty();
        }

        private class MenuItem
        {
            public GenericMenu.MenuFunction action;
            public string path;
            public GUIContent label;

            public MenuItem(string path, string name, GenericMenu.MenuFunction action)
            {
                this.action = action;
                this.label = new GUIContent(path + '/' + name);
                this.path = path;
            }
        }

        void MethodSelector(SerializedProperty property)
        {
            // Return type constraint
            Type returnType = null;
            // Arg type constraint
            Type[] argumentTypes = new Type[0];

            // Get return type and argument constraints
            SerializableCallbackBase dummy = GetDummyFunction(property);
            Type[] genericTypes = dummy.GetType().BaseType.GetGenericArguments();

            // SerializableEventBase is always void return type
            if (dummy is SerializableEventBase)
            {
                returnType = typeof(void);
                if (genericTypes.Length > 1)
                {
                    argumentTypes = new Type[genericTypes.Length];
                    Array.Copy(genericTypes, argumentTypes, genericTypes.Length);
                }
            }
            else
            {
                if (genericTypes != null && genericTypes.Length > 0)
                {
                    // The last generic argument is the return type
                    returnType = genericTypes[genericTypes.Length - 1];
                    if (genericTypes.Length > 1)
                    {
                        argumentTypes = new Type[genericTypes.Length - 1];
                        Array.Copy(genericTypes, argumentTypes, genericTypes.Length - 1);
                    }
                }
            }

            SerializedProperty targetProp = property.FindPropertyRelative("m_Target");

            List<MenuItem> dynamicItems = new List<MenuItem>();
            List<MenuItem> staticItems = new List<MenuItem>();

            List<Object> targets = new List<Object>() {targetProp.objectReferenceValue};
            if (targets[0] is Component)
            {
                targets = (targets[0] as Component).gameObject.GetComponents<Component>().ToList<Object>();
                targets.Add((targetProp.objectReferenceValue as Component).gameObject);
            }
            else if (targets[0] is GameObject)
            {
                targets = (targets[0] as GameObject).GetComponents<Component>().ToList<Object>();
                targets.Add(targetProp.objectReferenceValue as GameObject);
            }

            for (int c = 0; c < targets.Count; c++)
            {
                Object t = targets[c];
                MethodInfo[] methods = targets[c].GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

                for (int i = 0; i < methods.Length; i++)
                {
                    MethodInfo method = methods[i];

                    // Skip methods with wrong return type
                    if (returnType != null && method.ReturnType != returnType)
                    {
                        continue;
                    }

                    // Skip methods with null return type
                    // if (method.ReturnType == typeof(void)) continue;
                    // Skip generic methods
                    if (method.IsGenericMethod)
                    {
                        continue;
                    }

                    Type[] parameters = method.GetParameters().Select(x => x.ParameterType).ToArray();

                    // Skip methods with more than 4 args
                    if (parameters.Length > 4)
                    {
                        continue;
                    }

                    // Skip methods with unsupported args
                    if (parameters.Any(x => !Argument.IsSupported(x)))
                    {
                        continue;
                    }

                    string methodPrettyName = PrettifyMethod(methods[i]);
                    staticItems.Add(new MenuItem(targets[c].GetType().Name + "/" + methods[i].DeclaringType.Name,
                        methodPrettyName, () => SetMethod(property, t, method, false)));

                    // Skip methods with wrong constrained args
                    if (argumentTypes.Length == 0 || !Enumerable.SequenceEqual(argumentTypes, parameters))
                    {
                        continue;
                    }

                    dynamicItems.Add(new MenuItem(targets[c].GetType().Name + "/" + methods[i].DeclaringType.Name,
                        methods[i].Name, () => SetMethod(property, t, method, true)));
                }
            }

            // Construct and display context menu
            GenericMenu menu = new GenericMenu();
            if (dynamicItems.Count > 0)
            {
                string[] paths = dynamicItems.GroupBy(x => x.path).Select(x => x.First().path).ToArray();
                foreach (string path in paths)
                {
                    menu.AddItem(new GUIContent(path + "/Dynamic " + PrettifyTypes(argumentTypes)), false, null);
                }

                for (int i = 0; i < dynamicItems.Count; i++)
                {
                    menu.AddItem(dynamicItems[i].label, false, dynamicItems[i].action);
                }

                foreach (string path in paths)
                {
                    menu.AddItem(new GUIContent(path + "/  "), false, null);
                    menu.AddItem(new GUIContent(path + "/Static parameters"), false, null);
                }
            }

            for (int i = 0; i < staticItems.Count; i++)
            {
                menu.AddItem(staticItems[i].label, false, staticItems[i].action);
            }

            if (menu.GetItemCount() == 0)
            {
                menu.AddDisabledItem(new GUIContent("No methods with return type '" + GetTypeName(returnType) + "'"));
            }

            menu.ShowAsContext();
        }

        string PrettifyMethod(string methodName, Type[] parameterTypes)
        {
            string parameterNames = PrettifyTypes(parameterTypes);
            return methodName + "(" + parameterNames + ")";
        }

        string PrettifyMethod(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            ParameterInfo[] parameters = methodInfo.GetParameters();
            string parameterNames = PrettifyTypes(parameters.Select(x => x.ParameterType).ToArray());
            return GetTypeName(methodInfo.ReturnParameter.ParameterType) + " " + methodInfo.Name + "(" +
                   parameterNames + ")";
        }

        string PrettifyTypes(Type[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            return string.Join(", ", types.Select(x => GetTypeName(x)).ToArray());
        }

        MethodInfo GetMethod(object target, string methodName, Type[] types)
        {
            MethodInfo activeMethod = target.GetType().GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null, CallingConventions.Any, types,
                null);
            return activeMethod;
        }

        private Type[] GetArgumentTypes(SerializedProperty argProp)
        {
            Type[] types = new Type[argProp.arraySize];
            for (int i = 0; i < argProp.arraySize; i++)
            {
                types[i] = Argument.RealType((Argument.ArgumentType) argProp
                    .FindPropertyRelative("Array.data[" + i + "].ArgumentTypes").enumValueIndex);
            }

            return types;
        }

        private void SetMethod(SerializedProperty property, UnityEngine.Object target, MethodInfo methodInfo,
            bool dynamic)
        {
            SerializedProperty targetProperty = property.FindPropertyRelative("m_Target");
            targetProperty.objectReferenceValue = target;

            SerializedProperty methodProperty = property.FindPropertyRelative("m_MethodName");
            methodProperty.stringValue = methodInfo.Name;

            SerializedProperty dynamicProperty = property.FindPropertyRelative("m_Dynamic");
            dynamicProperty.boolValue = dynamic;

            SerializedProperty argumentProperty = property.FindPropertyRelative("m_Arguments");

            ParameterInfo[] parameters = methodInfo.GetParameters();

            argumentProperty.arraySize = parameters.Length;
            for (int i = 0; i < parameters.Length; i++)
            {
                argumentProperty.FindPropertyRelative("Array.data[" + i + "].ArgumentTypes").enumValueIndex =
                    (int) Argument.FromRealType(parameters[i].ParameterType);
            }

            property.FindPropertyRelative("m_Dirty").boolValue = true;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private static string GetTypeName(Type t)
        {
            if (t == typeof(int))
            {
                return "int";
            }
            else if (t == typeof(float))
            {
                return "float";
            }
            else if (t == typeof(string))
            {
                return "string";
            }
            else if (t == typeof(bool))
            {
                return "bool";
            }
            else if (t == typeof(void))
            {
                return "void";
            }
            else
            {
                return t.Name;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineheight = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;

            SerializedProperty targetProperty = property.FindPropertyRelative("m_Target");
            SerializedProperty argumentProperties = property.FindPropertyRelative("m_Arguments");
            SerializedProperty dynamicProperty = property.FindPropertyRelative("m_Dynamic");

            float height = lineheight + lineheight;
            if (targetProperty.objectReferenceValue != null && !dynamicProperty.boolValue)
            {
                height += argumentProperties.arraySize * lineheight;
            }

            height += 8;
            return height;
        }

        private static SerializableCallbackBase GetDummyFunction(SerializedProperty prop)
        {
            string stringValue = prop.FindPropertyRelative("m_TypeName").stringValue;
            Type type = Type.GetType(stringValue, false);

            SerializableCallbackBase result;
            if (type == null)
            {
                return null;
            }
            else
            {
                result = (Activator.CreateInstance(type) as SerializableCallbackBase);
            }

            return result;
        }
    }
}

#endif