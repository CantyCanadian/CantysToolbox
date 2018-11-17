using UnityEditor;
using UnityEngine;
using Canty.Input;

namespace Canty.Editors
{
    public class InputEditor : EditorWindow
    {
        static private Vector2 s_EditorWindowSize = new Vector2(1000.0f, 1000.0f);

        private InputData m_DataSet;

        [MenuItem("Tool/Input Editor")]
        public static void ShowWindow()
        {
            EditorWindow mergerWindow = GetWindow<InputEditor>();
            mergerWindow.maxSize = new Vector2(1000.0f, 1000.0f);
            mergerWindow.minSize = mergerWindow.maxSize;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal(GUILayout.Height(35.0f));
                {
                    EditorGUILayout.LabelField("Input Manager", GUIUtil.TitleStyle, GUILayout.ExpandHeight(true));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginArea(new Rect(s_EditorWindowSize.x * 0.3f, 33.0f, s_EditorWindowSize.x * 0.4f, 30.0f));
                {
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                }
                GUILayout.EndArea();

                GUILayout.BeginHorizontal(GUILayout.Height(30.0f), GUILayout.Width(s_EditorWindowSize.x * 0.69f));
                {
                    GUILayout.Space(s_EditorWindowSize.x * 0.31f);

                    m_DataSet = (InputData)EditorGUILayout.ObjectField(m_DataSet, typeof(InputData), false, GUILayout.ExpandWidth(true));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(30.0f));
                {
                    GUILayout.Space(s_EditorWindowSize.x * 0.01f);
                    GUILayout.Box("", GUILayout.Width(s_EditorWindowSize.x * 0.98f), GUILayout.ExpandHeight(true));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}