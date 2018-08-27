using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlackCirclePostProcessShader : MonoBehaviour
{
    public Material ShaderMaterial;
    public Vector2 CenterPoint = new Vector2(0.5f, 0.5f);

    [Range(0.0f, 1.0f)]
    public float TransitionValue;

    public bool ReverseEffect = false;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        ShaderMaterial.SetFloat("_PositionX", CenterPoint.x);
        ShaderMaterial.SetFloat("_PositionY", CenterPoint.y);

        float width = Screen.width + (Mathf.Abs(CenterPoint.x * 2.0f - 1.0f) * Screen.width);
        float height = Screen.height + (Mathf.Abs(CenterPoint.y * 2.0f - 1.0f) * Screen.height);
        float radius = 0.5f * Mathf.Sqrt(width * width + height * height);

        ShaderMaterial.SetFloat("_Size", ReverseEffect ? (1.0f - TransitionValue) * radius : TransitionValue * radius);
        ShaderMaterial.SetVector("_ScreenResolution", new Vector2(Screen.width, Screen.height));
        ShaderMaterial.SetInt("_Reverse", ReverseEffect ? 1 : 0);
        Graphics.Blit(src, dst, ShaderMaterial);
    }
}
