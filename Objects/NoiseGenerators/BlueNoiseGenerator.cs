using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public class BlueNoiseGenerator : BaseNoiseGenerator
    {
        private RandomNumberGenerator m_RNG;

        /// <summary>
        /// Generates the next value.
        /// </summary>
        public override float Next()
        {
            float value = Mathf.Abs(m_RNG.NextFloat() - m_RNG.CheckNextFloat());
            return value;
        }

        /// <summary>
        /// Generates a new texture.
        /// </summary>
        public override Texture2D NextTexture(int width, int height)
        {
            Texture2D newTex = new Texture2D(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float next = Next();
                    newTex.SetPixel(x, y, new Color(next, next, next));
                }
            }

            newTex.Apply();

            return newTex;
        }

        /// <summary>
        /// Skips one value forward.
        /// </summary>
        public override void Skip()
        {
            m_RNG.Skip();
        }

        /// <summary>
        /// Skips multiple values forward.
        /// </summary>
        public override void Skip(int steps)
        {
            m_RNG.Skip(steps);
        }

        /// <summary>
        /// Skips one value backwards.
        /// </summary>
        public override void Back()
        {
            m_RNG.Back();
        }

        /// <summary>
        /// Skips multiple balues backwards.
        /// </summary>
        public override void Back(int steps)
        {
            m_RNG.Back(steps);
        }

        /// <summary>
        /// Starts the internal RNG object with its default seed.
        /// </summary>
        public BlueNoiseGenerator()
        {
            m_RNG = new RandomNumberGenerator();
        }

        /// <summary>
        /// Starts the internal RNG object with an overwritten seed.
        /// </summary>
        public BlueNoiseGenerator(int seed)
        {
            m_RNG = new RandomNumberGenerator(seed);
        }
    }
}