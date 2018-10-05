using UnityEngine;
using Canty;

public class ShittyAtlas : MonoBehaviour
{
    public CurveTimer AnimationTimer;
    public int MaxIndex = 25;

    private Renderer m_Renderer;

    public void Play(bool backwards = false)
    {
        AnimationTimer.Play(backwards);
    }

    private void Update()
    {
        if (AnimationTimer.isPlaying)
        {
            m_Renderer.material.SetFloat("_CurrentIndex", Mathf.FloorToInt((AnimationTimer.Value / AnimationTimer.CurveTime) * (MaxIndex - 1)));
        }
    }

    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }
}
