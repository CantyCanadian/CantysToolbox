using UnityEngine;

namespace Canty.Shaders
{
    /// <summary>
    /// Basic class to use when using post-process shaders. Use this one unless specified otherwise in the shader file.
    /// </summary>
    [ExecuteInEditMode]
    public class PostProcessShader : PostProcessShaderBase
    {
        public Material ShaderMaterial;

        public override void Blit(RenderTexture src, RenderTexture dst)
        {
            Graphics.Blit(src, dst, ShaderMaterial);
        }
    }
}