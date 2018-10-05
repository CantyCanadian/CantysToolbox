using System;
using UnityEngine;

namespace Canty
{
    /// <summary>A collection of interpolation functions based on Robert Penner's easing equations that works using generic types casted to dynamic variable.
    /// <para>The functions will assume that the passed-in variable type can be used for math equations. If it can't, problems with ensue.</para></summary>
    public class CurvesUtil
    {
        #region Linear

        /// <summary>
        /// Linear interpolation. y = x
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float Linear(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * delta);
        }

        #endregion

        #region Exponential

        /// <summary>
        /// Exponential interpolation with ease-out. y = 2^x
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ExponentialEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (-1 * Mathf.Pow(2, -10 * delta) + 1));
        }

        /// <summary>
        /// Exponential interpolation with ease-in. y = 2^x
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ExponentialEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (Mathf.Pow(2, 10 * (delta - 1))));
        }

        /// <summary>
        /// Exponential interpolation with acceleration near the middle. y = 2^x
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ExponentialEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (Mathf.Pow(2, 10 * (trueX - 1))));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (-1 * Mathf.Pow(2, -10 * trueX) + 1));
            }
        }

        /// <summary>
        /// Exponential interpolation with deceleration near the middle. y = 2^y
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ExponentialEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (-1 * Mathf.Pow(2, -10 * trueX) + 1));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (Mathf.Pow(2, 10 * (trueX - 1))));
            }
        }

        #endregion

        #region Circular

        /// <summary>
        /// Circular interpolation with ease-out. y = sqrt(1 - x^2)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CircularEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (Mathf.Sqrt(1 - Mathf.Pow(delta - 1, 2))));
        }

        /// <summary>
        /// Circular interpolation with ease-in. y = sqrt(1 - x^2)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CircularEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (-1 * Mathf.Sqrt(1 - Mathf.Pow(delta, 2)) + 1));
        }

        /// <summary>
        /// Exponential interpolation with acceleration near the middle. y = sqrt(1 - x^2)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CircularEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (-1 * Mathf.Sqrt(1 - Mathf.Pow(trueX, 2)) + 1));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) +
                              ((x2 - x1) / 2.0f) * (Mathf.Sqrt(1 - Mathf.Pow(trueX - 1, 2))));
            }
        }

        /// <summary>
        /// Exponential interpolation with deceleration near the middle. y = sqrt(1 - x^2)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CircularEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (Mathf.Sqrt(1 - Mathf.Pow(trueX - 1, 2))));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) +
                              ((x2 - x1) / 2.0f) * (-1 * Mathf.Sqrt(1 - Mathf.Pow(trueX, 2)) + 1));
            }
        }

        #endregion

        #region Quadratic

        /// <summary>
        /// Quadratic interpolation with ease-out. y = x^2
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuadraticEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (-1 * delta * (delta - 2)));
        }

        /// <summary>
        /// Quadratic interpolation with ease-in. y = x^2
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuadraticEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (Mathf.Pow(delta, 2.0f)));
        }

        /// <summary>
        /// Quadratic interpolation with acceleration near the middle. y = x^2
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuadraticEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (Mathf.Pow(trueX, 2.0f)));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (-1 * trueX * (trueX - 2)));
            }
        }

        /// <summary>
        /// Quadratic interpolation with deceleration near the middle. y = x^2
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuadraticEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1 : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (-1 * trueX * (trueX - 2)));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (Mathf.Pow(trueX, 2.0f)));
            }
        }

        #endregion

        #region Sine

        /// <summary>
        /// Sine interpolation with ease-out. y = sin(x)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float SineEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (Mathf.Sin(delta * (Mathf.PI / 2.0f))));
        }

        /// <summary>
        /// Sine interpolation with ease-in. y = sin(x)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float SineEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (Mathf.Sin(delta * (Mathf.PI / 2.0f) + (1.5f * Mathf.PI)) + 1.0f));
        }

        /// <summary>
        /// Sine interpolation with acceleration near the middle. y = sin(x)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float SineEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) *
                              (Mathf.Sin(trueX * (Mathf.PI / 2.0f) + (1.5f * Mathf.PI)) + 1.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (Mathf.Sin(trueX * (Mathf.PI / 2.0f))));
            }
        }

        /// <summary>
        /// Sine interpolation with deceleration near the middle. y = sin(x)
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float SineEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (Mathf.Sin(trueX * (Mathf.PI / 2.0f))));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                              (Mathf.Sin(trueX * (Mathf.PI / 2.0f) + (1.5f * Mathf.PI)) + 1.0f));
            }
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Cubic interpolation with ease-out. y = x^3
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CubicEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (Mathf.Pow(delta - 1.0f, 3.0f) + 1.0f));
        }

        /// <summary>
        /// Cubic interpolation with ease-in. y = x^3
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CubicEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * Mathf.Pow(delta, 3.0f));
        }

        /// <summary>
        /// Cubic interpolation with acceleration near the middle. y = x^3
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CubicEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * Mathf.Pow(trueX, 3.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 3.0f) + 1.0f));
            }
        }

        /// <summary>
        /// Cubic interpolation with deceleration near the middle. y = x^3
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float CubicEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 3.0f) + 1.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * Mathf.Pow(trueX, 3.0f));
            }
        }

        #endregion

        #region Quartic

        /// <summary>
        /// Quartic interpolation with ease-out. y = x^4
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuarticEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (-1.0f * Mathf.Pow(delta - 1.0f, 4.0f) + 1.0f));
        }

        /// <summary>
        /// Quartic interpolation with ease-in. y = x^4
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuarticEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * Mathf.Pow(delta, 4.0f));
        }

        /// <summary>
        /// Quartic interpolation with acceleration near the middle. y = x^4
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuarticEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * Mathf.Pow(trueX, 4.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) +
                              ((x2 - x1) / 2.0f) * (-1.0f * Mathf.Pow(trueX - 1.0f, 4.0f) + 1.0f));
            }
        }

        /// <summary>
        /// Quartic interpolation with deceleration near the middle. y = x^4
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuarticEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (-1.0f * Mathf.Pow(trueX - 1.0f, 4.0f) + 1.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * Mathf.Pow(trueX, 4.0f));
            }
        }

        #endregion

        #region Quintic

        /// <summary>
        /// Quintic interpolation with ease-out. y = x^5
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuinticEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (Mathf.Pow(delta - 1.0f, 5.0f) + 1.0f));
        }

        /// <summary>
        /// Quintic interpolation with ease-in. y = x^5
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuinticEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * Mathf.Pow(delta, 5.0f));
        }

        /// <summary>
        /// Quintic interpolation with acceleration near the middle. y = x^5
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuinticEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * Mathf.Pow(trueX, 5.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 5.0f) + 1.0f));
            }
        }

        /// <summary>
        /// Quintic interpolation with deceleration near the middle. y = x^5
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float QuinticEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (Mathf.Pow(trueX - 1.0f, 5.0f) + 1.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * Mathf.Pow(trueX, 5.0f));
            }
        }

        #endregion

        #region Elastic

        /// <summary>
        /// Elastic interpolation with ease-out. y = sin(x) with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ElasticEaseOut(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) *
                          (Mathf.Pow(2.0f, -10.0f * delta) *
                           Mathf.Sin(((delta - 0.07f) * (2.0f * Mathf.PI)) / 0.3f) + 1.0f));
        }

        /// <summary>
        /// Elastic interpolation with ease-in. y = sin(x) with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ElasticEaseIn(float x1, float x2, float delta)
        {
            return x1 + ((x2 - x1) * (-Mathf.Pow(2.0f, 10.0f * (delta - 1.0f)) *
                                         Mathf.Sin(((delta - 1.07f) * (2.0f * Mathf.PI)) / 0.3f)));
        }

        /// <summary>
        /// Elastic interpolation with acceleration near the middle. y = sin(x) with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ElasticEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) *
                              (-Mathf.Pow(2.0f, 10.0f * (trueX - 1.0f)) *
                               Mathf.Sin(((trueX - 1.07f) * (2.0f * Mathf.PI)) / 0.3f)));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                              (Mathf.Pow(2.0f, -10.0f * trueX) *
                               Mathf.Sin(((trueX - 0.07f) * (2.0f * Mathf.PI)) / 0.3f) + 1.0f));
            }
        }

        /// <summary>
        /// Elastic interpolation with deceleration near the middle. y = sin(x) with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float ElasticEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) *
                              (Mathf.Pow(2.0f, -10.0f * trueX) *
                               Mathf.Sin(((trueX - 0.07f) * (2.0f * Mathf.PI)) / 0.3f) + 1.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                              (-Mathf.Pow(2.0f, 10.0f * (trueX - 1.0f)) *
                               Mathf.Sin(((trueX - 1.07f) * (2.0f * Mathf.PI)) / 0.3f)));
            }
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Bounce interpolation with ease-out. y = x^2 with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BounceEaseOut(float x1, float x2, float delta)
        {
            float a = 7.5625f;

            if (delta < (1.0f / 2.75f))
            {
                return x1 + ((x2 - x1) * (a * Mathf.Pow(delta, 2.0f)));
            }
            else if (delta < (2.0f / 2.75f))
            {
                return x1 + ((x2 - x1) * (a * Mathf.Pow(delta - (1.5f / 2.75f), 2.0f) + (3.0f / 4.0f)));
            }
            else if (delta < (2.5f / 2.75f))
            {
                return x1 + ((x2 - x1) * (a * Mathf.Pow(delta - (2.25f / 2.75f), 2.0f) + (15.0f / 16.0f)));
            }
            else
            {
                return x1 + ((x2 - x1) * (a * Mathf.Pow(delta - (2.625f / 2.75f), 2.0f) + (63.0f / 64.0f)));
            }
        }

        /// <summary>
        /// Bounce interpolation with ease-in. y = x^2 with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BounceEaseIn(float x1, float x2, float delta)
        {
            float a = 7.5625f;

            if (delta < (0.25f / 2.75f))
            {
                return x1 + ((x2 - x1) * (a * -Mathf.Pow(delta - (0.125f / 2.75f), 2.0f) + (1.0f / 64.0f)));
            }
            else if (delta < (0.75f / 2.75f))
            {
                return x1 + ((x2 - x1) * (a * -Mathf.Pow(delta - (0.5f / 2.75f), 2.0f) + (1.0f / 16.0f)));
            }
            else if (delta < (1.75f / 2.75f))
            {
                return x1 + ((x2 - x1) * (a * -Mathf.Pow(delta - (1.25f / 2.75f), 2.0f) + (1.0f / 4.0f)));
            }
            else
            {
                return x1 + ((x2 - x1) * (a * -Mathf.Pow(delta - 1.0f, 2.0f) + 1.0f));
            }
        }

        /// <summary>
        /// Bounce interpolation with acceleration near the middle. y = x^2 with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BounceEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 7.5625f;

            if (delta < 0.5f)
            {
                if (trueX < (0.25f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) *
                                  (a * -Mathf.Pow(trueX - (0.125f / 2.75f), 2.0f) + (1.0f / 64.0f)));
                }
                else if (trueX < (0.75f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) *
                                  (a * -Mathf.Pow(trueX - (0.5f / 2.75f), 2.0f) + (1.0f / 16.0f)));
                }
                else if (trueX < (1.75f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) *
                                  (a * -Mathf.Pow(trueX - (1.25f / 2.75f), 2.0f) + (1.0f / 4.0f)));
                }
                else
                {
                    return x1 + (((x2 - x1) / 2.0f) * (a * -Mathf.Pow(trueX - 1.0f, 2.0f) + 1.0f));
                }
            }
            else
            {
                if (trueX < (1.0f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) * (a * Mathf.Pow(trueX, 2.0f)));
                }
                else if (trueX < (2.0f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                                  (a * Mathf.Pow(trueX - (1.5f / 2.75f), 2.0f) + (3.0f / 4.0f)));
                }
                else if (trueX < (2.5f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                                  (a * Mathf.Pow(trueX - (2.25f / 2.75f), 2.0f) + (15.0f / 16.0f)));
                }
                else
                {
                    return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                                  (a * Mathf.Pow(trueX - (2.625f / 2.75f), 2.0f) + (63.0f / 64.0f)));
                }
            }
        }

        /// <summary>
        /// Bounce interpolation with deceleration near the middle. y = x^2 with exponential decay
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BounceEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 7.5625f;

            if (delta < 0.5f)
            {
                if (trueX < (1.0f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) * (a * Mathf.Pow(trueX, 2.0f)));
                }
                else if (trueX < (2.0f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) *
                                  (a * Mathf.Pow(trueX - (1.5f / 2.75f), 2.0f) + (3.0f / 4.0f)));
                }
                else if (trueX < (2.5f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) *
                                  (a * Mathf.Pow(trueX - (2.25f / 2.75f), 2.0f) + (15.0f / 16.0f)));
                }
                else
                {
                    return x1 + (((x2 - x1) / 2.0f) *
                                  (a * Mathf.Pow(trueX - (2.625f / 2.75f), 2.0f) + (63.0f / 64.0f)));
                }
            }
            else
            {
                if (trueX < (0.25f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                                  (a * -Mathf.Pow(trueX - (0.125f / 2.75f), 2.0f) + (1.0f / 64.0f)));
                }
                else if (trueX < (0.75f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                                  (a * -Mathf.Pow(trueX - (0.5f / 2.75f), 2.0f) + (1.0f / 16.0f)));
                }
                else if (trueX < (1.75f / 2.75f))
                {
                    return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                                  (a * -Mathf.Pow(trueX - (1.25f / 2.75f), 2.0f) + (1.0f / 4.0f)));
                }
                else
                {
                    return x1 + (((x2 - x1) / 2.0f) +
                                  ((x2 - x1) / 2.0f) * (a * -Mathf.Pow(trueX - 1.0f, 2.0f) + 1.0f));
                }
            }
        }

        #endregion

        #region Back

        /// <summary>
        /// Back interpolation with ease-out. y = x^3 with overshooting
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BackEaseOut(float x1, float x2, float delta)
        {
            float a = 1.70158f;

            return x1 + ((x2 - x1) * (Mathf.Pow(delta - 1.0f, 2.0f) * ((a + 1.0f) * (delta - 1.0f) + a) + 1.0f));
        }

        /// <summary>
        /// Back interpolation with ease-in. y = x^3 with overshooting
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BackEaseIn(float x1, float x2, float delta)
        {
            float a = 1.70158f;

            return x1 + ((x2 - x1) * (Mathf.Pow(delta, 2.0f) * ((a + 1.0f) * delta - a)));
        }

        /// <summary>
        /// Back interpolation with acceleration near the middle. y = x^3 with overshooting
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BackEaseInOut(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 1.70158f;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) * (Mathf.Pow(trueX, 2.0f) * ((a + 1.0f) * trueX - a)));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) + ((x2 - x1) / 2.0f) *
                              (Mathf.Pow(trueX - 1.0f, 2.0f) * ((a + 1.0f) * (trueX - 1.0f) + a) + 1.0f));
            }
        }

        /// <summary>
        /// Back interpolation with deceleration near the middle. y = x^3 with overshooting
        /// </summary>
        /// <param name="x1">Value at 0.</param>
        /// <param name="x2">Value at 1.</param>
        /// <param name="delta">Interpolation percentage. [0, 1]</param>
        /// <returns>Interpolated value.</returns>
        public static float BackEaseOutIn(float x1, float x2, float delta)
        {
            float trueX = delta * 2.0f;
            trueX = trueX >= 1.0f ? trueX - 1.0f : trueX;

            float a = 1.70158f;

            if (delta < 0.5f)
            {
                return x1 + (((x2 - x1) / 2.0f) *
                              (Mathf.Pow(trueX - 1.0f, 2.0f) * ((a + 1.0f) * (trueX - 1.0f) + a) + 1.0f));
            }
            else
            {
                return x1 + (((x2 - x1) / 2.0f) +
                              ((x2 - x1) / 2.0f) * (Mathf.Pow(trueX, 2.0f) * ((a + 1.0f) * trueX - a)));
            }
        }

        #endregion
    }
}
