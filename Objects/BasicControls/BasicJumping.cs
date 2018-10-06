using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJumping : MonoBehaviour
{
    public GameObject JumpingObject;
    public Collider ObjectRigidbody;
    public KeyCode JumpingKey;

    public float JumpingTime = 1.0f;
    public float JumpingStrength = 1.0f;

    private Coroutine m_JumpingReference;
    private bool m_IsGrounded = true;

	void Update ()
	{
	    if (Input.GetKeyDown(JumpingKey) && m_IsGrounded == true)
	    {
	        if (m_JumpingReference == null)
	        {
	            m_JumpingReference = StartCoroutine(JumpingLoop());
            }
	    }
    }

    private IEnumerator JumpingLoop()
    {
        float delta = 0.0f;

        while (Input.GetKey(JumpingKey))
        {
            delta += Time.deltaTime;

            JumpingObject.transform.position += Vector3.up * JumpingStrength;

            if (delta >= JumpingTime)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitUntil(() => m_IsGrounded);

        m_JumpingReference = null;
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
