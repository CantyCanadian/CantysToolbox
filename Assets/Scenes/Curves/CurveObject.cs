using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveObject : MonoBehaviour
{
    public CurveType Type = CurveType.Linear;
    public CurveShape Shape = CurveShape.EaseIn;
    public Vector3 PointBDiff;
    public float MovementTime = 2.0f;
    public float WaitTime = 1.0f;

    private float m_Delta = 0.0f;
    private RectTransform m_Transform;
    private Vector3 m_OriginalPosition;

    public enum CurveType
    {
        Linear,
        Exponential,
        Circular,
        Quadratic,
        Sine,
        Cubic,
        Quartic,
        Quintic,
        Elastic,
        Bounce,
        Back
    }

    public enum CurveShape
    {
        EaseIn,
        EaseOut,
        EaseOutIn,
        EaseInOut
    }

	void Start ()
    {
        m_Transform = GetComponent<RectTransform>();
        m_OriginalPosition = m_Transform.position;

        StartCoroutine(MovementLoop());
	}
	
    private IEnumerator MovementLoop()
    {
        while(true)
        {
            while(m_Delta <= MovementTime)
            {
                m_Transform.position = GetCurveResult();
                m_Delta += Time.deltaTime;
                yield return null;
            }

            m_Transform.position = m_OriginalPosition + PointBDiff;
            yield return new WaitForSeconds(WaitTime);
            m_Delta = 0.0f;
        }
    }

    private Vector3 GetCurveResult()
    {
        switch(Type)
        {
            case CurveType.Linear:
                return Curves.Linear(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

            case CurveType.Exponential:
                switch(Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.ExponentialEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.ExponentialEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.ExponentialEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.ExponentialEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Circular:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.CircularEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.CircularEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.CircularEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.CircularEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Quadratic:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.QuadraticEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.QuadraticEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.QuadraticEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.QuadraticEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Sine:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.SineEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.SineEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.SineEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.SineEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Cubic:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.CubicEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.CubicEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.CubicEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.CubicEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Quartic:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.QuarticEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.QuarticEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.QuarticEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.QuarticEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Quintic:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.QuinticEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.QuinticEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.QuinticEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.QuinticEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Elastic:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.ElasticEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.ElasticEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.ElasticEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.ElasticEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Bounce:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.BounceEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.BounceEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.BounceEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.BounceEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;

            case CurveType.Back:
                switch (Shape)
                {
                    case CurveShape.EaseIn:
                        return Curves.BackEaseIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOut:
                        return Curves.BackEaseOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseInOut:
                        return Curves.BackEaseInOut(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);

                    case CurveShape.EaseOutIn:
                        return Curves.BackEaseOutIn(m_OriginalPosition, m_OriginalPosition + PointBDiff, m_Delta / MovementTime);
                }
                break;
        }

        return new Vector3();
    }
}
