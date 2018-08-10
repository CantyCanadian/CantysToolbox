using UnityEngine;

[ExecuteInEditMode]
public class CutoutShader : MonoBehaviour
{
    public Material CutoutMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, CutoutMaterial);
    }
}
