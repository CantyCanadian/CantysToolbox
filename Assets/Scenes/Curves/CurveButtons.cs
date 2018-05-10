using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveButtons : MonoBehaviour
{
    public CurveObject EaseIn;
    public CurveObject EaseOut;
    public CurveObject EaseInOut;
    public CurveObject EaseOutIn;

    public void OnLinear()
    {
        EaseIn.Type = CurveObject.CurveType.Linear;
        EaseOut.Type = CurveObject.CurveType.Linear;
        EaseInOut.Type = CurveObject.CurveType.Linear;
        EaseOutIn.Type = CurveObject.CurveType.Linear;
    }

    public void OnExponential()
    {
        EaseIn.Type = CurveObject.CurveType.Exponential;
        EaseOut.Type = CurveObject.CurveType.Exponential;
        EaseInOut.Type = CurveObject.CurveType.Exponential;
        EaseOutIn.Type = CurveObject.CurveType.Exponential;
    }

    public void OnCircular()
    {
        EaseIn.Type = CurveObject.CurveType.Circular;
        EaseOut.Type = CurveObject.CurveType.Circular;
        EaseInOut.Type = CurveObject.CurveType.Circular;
        EaseOutIn.Type = CurveObject.CurveType.Circular;
    }

    public void OnQuadratic()
    {
        EaseIn.Type = CurveObject.CurveType.Quadratic;
        EaseOut.Type = CurveObject.CurveType.Quadratic;
        EaseInOut.Type = CurveObject.CurveType.Quadratic;
        EaseOutIn.Type = CurveObject.CurveType.Quadratic;
    }

    public void OnSine()
    {
        EaseIn.Type = CurveObject.CurveType.Sine;
        EaseOut.Type = CurveObject.CurveType.Sine;
        EaseInOut.Type = CurveObject.CurveType.Sine;
        EaseOutIn.Type = CurveObject.CurveType.Sine;
    }

    public void OnCubic()
    {
        EaseIn.Type = CurveObject.CurveType.Cubic;
        EaseOut.Type = CurveObject.CurveType.Cubic;
        EaseInOut.Type = CurveObject.CurveType.Cubic;
        EaseOutIn.Type = CurveObject.CurveType.Cubic;
    }

    public void OnQuartic()
    {
        EaseIn.Type = CurveObject.CurveType.Quartic;
        EaseOut.Type = CurveObject.CurveType.Quartic;
        EaseInOut.Type = CurveObject.CurveType.Quartic;
        EaseOutIn.Type = CurveObject.CurveType.Quartic;
    }

    public void OnQuintic()
    {
        EaseIn.Type = CurveObject.CurveType.Quintic;
        EaseOut.Type = CurveObject.CurveType.Quintic;
        EaseInOut.Type = CurveObject.CurveType.Quintic;
        EaseOutIn.Type = CurveObject.CurveType.Quintic;
    }

    public void OnElastic()
    {
        EaseIn.Type = CurveObject.CurveType.Elastic;
        EaseOut.Type = CurveObject.CurveType.Elastic;
        EaseInOut.Type = CurveObject.CurveType.Elastic;
        EaseOutIn.Type = CurveObject.CurveType.Elastic;
    }

    public void OnBounce()
    {
        EaseIn.Type = CurveObject.CurveType.Bounce;
        EaseOut.Type = CurveObject.CurveType.Bounce;
        EaseInOut.Type = CurveObject.CurveType.Bounce;
        EaseOutIn.Type = CurveObject.CurveType.Bounce;
    }

    public void OnBack()
    {
        EaseIn.Type = CurveObject.CurveType.Back;
        EaseOut.Type = CurveObject.CurveType.Back;
        EaseInOut.Type = CurveObject.CurveType.Back;
        EaseOutIn.Type = CurveObject.CurveType.Back;
    }
}
