using System;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float secondsToWait = 5.0f;

    private void Update()
    {
        float timer = secondsToWait;

        while(timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }

        Destroy(gameObject);
    }
}