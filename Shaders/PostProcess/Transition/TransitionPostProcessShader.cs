///====================================================================================================
///
///     TransitionPostProcessShader by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty.Shaders
{
    /// <summary>
    /// Class to use when using transition post-process shaders. Includes extra content for easy editing and timing management.
    /// </summary>
    [ExecuteInEditMode]
    public class TransitionPostProcessShader : PostProcessShaderBase
    {
        public Material ShaderMaterial;

        public CurveTimer TransitionTimer;

        [Range(0.0f, 1.0f)] public float TransitionValue;

        public void Play(bool backwards = false)
        {
            TransitionTimer.Play(backwards);
        }

        public void Update()
        {
            if (TransitionTimer.isPlaying)
            {
                TransitionValue = TransitionTimer.Value;
            }
        }

        public override void Blit(RenderTexture src, RenderTexture dst)
        {
            ShaderMaterial.SetFloat("_TransitionValue", TransitionValue);
            ShaderMaterial.SetVector("_ScreenResolution", new Vector2(Screen.width, Screen.height));

            Graphics.Blit(src, dst, ShaderMaterial);
        }
    }
}