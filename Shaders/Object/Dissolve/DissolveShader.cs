using System.Collections;
using UnityEngine;

// Simple script to run the dissolve shader, made by CantyCanadian.
public class DissolveShader : MonoBehaviour
{
    public Curve DissolveCurve;
    public float TimeToDissolve = 2.0f;

    private Renderer m_Renderer;
    private Coroutine m_Coroutine = null;

    public void Play(bool backwards)
    {
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }

        if (backwards)
        {
            m_Coroutine = StartCoroutine(PlayLoop(1.0f, 0.0f));
        }
        else
        {
            m_Coroutine = StartCoroutine(PlayLoop(0.0f, 1.0f));
        }
    }

    private IEnumerator PlayLoop(float v1, float v2)
    {
        float delta = 0.0f;

        while(delta <= TimeToDissolve)
        {
            delta += Time.deltaTime;

            float value = DissolveCurve.Invoke(v1, v2, delta / TimeToDissolve);

            m_Renderer.material.SetFloat("_Dissolve", value);

            yield return null;
        }
    }

	private void Start ()
    {
        m_Renderer = GetComponent<Renderer>();
	}
}
