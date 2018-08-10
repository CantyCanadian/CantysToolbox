using System;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public Dictionary<KeyCode, Direction> KeyMovement;
    public float MovementSpeed = 3.0f;
    
    private void Update()
    {
        Vector2 movement = new Vector2();

        foreach(KeyValuePair<KeyCode, Direction> kvp in KeyMovement)
        {
            if (Input.GetKey(kvp.Key))
            {
                switch(kvp.Value)
                {
                    case Direction.Up:
                        movement.y += MovementSpeed * Time.deltaTime;
                        break;

                    case Direction.Down:
                        movement.y -= MovementSpeed * Time.deltaTime;
                        break;

                    case Direction.Left:
                        movement.x -= MovementSpeed * Time.deltaTime;
                        break;

                    case Direction.Right:
                        movement.x += MovementSpeed * Time.deltaTime;
                        break;
                }
            }
        }
    }
}