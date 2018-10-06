using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEngine;

public class VelocityBobbing : MonoBehaviour
{
    public GameObject BobbingObject;
    public CurveTimer BobbingCurve;
    public Vector3 BobbingAngle;
    public float BobbingStrength;
    public float BobbingHeight;

    private Vector3 m_LastPosition;
    private Vector3 m_DefaultPos;
    private bool m_FirstUpdate = false;

	void Update ()
	{
	    if (m_FirstUpdate == false)
	    {
	        BobbingCurve.Play(true);
            m_DefaultPos = BobbingObject.transform.localPosition;
	        m_FirstUpdate = true;
	        return;
	    }

	    Vector3 difference = transform.localPosition - m_LastPosition;
	    m_LastPosition = transform.localPosition;

        BobbingCurve.SetTimeScale(1.0f + difference.magnitude * BobbingStrength);
	    float bobbingMagnitude = BobbingCurve.Value * BobbingHeight;

	    BobbingObject.transform.localPosition = m_DefaultPos + (BobbingAngle * bobbingMagnitude);
	}
}
