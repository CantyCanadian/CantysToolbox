using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Canty.Editors
{
    public class AssetBookmark : EditorWindow
    {
        [System.Serializable]
        class SelectedObject
        {
            public Object m_object = null;
            public string m_type = "";
        }

        [SerializeField] SelectedObject m_selectedObject = null;
        [SerializeField] List<SelectedObject> m_selectedObjects = new List<SelectedObject>();

        [SerializeField] Vector2 m_scrollPosition = Vector2.zero;

        [System.NonSerialized] GUIStyle m_eraseButtonStyle;
        [System.NonSerialized] GUIStyle m_typeLabelStyle;

        [MenuItem("Tool/Asset Bookmark")]
        static void ShowWindow()
        {
            GetWindow<AssetBookmark>("ABookmark");
        }

        void OnSelectionChange()
        {
            if (Selection.activeObject == null)
            {
                m_selectedObject = null;
            }
            else if (m_selectedObject == null || m_selectedObject.m_object != Selection.activeObject)
            {
                SelectedObject obj = m_selectedObjects.Find(item => item.m_object == Selection.activeObject);

                m_selectedObject = obj;

                if (obj == null)
                {
                    string type = "";
                    string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

                    if (AssetDatabase.Contains(Selection.activeInstanceID) == false)
                    {
                        type = "In-Scene";
                    }
                    else
                    {
                        int index = assetPath.LastIndexOf('.');

                        if (index == -1)
                        {
                            type = "Folder";
                        }
                        else
                        {
                            string ending = assetPath.Substring(index);

                            switch (ending)
                            {
                                case ".png":
                                case ".jpg":
                                case ".jpeg":
                                    type = "Image";
                                    break;

                                case ".cs":
                                    type = "Script";
                                    break;

                                case ".csv":
                                    type = "CSV";
                                    break;

                                case ".unity":
                                    type = "World";
                                    break;

                                case ".prefab":
                                    type = "Prefab";
                                    break;
                            }
                        }
                    }


                    obj = new SelectedObject()
                    {
                        m_object = Selection.activeObject,
                        m_type = type
                    };

                    m_selectedObject = obj;
                }
            }
        }


        void OnGUI()
        {
            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

            GUILayout.Space(5);

            LayoutAddButton();

            for (int i = m_selectedObjects.Count - 1; i >= 0; --i)
            {
                LayoutItem(i, m_selectedObjects[i]);
            }

            EditorGUILayout.EndScrollView();
        }

        void OnEnable()
        {
            m_selectedObjects.Clear();

            int count = PlayerPrefs.GetInt("ASSETBOOKMARK_ITEMCOUNT");

            for (int i = 0; i < count; i++)
            {
                SelectedObject newObj = new SelectedObject();
                newObj.m_object = AssetDatabase.LoadAssetAtPath(PlayerPrefs.GetString("ASSETBOOKMARK_OBJECT" + i.ToString()), typeof(object));
                newObj.m_type = PlayerPrefs.GetString("ASSETBOOKMARK_TYPE" + i.ToString());

                m_selectedObjects.Add(newObj);
            }
        }

        void OnDisable()
        {
            PlayerPrefs.SetInt("ASSETBOOKMARK_ITEMCOUNT", m_selectedObjects.Count);

            for (int i = 0; i < m_selectedObjects.Count; i++)
            {
                PlayerPrefs.SetString("ASSETBOOKMARK_OBJECT" + i.ToString(), AssetDatabase.GetAssetPath(m_selectedObjects[i].m_object));
                PlayerPrefs.SetString("ASSETBOOKMARK_TYPE" + i.ToString(), m_selectedObjects[i].m_type);
            }
        }

        bool LayoutAddButton()
        {
            GUILayout.Space(5);

            bool add = GUILayout.Button("Add", EditorStyles.miniButton);

            if (add)
            {
                SelectedObject obj = m_selectedObjects.Find(item => item.m_object == Selection.activeObject);

                if (obj == null && m_selectedObject != null)
                {
                    m_selectedObjects.Add(m_selectedObject);
                }
            }

            GUILayout.Space(5);
            return add;
        }

        void LayoutItem(int i, SelectedObject obj)
        {
            if (m_eraseButtonStyle == null)
            {
                m_eraseButtonStyle = new GUIStyle();
                m_eraseButtonStyle.stretchWidth = false;
                m_eraseButtonStyle.margin.top = 3;
                m_eraseButtonStyle.margin.left = 5;
                m_eraseButtonStyle.margin.right = 5;
            }

            if (m_typeLabelStyle == null)
            {
                m_typeLabelStyle = EditorStyles.label;
                m_typeLabelStyle.stretchWidth = false;
                m_typeLabelStyle.margin.top = 3;
                m_typeLabelStyle.margin.left = 5;
                m_typeLabelStyle.margin.right = 5;
                m_typeLabelStyle.alignment = TextAnchor.MiddleCenter;
            }

            GUIStyle style = EditorStyles.miniButtonLeft;
            style.alignment = TextAnchor.MiddleLeft;
            style.stretchWidth = true;

            if (obj != null && obj.m_object != null)
            {
                GUILayout.BeginHorizontal();

                if (obj == m_selectedObject)
                {
                    GUI.enabled = false;
                }

                string objName = obj.m_object.name;

                if (GUILayout.Button(objName, style))
                {
                    m_selectedObject = obj;
                    Selection.activeObject = obj.m_object;
                }

                GUI.enabled = true;

                GUILayout.Label(obj.m_type, m_typeLabelStyle);

                if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"), m_eraseButtonStyle))
                {
                    for (int j = m_selectedObjects.Count - 1; j >= 0; --j)
                    {
                        if (m_selectedObjects[j] == obj)
                        {
                            m_selectedObjects.RemoveAt(j);
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}