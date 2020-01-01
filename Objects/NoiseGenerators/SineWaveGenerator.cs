using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public class SineWaveGenerator : BaseNoiseGenerator
    {
        private uint m_Frequency;
        private int m_Phase;

        /// <summary>
        /// Returns the next value from the sine wave (0.0 to 1.0).
        /// </summary>
        public override float Next()
        {
            float result = Mathf.Sin((1.0f / (m_Frequency / 2.0f)) * Mathf.PI * m_Phase);
            result = (result / 2.0f) + 0.5f;
            m_Phase++;
            return result;
        }

        /// <summary>
        /// Skips one value forward.
        /// </summary>
        public override void Skip()
        {
            m_Phase++;
        }

        /// <summary>
        /// Generates a new texture.
        /// </summary>
        public override Texture2D NextTexture(int width, int height)
        {
            Texture2D newTex = new Texture2D(width, height);
            int phase = m_Phase;

            for (int x = 0; x < width; x++)
            {
                m_Phase = phase;

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
        /// Skips multiple values forward.
        /// </summary>
        public override void Skip(int steps)
        {
            m_Phase += steps;
        }

        /// <summary>
        /// Skips one value backwards.
        /// </summary>
        public override void Back()
        {
            m_Phase--;
        }

        /// <summary>
        /// Skips multiple balues backwards.
        /// </summary>
        public override void Back(int steps)
        {
            m_Phase -= steps;
        }

        /// <summary>
        /// Manually sets the phase to a new value.
        /// </summary>
        public void SetPhase(int phase)
        {
            m_Phase = phase;
        }

        /// <summary>
        /// Creates an object that gives values pertaining to a sine wave using given parameters.
        /// </summary>
        /// <param name="frequency">How many Next calls is needed to finish a whole frequency.</param>
        /// <param name="phase">How many calls are initially skipped.</param>
        public SineWaveGenerator(uint frequency, int phase)
        {
            m_Frequency = frequency;
            m_Phase = phase;
        }
    }
}