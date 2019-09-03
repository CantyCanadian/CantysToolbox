///====================================================================================================
///
///     NoiseUtil by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty
{
    public static class NoiseUtil
    {
        //https://www.redblobgames.com/articles/noise/introduction.html

        private static RandomNumberGenerator s_RNG = new RandomNumberGenerator();
        private static Vector3 s_SineData = null;

        public static void SetSeed(int seed)
        {
            s_RNG = new RandomNumberGenerator(seed);
        }

        /// <summary>
        /// Generates a new Sine wave and returns its first value. If you only want the next value, call SineNext.
        /// </summary>
        /// <param name="frequency">How many values is there in a full sine wave (times you call SineNext()).</param>
        /// <param name="phase">Horizontal deviation of the sine wave to add a bit of randomness.</param>
        public static float SineNew(uint frequency = 50, float phase = s_RNG.Next(2 * Mathf.PI))
        {
            s_SineData = new Vector3(frequency, phase, 0);
            return SineNext();
        }

        /// <summary>
        /// Gets the next value in the generated sine wave. If no sine wave exists, it will create a new one using default values.
        /// </summary>
        public static float SineNext()
        {
            if (s_SineData == null)
            {
                s_SineData = new Vector2(frequency, s_RNG.Next(2 * Mathf.PI), 0);
            }

            float result = (0.5f * Mathf.sin(2 * Mathf.PI * s_SineData.z + s_SineData.y)) + 0.5f;
            s_SineData.z += (1 / s_SineData.x);

            return result;
        }

        /// <summary>
        /// Every values are random between 0.0 and 1.0 .
        /// </summary>
        public static float WhiteNoise()
        {
            return s_RNG.Next();
        }
        
        /// <summary>
        /// Smoothes out the result by averaging the random value with its neighbors. Gives less variation between values.
        /// </summary>
        public static float BrownNoise()
        {
            float value = (s_RNG.Next() + s_RNG.Next()) / 2.0f;
            s_RNG.Back();
            return value;
        }

        /// <summary>
        /// Forces extra variation onto the noise by using the value's difference from its neighbors. Gives a less stable result.
        /// </summary>
        /// <returns></returns>
        public static float BlueNoise()
        {
            float value = (s_RNG.Next() - s_RNG.Next()) / 2.0f;
            s_RNG.Back();
            return value;
        }
    }
}