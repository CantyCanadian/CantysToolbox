using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLifetime : MonoBehaviour
{
    private float m_Lifetime;

    public void Initialize(float lifetime)
    {
        m_Lifetime = lifetime;
    }

    public void Trigger()
    {
        Invoke("Discard", m_Lifetime);
    }

    public void Discard()
    {
        PoolManager.Instance.DiscardObject(gameObject);
    }
}
