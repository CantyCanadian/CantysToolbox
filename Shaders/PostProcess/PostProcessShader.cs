///====================================================================================================
///
///     PostProcessShader by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.Shaders
{
    /// <summary>
    /// Basic class to use when using post-process shaders. Use this one unless specified otherwise in the shader file.
    /// </summary>
    [ExecuteInEditMode]
    public class PostProcessShader : MonoBehaviour
    {
        public Material ShaderMaterial;

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            Graphics.Blit(src, dst, ShaderMaterial);
        }
    }
}