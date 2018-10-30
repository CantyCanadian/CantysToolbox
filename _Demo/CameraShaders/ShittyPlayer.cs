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

        ShittyAtlas sa = GetComponent<ShittyAtlas>();
        if (sa != null)
        {
            sa.Play();
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

        RadialBarShader rbs = GetComponent<RadialBarShader>();
        if (rbs != null)
        {
            rbs.Play();
        }
    }
}
