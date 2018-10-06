///====================================================================================================
///
///     BasicMovement by
///     - CantyCanadian
///
///====================================================================================================

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
        public enum Action
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Backward,
            Sprint
        }

        public KeyCodeActionDictionary KeyMovement;
        public bool UseLocalDirection = false;
        public float MovementSpeed = 3.0f;
        public float SprintMultiplier = 2.0f;

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
                float speed = 1.0f;

                foreach (KeyValuePair<KeyCode, Action> kvp in KeyMovement)
                {
                    if (Input.GetKey(kvp.Key))
                    {
                        switch (kvp.Value)
                        {
                            case Action.Up:
                                velocity.y += 1.0f;
                                break;

                            case Action.Down:
                                velocity.y -= 1.0f;
                                break;

                            case Action.Left:
                                velocity.x -= 1.0f;
                                break;

                            case Action.Right:
                                velocity.x += 1.0f;
                                break;

                            case Action.Forward:
                                velocity.z += 1.0f;
                                break;

                            case Action.Backward:
                                velocity.z -= 1.0f;
                                break;

                            case Action.Sprint:
                                speed = SprintMultiplier;
                                break;
                        }
                    }
                }

                velocity = velocity.normalized * (MovementSpeed * Time.deltaTime);

                if (UseLocalDirection)
                {
                    Vector3 sideways = transform.right * velocity.x * speed;
                    Vector3 upward = transform.up * velocity.y;
                    Vector3 forward = transform.forward * velocity.z * speed;

                    transform.position += sideways + upward + forward;
                }
                else
                {
                    transform.position += velocity;
                }
            }
        }

        [System.Serializable]
        public class KeyCodeActionDictionary : SerializableDictionary<KeyCode, Action>
        {
        }
    }
}