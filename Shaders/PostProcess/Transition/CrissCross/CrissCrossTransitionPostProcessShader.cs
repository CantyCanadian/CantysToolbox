using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CrissCrossTransitionPostProcessShader : MonoBehaviour
{
    public Material ShaderMaterial;

    public int BarCount;

    [Range(0.0f, 1.0f)]
    public float TransitionValue;

    [Range(0.0f, 180.0f)]
    public float Angle = 0.0f;

    public bool ReverseEffect = false;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        ShaderMaterial.SetFloat("_TransitionValue", ReverseEffect ? 1.0f - TransitionValue : TransitionValue);
        ShaderMaterial.SetInt("_Reverse", ReverseEffect ? 1 : 0);
        ShaderMaterial.SetFloat("_Angle", Angle);

        Graphics.Blit(src, dst, ShaderMaterial);
    }
}
