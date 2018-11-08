///====================================================================================================
///
///     MouseEvents by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Canty.UI
{
    /// <summary>
    /// Basic wrapping class around Unity's EventTrigger object. Allows for quick binding of function on various events.
    /// </summary>
    public class MouseEvents : EventTrigger
    {
        public bool OutputEvent = false;

        // When clicking on and off the object.
        public UnityEvent OnSelect;
        public UnityEvent OnDeselect;

        // When the selected object is updated.
        public UnityEvent OnUpdateSelected;

        // Right before a drag, whether the drag will work or not doesn't matter.
        public UnityEvent OnInitializePotentialDrag;

        // When dragging an object.
        public UnityEvent OnBeginDrag;
        public UnityEvent OnDrag;
        public UnityEvent OnEndDrag;

        // When dropping an object upon it.
        public UnityEvent OnDrop;

        // When using key events.
        public UnityEvent OnSubmit;
        public UnityEvent OnCancel;

        // When clicking.
        public UnityEvent OnPointerDown;
        public UnityEvent OnPointerClick;
        public UnityEvent OnPointerUp;

        // When using hover.
        public UnityEvent OnPointerEnter;
        public UnityEvent OnPointerExit;

        // When using keys to move the selection.
        public UnityEvent OnMove;

        // When the mouse wheel scrolls.
        public UnityEvent OnScroll;

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
    }
}