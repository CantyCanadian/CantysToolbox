using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Brown / Red noise is a type of noise that has less high frequencies, which creates a more stable output.
    /// </summary>
    public class RedNoiseGenerator : BaseNoiseGenerator
    {
        private RandomNumberGenerator m_RNG;
        private float m_Last = -1.0f;

        public override float Next()
        {
            float next = m_RNG.NextFloat();
            float value = next + m_RNG.CheckNextFloat();

            if (m_Last == -1.0f)
            {
                m_Last = next;
                value /= 2.0f;
            }
            else
            {
                value += m_Last;
                value /= 3.0f;
                m_Last = next;
            }
            
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
                    float next = m_RNG.NextFloat();
                    newTex.SetPixel(x, y, new Color(next, next, next));
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float next = newTex.GetPixel(x, y).r;
                    next += newTex.GetPixel(x - 1, y).r;
                    next += newTex.GetPixel(x + 1, y).r;
                    next += newTex.GetPixel(x, y - 1).r;
                    next += newTex.GetPixel(x, y + 1).r;
                    next /= 5.0f;

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
        public RedNoiseGenerator()
        {
            m_RNG = new RandomNumberGenerator();
        }

        /// <summary>
        /// Starts the internal RNG object with an overwritten seed.
        /// </summary>
        public RedNoiseGenerator(int seed)
        {
            m_RNG = new RandomNumberGenerator(seed);
        }
    }
}