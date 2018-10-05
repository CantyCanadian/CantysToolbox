///====================================================================================================
///
///     UIMouseEvents by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Canty.UI
{
    /// <summary>
    /// Basic class that supports two types of mouse pointer enter, stay and exit for quick editing.
    /// </summary>
    public class UIMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool OutputEvent = false;
        public EventTypes EventType;

        public UnityEvent onMouseEnter;
        public UnityEvent onMouseOver;
        public UnityEvent onMouseExit;
        public UnityEvent onMouseDown;
        public UnityEvent onMouseHeld;
        public UnityEvent onMouseUp;

        private bool m_onPointerOver = false;
        private bool m_onMouseDown = false;

        #region Enter

        public void OnMouseEnter()
        {
            if (EventType == EventTypes.Mouse)
            {
                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnMouseEnter");
                }

                onMouseEnter.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (EventType == EventTypes.Pointer)
            {
                m_onPointerOver = true;

                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnPointerEnter");
                }

                onMouseEnter.Invoke();
            }
        }

        #endregion

        #region Over

        public void OnMouseOver()
        {
            if (EventType == EventTypes.Mouse)
            {
                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnMouseOver");
                }

                onMouseOver.Invoke();
            }
        }

        public void OnPointerOver()
        {
            if (EventType == EventTypes.Pointer)
            {
                if (m_onPointerOver)
                {
                    if (OutputEvent)
                    {
                        Debug.Log(transform.name + " : Update / OnPointerOver");
                    }

                    onMouseOver.Invoke();
                }
            }
        }

        #endregion

        #region Exit

        public void OnMouseExit()
        {
            if (EventType == EventTypes.Mouse)
            {
                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnMouseExit");
                }

                onMouseExit.Invoke();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventType == EventTypes.Pointer)
            {
                m_onPointerOver = false;

                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnPointerExit");
                }

                onMouseExit.Invoke();
            }
        }

        #endregion

        #region Down

        public void OnMouseDown()
        {
            if (EventType == EventTypes.Mouse)
            {
                m_onMouseDown = true;

                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnMouseDown");
                }

                onMouseDown.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (EventType == EventTypes.Pointer)
            {
                m_onMouseDown = true;

                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnPointerDown");
                }

                onMouseDown.Invoke();
            }
        }

        #endregion

        #region Held

        public void OnMouseHeld()
        {
            if (EventType == EventTypes.Mouse)
            {
                if (m_onMouseDown)
                {
                    if (OutputEvent)
                    {
                        Debug.Log(transform.name + " : OnMouseHeld");
                    }

                    onMouseHeld.Invoke();
                }
            }
        }

        public void OnPointerHeld()
        {
            if (EventType == EventTypes.Pointer)
            {
                if (m_onMouseDown)
                {
                    if (OutputEvent)
                    {
                        Debug.Log(transform.name + " : OnPointerHeld");
                    }

                    onMouseHeld.Invoke();
                }
            }
        }

        #endregion

        #region Up

        public void OnMouseUp()
        {
            if (EventType == EventTypes.Mouse)
            {
                m_onMouseDown = false;

                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnMouseUp");
                }

                onMouseUp.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (EventType == EventTypes.Pointer)
            {
                m_onMouseDown = false;

                if (OutputEvent)
                {
                    Debug.Log(transform.name + " : OnPointerUp");
                }

                onMouseUp.Invoke();
            }
        }

        #endregion

        public enum EventTypes
        {
            Mouse,
            Pointer
        }

        public void Update()
        {
            OnPointerOver();
            OnMouseHeld();
            OnPointerHeld();
        }
    }
}