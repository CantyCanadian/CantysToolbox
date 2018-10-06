///====================================================================================================
///
///     PostProcessShaderStack by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.Shaders
{
    /// <summary>
    /// Basic class to use when using multiple post-process shaders.
    /// </summary>
    [ExecuteInEditMode]
    public class PostProcessShaderStack : PostProcessShaderBase
    {
        public Material[] ShaderMaterial;

        public override void Blit(RenderTexture src, RenderTexture dst)
        {
            bool flip = false;
            foreach (Material material in ShaderMaterial)
            {
                if (flip)
                {
                    Graphics.Blit(dst, src, material);
                    flip = false;
                }
                else
                {
                    Graphics.Blit(src, dst, material);
                    flip = true;
                }
            }

            if (flip == false)
            {
                Graphics.Blit(src, dst);
            }
        }
    }
}