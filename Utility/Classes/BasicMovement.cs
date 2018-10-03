using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Basic class to add movement to an object in a versatile way.
    /// </summary>
    public class BasicMovement : MonoBehaviour
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Backward
        }

        public KeyCodeMovementDictionary KeyMovement;
        public bool UseLocalDirection = false;
        public float MovementSpeed = 3.0f;

        private bool m_PreventMovement = false;

        public void SetPreventMovement(bool flag)
        {
            m_PreventMovement = flag;
        }

        private void Update()
        {
            if (!m_PreventMovement)
            {
                Vector3 velocity = new Vector3();

                foreach (KeyValuePair<KeyCode, Direction> kvp in KeyMovement)
                {
                    if (Input.GetKey(kvp.Key))
                    {
                        switch (kvp.Value)
                        {
                            case Direction.Up:
                                velocity.y += MovementSpeed * Time.deltaTime;
                                break;

                            case Direction.Down:
                                velocity.y -= MovementSpeed * Time.deltaTime;
                                break;

                            case Direction.Left:
                                velocity.x -= MovementSpeed * Time.deltaTime;
                                break;

                            case Direction.Right:
                                velocity.x += MovementSpeed * Time.deltaTime;
                                break;

                            case Direction.Forward:
                                velocity.z += MovementSpeed * Time.deltaTime;
                                break;

                            case Direction.Backward:
                                velocity.z -= MovementSpeed * Time.deltaTime;
                                break;
                        }
                    }
                }

                if (UseLocalDirection)
                {
                    Vector3 sideways = transform.right * velocity.x;
                    Vector3 upward = transform.up * velocity.y;
                    Vector3 forward = transform.forward * velocity.z;

                    transform.position += sideways + upward + forward;
                }
                else
                {
                    transform.position += velocity;
                }
            }
        }

        [System.Serializable]
        public class KeyCodeMovementDictionary : SerializableDictionary<KeyCode, Direction>
        {
        }
    }
}