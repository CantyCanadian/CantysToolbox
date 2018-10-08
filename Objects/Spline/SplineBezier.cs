///====================================================================================================
///
///     SplineBezier by
///     - CantyCanadian
///		- CatLikeCoding
///
///====================================================================================================

#if UNITY_EDITOR

using UnityEditor;

#endif

using System;
using UnityEngine;

namespace Canty.Spline
{
    /// <summary>
    /// Core spline class. Should be controlled using the inspector instead of via the class itself.
    /// </summary>
    public class SplineBezier : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private Vector3[] m_Points;
        [SerializeField] [HideInInspector] private BezierControlPointMode[] m_Modes;
        private bool m_Loop;

        public enum BezierControlPointMode
        {
            Free,
            Aligned,
            Mirrored
        }

        public int CurveCount
        {
            get { return (m_Points.Length - 1) / 3; }
        }

        public int ControlPointCount
        {
            get { return m_Points.Length; }
        }

        public bool Loop
        {
            get { return m_Loop; }
            set
            {
                m_Loop = value;
                if (value == true)
                {
                    m_Modes[m_Modes.Length - 1] = m_Modes[0];
                    SetControlPoint(0, m_Points[0]);
                }
            }
        }

        public Vector3 GetControlPoint(int index)
        {
            return m_Points[index];
        }

        public void SetControlPoint(int index, Vector3 point)
        {
            if (index % 3 == 0)
            {
                Vector3 delta = point - m_Points[index];
                if (m_Loop)
                {
                    if (index == 0)
                    {
                        m_Points[1] += delta;
                        m_Points[m_Points.Length - 2] += delta;
                        m_Points[m_Points.Length - 1] = point;
                    }
                    else if (index == m_Points.Length - 1)
                    {
                        m_Points[0] = point;
                        m_Points[1] += delta;
                        m_Points[index - 1] += delta;
                    }
                    else
                    {
                        m_Points[index - 1] += delta;
                        m_Points[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        m_Points[index - 1] += delta;
                    }

                    if (index + 1 < m_Points.Length)
                    {
                        m_Points[index + 1] += delta;
                    }
                }
            }

            m_Points[index] = point;
            EnforceMode(index);
        }

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            m_Modes[modeIndex] = mode;
            if (m_Loop)
            {
                if (modeIndex == 0)
                {
                    m_Modes[m_Modes.Length - 1] = mode;
                }
                else if (modeIndex == m_Modes.Length - 1)
                {
                    m_Modes[0] = mode;
                }
            }

            EnforceMode(index);
        }

        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;

            BezierControlPointMode mode = m_Modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !m_Loop && (modeIndex == 0 || modeIndex == m_Modes.Length - 1))
            {
                return;
            }

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = m_Points.Length - 2;
                }

                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= m_Points.Length)
                {
                    enforcedIndex = 1;
                }
            }
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= m_Points.Length)
                {
                    fixedIndex = 1;
                }

                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = m_Points.Length - 2;
                }
            }

            Vector3 middle = m_Points[middleIndex];
            Vector3 enforcedTangent = middle - m_Points[fixedIndex];

            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, m_Points[enforcedIndex]);
            }

            m_Points[enforcedIndex] = middle + enforcedTangent;
        }

        public void AddCurve()
        {
            Vector3 point = m_Points[m_Points.Length - 1];
            Array.Resize(ref m_Points, m_Points.Length + 3);
            point.x += 1.0f;
            m_Points[m_Points.Length - 3] = point;
            point.x += 1.0f;
            m_Points[m_Points.Length - 2] = point;
            point.x += 1.0f;
            m_Points[m_Points.Length - 1] = point;

            Array.Resize(ref m_Modes, m_Modes.Length + 1);
            m_Modes[m_Modes.Length - 1] = m_Modes[m_Modes.Length - 2];
            EnforceMode(m_Points.Length - 4);

            if (m_Loop)
            {
                m_Points[m_Points.Length - 1] = m_Points[0];
                m_Modes[m_Modes.Length - 1] = m_Modes[0];
                EnforceMode(0);
            }
        }

        public void Reset()
        {
            m_Points = new Vector3[]
            {
                new Vector3(1.0f, 0.0f, 0.0f), new Vector3(2.0f, 0.0f, 0.0f), new Vector3(3.0f, 0.0f, 0.0f),
                new Vector3(4.0f, 0.0f, 0.0f)
            };

            m_Modes = new BezierControlPointMode[] {BezierControlPointMode.Free, BezierControlPointMode.Free};
        }

        public Vector3 GetVelocity(float time)
        {
            int i;
            if (time >= 1f)
            {
                time = 1f;
                i = m_Points.Length - 4;
            }
            else
            {
                time = Mathf.Clamp01(time) * CurveCount;
                i = (int) time;
                time -= i;
                i *= 3;
            }

            return transform.TransformPoint(GetFirstDerivative(m_Points[i], m_Points[i + 1], m_Points[i + 2],
                       m_Points[i + 3], time)) - transform.position;
        }

        public Vector3 GetDirection(float time)
        {
            return GetVelocity(time).normalized;
        }

        public Vector3 GetPoint(float time)
        {
            int i;
            if (time >= 1.0f)
            {
                time = 1.0f;
                i = m_Points.Length - 4;
            }
            else
            {
                time = Mathf.Clamp01(time) * CurveCount;
                i = (int) time;
                time -= i;
                i *= 3;
            }

            return transform.TransformPoint(GetPoint(m_Points[i], m_Points[i + 1], m_Points[i + 2], m_Points[i + 3],
                time));
        }

        public static Vector3 GetPoint(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float time)
        {
            time = Mathf.Clamp01(time);
            float oneMinusT = 1f - time;
            return
                Mathf.Pow(oneMinusT, 3.0f) * point0 + 3.0f * Mathf.Pow(oneMinusT, 2.0f) * time * point1 +
                3.0f * oneMinusT * Mathf.Pow(time, 2.0f) * point2 + Mathf.Pow(time, 3.0f) * point3;
        }

        public static Vector3 GetFirstDerivative(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3,
            float time)
        {
            time = Mathf.Clamp01(time);
            float oneMinusT = 1f - time;
            return 3f * oneMinusT * oneMinusT * (point1 - point0) + 6f * oneMinusT * time * (point2 - point1) +
                   3f * time * time * (point3 - point2);
        }

        public BezierControlPointMode GetControlPointMode(int index)
        {
            return m_Modes[(index + 1) / 3];
        }
    }

    #if UNITY_EDITOR

    [CustomEditor(typeof(SplineBezier))]
    public class SplineBezierInspector : Editor
    {
        private static Color[] s_ModeColors = {Color.white, Color.yellow, Color.cyan};

        private SplineBezier m_Spline;
        private Transform m_HandleTransform;
        private Quaternion m_HandleRotation;

        private const int m_StepsPerCurve = 10;
        private const float m_DirectionScale = 0.5f;

        private const float m_HandleSize = 0.06f;
        private const float m_PickSize = 0.09f;

        private int m_SelectedIndex = -1;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            m_Spline = target as SplineBezier;
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", m_Spline.Loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Spline, "Toggle Loop");
                EditorUtility.SetDirty(m_Spline);
                m_Spline.Loop = loop;
            }

            if (m_SelectedIndex >= 0 && m_SelectedIndex < m_Spline.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }

            if (GUILayout.Button("Add Curve"))
            {
                Undo.RecordObject(m_Spline, "Add Curve");
                m_Spline.AddCurve();
                EditorUtility.SetDirty(m_Spline);
            }
        }

        private void OnSceneGUI()
        {
            m_Spline = target as SplineBezier;
            m_HandleTransform = m_Spline.transform;
            m_HandleRotation = Tools.pivotRotation == PivotRotation.Local
                ? m_HandleTransform.rotation
                : Quaternion.identity;

            Vector3 point0 = ShowPoint(0);
            for (int i = 1; i < m_Spline.ControlPointCount; i += 3)
            {
                Vector3 point1 = ShowPoint(i);
                Vector3 point2 = ShowPoint(i + 1);
                Vector3 point3 = ShowPoint(i + 2);

                Handles.color = Color.gray;
                Handles.DrawLine(point0, point1);
                Handles.DrawLine(point2, point3);

                Handles.DrawBezier(point0, point3, point1, point2, Color.white, null, 2.0f);
                point0 = point3;
            }

            ShowDirections();
        }

        private void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();

            Vector3 point = EditorGUILayout.Vector3Field("    Position", m_Spline.GetControlPoint(m_SelectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Spline, "Move Point");
                EditorUtility.SetDirty(m_Spline);
                m_Spline.SetControlPoint(m_SelectedIndex, point);
            }

            EditorGUI.BeginChangeCheck();
            SplineBezier.BezierControlPointMode mode =
                (SplineBezier.BezierControlPointMode) EditorGUILayout.EnumPopup("Mode",
                    m_Spline.GetControlPointMode(m_SelectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Spline, "Change Point Mode");
                m_Spline.SetControlPointMode(m_SelectedIndex, mode);
                EditorUtility.SetDirty(m_Spline);
            }
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            Vector3 point = m_Spline.GetPoint(0f);
            Handles.DrawLine(point, point + m_Spline.GetDirection(0f) * m_DirectionScale);

            int steps = m_StepsPerCurve * m_Spline.CurveCount;
            for (int i = 1; i <= steps; i++)
            {
                point = m_Spline.GetPoint(i / (float) steps);
                Handles.DrawLine(point, point + m_Spline.GetDirection(i / (float) steps) * m_DirectionScale);
            }
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = m_HandleTransform.TransformPoint(m_Spline.GetControlPoint(index));
            float size = HandleUtility.GetHandleSize(point);
            if (index == 0)
            {
                size *= 2.0f;
            }

            Handles.color = s_ModeColors[(int) m_Spline.GetControlPointMode(index)];
            ;
            if (Handles.Button(point, m_HandleRotation, size * m_HandleSize, size * m_PickSize, Handles.DotHandleCap))
            {
                m_SelectedIndex = index;
                Repaint();
            }

            if (m_SelectedIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, m_HandleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_Spline, "Move Point");
                    EditorUtility.SetDirty(m_Spline);
                    m_Spline.SetControlPoint(index, m_HandleTransform.InverseTransformPoint(point));
                }
            }

            return point;
        }
    }
    #endif
}