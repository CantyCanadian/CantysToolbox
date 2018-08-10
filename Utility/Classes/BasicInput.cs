using System;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Dictionary<KeyCode, UnityEvent> KeyEvent;
    
    private void Update()
    {
        foreach(KeyValuePair<KeyCode, Direction> kvp in KeyEvent)
        {
            if (Input.GetKey(kvp.Key))
            {
                kvp.Value.Invoke();
            }
        }
    }
}