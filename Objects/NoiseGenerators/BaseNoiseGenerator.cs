using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public abstract class BaseNoiseGenerator
    {
        /// <summary>
        /// Generates the next value.
        /// </summary>
        public abstract float Next();

        /// <summary>
        /// Generates a new texture.
        /// </summary>
        public abstract Texture2D NextTexture(int width, int height);

        /// <summary>
        /// Skips one value forward.
        /// </summary>
        public abstract void Skip();

        /// <summary>
        /// Skips multiple values forward.
        /// </summary>
        public abstract void Skip(int steps);

        /// <summary>
        /// Skips one value backwards.
        /// </summary>
        public abstract void Back();

        /// <summary>
        /// Skips multiple balues backwards.
        /// </summary>
        public abstract void Back(int steps);
    }
}