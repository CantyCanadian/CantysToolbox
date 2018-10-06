using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJumping : MonoBehaviour
{
    public GameObject JumpingObject;
    public KeyCode JumpingKey;
    
    public float JumpingStrength = 1.0f;
    public float Gravity = 9.8f;

    private float m_VerticalVelocity = 0.0f;
    
    private bool m_IsGrounded = true;
    private bool m_IsJumping = false;

	void Update ()
	{
	    if (Input.GetKeyDown(JumpingKey) && m_IsGrounded)
	    {
	        m_IsJumping = true;
	    }
    }

    void FixedUpdate()
    {
        if (m_IsGrounded && m_IsJumping)
        {
            m_VerticalVelocity = JumpingStrength;
            m_IsJumping = false;
        }
        else if (!m_IsGrounded)
        {
            m_VerticalVelocity -= Gravity * Time.fixedDeltaTime;
        }

        transform.position += Vector3.up * m_VerticalVelocity * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Floor")
        {
            m_IsGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        m_IsGrounded = false;
    }
}
