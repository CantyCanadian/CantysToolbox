using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMouseCamera : MonoBehaviour
{
    public int PlayerID = 0;
    public Animator PlayerAnimator;

    public GameObject YawObject;
    public GameObject PitchObject;

    public bool YawInLocalSpace = true;
    public bool PitchInLocalSpace = true;

    public float YawSpeed = 1.0f;
    public float PitchSpeed = 1.0f;

    public Vector2 HalfPitchLimits = new Vector2(20.0f, 40.0f);

    private float m_InitialPitch = 0.0f;
    private float m_CurrentPitch = 0.0f;

    private Quaternion m_PitchRotation
    {
        get { return (PitchInLocalSpace ? PitchObject.transform.localRotation : PitchObject.transform.rotation); }
        set
        {
            if (PitchInLocalSpace)
            {
                PitchObject.transform.localRotation = value;
            }
            else
            {
                PitchObject.transform.rotation = value;
            }
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_InitialPitch = (PitchInLocalSpace ? PitchObject.transform.localEulerAngles.x : PitchObject.transform.eulerAngles.x);
        m_CurrentPitch = m_InitialPitch;
    }

	void Update ()
	{
	    float XRotation = Input.GetAxis("Player" + PlayerID.ToString() + "RightJoystickX");

        if (YawInLocalSpace)
	    {
	        YawObject.transform.localEulerAngles += new Vector3(0.0f, XRotation * YawSpeed, 0.0f);
        }
	    else
	    {
	        YawObject.transform.eulerAngles += new Vector3(0.0f, XRotation * YawSpeed, 0.0f);
        }

	    PlayerAnimator.SetFloat("Rotation", XRotation);

        m_CurrentPitch -= Input.GetAxis("Player" + PlayerID.ToString() + "RightJoystickY") * PitchSpeed;
	    m_CurrentPitch = Mathf.Clamp(m_CurrentPitch, m_InitialPitch - HalfPitchLimits.x, m_InitialPitch + HalfPitchLimits.y);
        
        Quaternion rotation = Quaternion.Euler(m_CurrentPitch, 0.0f, 0.0f);

        m_PitchRotation = rotation;
    }
}
