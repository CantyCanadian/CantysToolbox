using UnityEngine;
using Canty;

// Simple script to run the dissolve shader, made by CantyCanadian.
public class NormalExtrudeShader : MonoBehaviour
{
    public CurveTimer ExtrudeTimer;

    private Renderer m_Renderer;

    public void Play(bool backwards = false)
    {
        ExtrudeTimer.Play(backwards);
    }

    private void Update()
    {
        if (ExtrudeTimer.isPlaying)
        {
            m_Renderer.material.SetFloat("_Amount", ExtrudeTimer.Value);
        }
    }

    private void Start ()
    {
        m_Renderer = GetComponent<Renderer>();
	}
}
