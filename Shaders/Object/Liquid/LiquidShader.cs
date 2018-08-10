using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script required to use the Liquid shader. Original script made by @minionsart on Twitter, velocity math made by @exkise, put together and modified by @cantycanadian.
/// </summary>
public class LiquidShader : MonoBehaviour
{
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Drag = 0f;
    public float WorldScale = 10.0f;

    private Renderer m_Renderer;
    private Vector3 m_LastWorldPosition;
    private Vector3 m_AngularVelocity;
    private Vector3 m_CurrentPosition;
    private Vector3 m_CurrentVelocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        float drag = (1.0f - Drag * deltaTime);

        // Velocity calculations.
        Vector3 worldMovement = (transform.position - m_LastWorldPosition) * WorldScale;
        Vector3 acceleration = new Vector3(-(m_CurrentPosition.x + worldMovement.x), 0f, -(m_CurrentPosition.z + worldMovement.z));

        m_CurrentVelocity = m_CurrentVelocity * drag + acceleration * WobbleSpeed * deltaTime;
        m_CurrentVelocity = m_CurrentVelocity.Clamp(-MaxWobble, MaxWobble);

        m_CurrentPosition += m_CurrentVelocity * deltaTime;

        m_LastWorldPosition = transform.position;

        // Send it to the shader.
        m_Renderer.material.SetFloat("_WobbleX", m_CurrentPosition.x);
        m_Renderer.material.SetFloat("_WobbleZ", m_CurrentPosition.z);
    }
}
