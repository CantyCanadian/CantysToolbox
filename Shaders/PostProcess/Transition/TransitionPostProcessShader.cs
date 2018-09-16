using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TransitionPostProcessShader : MonoBehaviour
{
    public Material ShaderMaterial;

    [Range(0.0f, 1.0f)]
    public float TransitionValue;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        ShaderMaterial.SetFloat("_TransitionValue", TransitionValue);
        ShaderMaterial.SetVector("_ScreenResolution", new Vector2(Screen.width, Screen.height));

        Graphics.Blit(src, dst, ShaderMaterial);
    }
}
