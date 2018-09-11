using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLifetime : MonoBehaviour, IPoolComponent
{
    public float Lifetime;

    public void Initialize() { }

    public void OnTrigger()
    {
        StartCoroutine(LifetimeLoop());
    }

    public void OnDiscard()
    {
        StopAllCoroutines();
        PoolManager.Instance.DiscardObject(gameObject);
    }

    public IEnumerator LifetimeLoop()
    {
        float timer = 0.0f;
        while(timer < Lifetime)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        OnDiscard();
    }
}
