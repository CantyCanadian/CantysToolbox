///====================================================================================================
///
///     PostProcessShaderBase by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.Shaders
{
    public abstract class PostProcessShaderBase : MonoBehaviour
    {
        public abstract void Blit(RenderTexture src, RenderTexture dst);

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            Blit(src, dst);
        }
    }
}