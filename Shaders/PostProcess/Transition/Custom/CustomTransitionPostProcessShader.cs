using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomTransitionPostProcessShader : MonoBehaviour
{
    public Material ShaderMaterial;
    public Texture TransitionPattern;

    [Range(0.0f, 1.0f)]
    public float TransitionValue;

    public bool ReverseEffect = false;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        ShaderMaterial.SetFloat("_Transition", ReverseEffect ? TransitionValue : (1.0f - TransitionValue));
        ShaderMaterial.SetTexture("_TransitionPattern", TransitionPattern);
        ShaderMaterial.SetInt("_Reverse", ReverseEffect ? 1 : 0);
        Graphics.Blit(src, dst, ShaderMaterial);
    }
}
