using UnityEngine;

[ExecuteInEditMode]
public class CutoutPostProcessShader : MonoBehaviour
{
    public Material ShaderMaterial;
    public Texture CutoutMask;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        ShaderMaterial.SetTexture("_Mask", CutoutMask);
        Graphics.Blit(src, dst, ShaderMaterial);
    }
}
