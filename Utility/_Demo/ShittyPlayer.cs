using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
