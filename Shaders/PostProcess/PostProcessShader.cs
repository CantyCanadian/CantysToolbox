using UnityEngine;

[ExecuteInEditMode]
public class PostProcessShader : MonoBehaviour
{
    public Material ShaderMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, ShaderMaterial);
    }
}
