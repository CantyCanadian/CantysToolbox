///====================================================================================================
///
///     SplineWalker by
///     - CantyCanadian
///		- CatLikeCoding
///
///====================================================================================================

using UnityEngine;
using Canty.Spline;

namespace Canty
{
    /// <summary>
    /// Basic component that moves an object following a SplineBezier.
    /// </summary>
    public class SplineWalker : MonoBehaviour
    {
        public SplineBezier Spline;
        public float Duration;
        public SplineWalkerModes Mode;
        public bool LookForward;

        private bool goingForward = true;

        private float Progress;

        public enum SplineWalkerModes
        {
            Once,
            Loop,
            PingPong
        }

        private void Update()
        {
            if (goingForward)
            {
                Progress += Time.deltaTime / Duration;
                if (Progress > 1.0f)
                {
                    switch (Mode)
                    {
                        case SplineWalkerModes.Once:
                            Progress = 1.0f;
                            break;

                        case SplineWalkerModes.Loop:
                            Progress -= 1.0f;
                            break;

                        case SplineWalkerModes.PingPong:
                            Progress = 2.0f - Progress;
                            goingForward = false;
                            break;
                    }
                }
            }
            else
            {
                Progress -= Time.deltaTime / Duration;
                if (Progress < 0f)
                {
                    Progress = -Progress;
                    goingForward = true;
                }
            }

            Vector3 position = Spline.GetPoint(Progress);
            transform.localPosition = position;
            if (LookForward)
            {
                transform.LookAt(position + Spline.GetDirection(Progress));
            }
        }
    }
}