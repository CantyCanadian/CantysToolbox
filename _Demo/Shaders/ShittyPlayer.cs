using UnityEngine;
using Canty.Shaders;

public class ShittyPlayer : MonoBehaviour
{
	public void Play()
    {
        TransitionPostProcessShader tpps = GetComponent<TransitionPostProcessShader>();
        if (tpps != null)
        {
            tpps.Play();
        }

        DissolveShader ds = GetComponent<DissolveShader>();
        if (ds != null)
        {
            ds.Play();
        }

        NormalExtrudeShader nes = GetComponent<NormalExtrudeShader>();
        if (nes != null)
        {
            nes.Play();
        }
    }
}
