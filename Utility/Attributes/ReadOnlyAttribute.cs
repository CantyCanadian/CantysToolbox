///====================================================================================================
///
///     ReadOnlyAttribute by
///     - CantyCanadian
///
///====================================================================================================

<<<<<<< HEAD
=======
#if UNITY_EDITOR

>>>>>>> 01b738a6f3536b6b71010891aa683b207d2cd928
using UnityEngine;
using UnityEditor;

namespace Canty
{
    /// <summary>
<<<<<<< HEAD
    /// Display a serialized property in Unity's editor window in a non-modifiable way.
=======
    /// Display a serialized property in Unity's editor window.
>>>>>>> 01b738a6f3536b6b71010891aa683b207d2cd928
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }

<<<<<<< HEAD
#if UNITY_EDITOR
=======
    /// Attribute taken from It3ration on Unity Answers.
>>>>>>> 01b738a6f3536b6b71010891aa683b207d2cd928
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
<<<<<<< HEAD
#endif
}
=======
}

#endif
>>>>>>> 01b738a6f3536b6b71010891aa683b207d2cd928
