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
        private static int s_Seed = 23487;

        public static void SetSeed(int seed)
        {
            s_Seed = seed;
        }

        public static float WhiteNoise(float x, float y)
        {
            Random.InitState(s_Seed);

            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    if (i == x && j == y)
                    {
                        return Random.value;
                    }
                    else
                    {
                        Random.value;
                    }
                }
            }
        }
    }
}