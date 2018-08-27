using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BarsTransitionPostProcessShader : MonoBehaviour
{
    public Material ShaderMaterial;

    [Range(0.0f, 1.0f)]
    public float TransitionValue;

    public Transitions Transition = Transitions.Horizontal;
    public bool ReverseEffect = false;

    public enum Transitions
    {
        Horizontal,
        Vertical,
        DiagonalUpLeft,
        DiagonalUpRight
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        ShaderMaterial.SetFloat("_ClosedVal", TransitionValue);
        ShaderMaterial.SetInt("_Reverse", ReverseEffect ? 1 : 0);
        Graphics.Blit(src, dst, ShaderMaterial);
    }
}
