using UnityEngine;
using Canty;

// Simple script to run the dissolve shader, made by CantyCanadian.
public class DissolveShader : MonoBehaviour
{
    public CurveTimer DissolveTimer;

    private Renderer m_Renderer;

    public void Play(bool backwards = false)
    {
        DissolveTimer.Play(backwards);
    }

    private void Update()
    {
        if (DissolveTimer.isPlaying)
        {
            m_Renderer.material.SetFloat("_Dissolve", DissolveTimer.Value);
        }
    }

    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }
}