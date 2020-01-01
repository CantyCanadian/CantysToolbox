using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// This object allows you to create a special custom noise by compounding multiple noise generators using custom weights.
    /// </summary>
    public class WeightedCompoundNoiseGenerators : BaseNoiseGenerator
    {
        public struct NoiseGeneratorPair
        {
            public NoiseGeneratorPair(BaseNoiseGenerator generator, float weight)
            {
                Generator = generator;
                Weight = weight;
            }

            public BaseNoiseGenerator Generator;
            public float Weight;
        }

        private NoiseGeneratorPair[] m_Pairs;

        /// <summary>
        /// Returns the weighted average of all the given noise pairs (0.0 to 1.0).
        /// </summary>
        /// <returns></returns>
        public override float Next()
        {
            float result = 0.0f;
            float total = 0.0f;

            foreach(NoiseGeneratorPair pair in m_Pairs)
            {
                result += pair.Generator.Next() * pair.Weight;
                total += pair.Weight;
            }

            return result / total;
        }

        /// <summary>
        /// Generates a new texture.
        /// </summary>
        public override Texture2D NextTexture(int width, int height)
        {
            Texture2D newTex = new Texture2D(width, height);

            Dictionary<Texture2D, float> textures = new Dictionary<Texture2D, float>();

            foreach (NoiseGeneratorPair pair in m_Pairs)
            {
                textures.Add(pair.Generator.NextTexture(width, height), pair.Weight);
            }

            float result = 0.0f;
            float total = 0.0f;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result = 0.0f;
                    total = 0.0f;

                    foreach (KeyValuePair<Texture2D, float> tex in textures)
                    {
                        result += tex.Key.GetPixel(x, y).r * tex.Value;
                        total += tex.Value;
                    }

                    result /= total;
                    newTex.SetPixel(x, y, new Color(result, result, result));
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
            foreach (NoiseGeneratorPair pair in m_Pairs)
            {
                pair.Generator.Skip();
            }
        }

        /// <summary>
        /// Skips multiple values forward.
        /// </summary>
        public override void Skip(int steps)
        {
            foreach (NoiseGeneratorPair pair in m_Pairs)
            {
                pair.Generator.Skip(steps);
            }
        }

        /// <summary>
        /// Skips one value backwards.
        /// </summary>
        public override void Back()
        {
            foreach (NoiseGeneratorPair pair in m_Pairs)
            {
                pair.Generator.Back();
            }
        }

        /// <summary>
        /// Skips multiple balues backwards.
        /// </summary>
        public override void Back(int steps)
        {
            foreach (NoiseGeneratorPair pair in m_Pairs)
            {
                pair.Generator.Back(steps);
            }
        }

        public WeightedCompoundNoiseGenerators(params NoiseGeneratorPair[] pairs)
        {
            m_Pairs = pairs;
        }
    }
}