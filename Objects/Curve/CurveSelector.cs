///====================================================================================================
///
///     CurveSelector by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty
{
    [System.Serializable]
    public class CurveSelector
    {
        public enum CurveTypes
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

        public enum CurveProgressions
        {
            EaseIn,
            EaseOut,
            EaseInOut,
            EaseOutIn
        }

        public CurveTypes CurveType;
        public CurveProgressions CurveProgression;

		/// <summary>
		/// Invoke the function selected via the enums.
		/// </summary>
        public float Invoke(float x1, float x2, float delta)
        {
            switch (CurveType)
            {
                case CurveTypes.Linear:
                    return CurvesUtil.Linear(x1, x2, delta);

                case CurveTypes.Exponential:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.ExponentialEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.ExponentialEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.ExponentialEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.ExponentialEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Circular:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.CircularEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.CircularEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.CircularEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.CircularEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Quadratic:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.QuadraticEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.QuadraticEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.QuadraticEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.QuadraticEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Sine:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.SineEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.SineEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.SineEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.SineEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Cubic:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.CubicEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.CubicEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.CubicEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.CubicEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Quartic:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.QuarticEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.QuarticEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.QuarticEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.QuarticEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Quintic:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.QuinticEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.QuinticEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.QuinticEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.QuinticEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Elastic:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.ElasticEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.ElasticEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.ElasticEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.ElasticEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Bounce:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.BounceEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.BounceEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.BounceEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.BounceEaseOutIn(x1, x2, delta);
                    }

                    break;

                case CurveTypes.Back:
                    switch (CurveProgression)
                    {
                        case CurveProgressions.EaseIn:
                            return CurvesUtil.BackEaseIn(x1, x2, delta);

                        case CurveProgressions.EaseOut:
                            return CurvesUtil.BackEaseOut(x1, x2, delta);

                        case CurveProgressions.EaseInOut:
                            return CurvesUtil.BackEaseInOut(x1, x2, delta);

                        case CurveProgressions.EaseOutIn:
                            return CurvesUtil.BackEaseOutIn(x1, x2, delta);
                    }

                    break;
            }

            Debug.Log("Curve Object : Curve Type or Progression not found");
            return x1;
        }

        /// <summary>
        /// Creates a new CurveSelector object with a given curve type and curve progression (if needed be).
        /// </summary>
        public CurveSelector(CurveTypes type, CurveProgressions progression = CurveProgressions.EaseIn)
        {
            CurveType = type;
            CurveProgression = progression;
        }
    }
}