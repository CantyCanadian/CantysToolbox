using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Canty;

public class RadialBarShader : MonoBehaviour
{
    public CurveTimer BarTimer;
    public Text PercentText;
    public Image Bars;

    private Image m_Image;

    public void Play(bool backwards = false)
    {
        BarTimer.Play(backwards);
    }

    private void Update()
    {
        if (BarTimer.isPlaying)
        {
            Bars.material.SetFloat("_BarProgress", BarTimer.Value);
            PercentText.text = (Mathf.FloorToInt(BarTimer.Value * 100.0f)).ToString();
        }
    }
}
