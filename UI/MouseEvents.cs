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
        public UnityEvent OnSelectEvent;
        public UnityEvent OnDeselectEvent;

        // When the selected object is updated.
        public UnityEvent OnUpdateSelectedEvent;

        // Right before a drag, whether the drag will work or not doesn't matter.
        public UnityEvent OnInitializePotentialDragEvent;

        // When dragging an object.
        public UnityEvent OnBeginDragEvent;
        public UnityEvent OnDragEvent;
        public UnityEvent OnEndDragEvent;

        // When dropping an object upon it.
        public UnityEvent OnDropEvent;

        // When using key events.
        public UnityEvent OnSubmitEvent;
        public UnityEvent OnCancelEvent;

        // When clicking.
        public UnityEvent OnPointerDownEvent;
        public UnityEvent OnPointerClickEvent;
        public UnityEvent OnPointerUpEvent;

        // When using hover.
        public UnityEvent OnPointerEnterEvent;
        public UnityEvent OnPointerExitEvent;

        // When using keys to move the selection.
        public UnityEvent OnMoveEvent;

        // When the mouse wheel scrolls.
        public UnityEvent OnScrollEvent;

        public override void OnSelect(BaseEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnSelect");
            }

            OnSelectEvent.Invoke();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnDeselect");
            }

            OnDeselectEvent.Invoke();
        }

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnUpdateSelected");
            }

            OnUpdateSelectedEvent.Invoke();
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnInitializePotentialDrag");
            }

            OnInitializePotentialDragEvent.Invoke();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnBeginDrag");
            }

            OnBeginDragEvent.Invoke();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnDrag");
            }

            OnDragEvent.Invoke();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnEndDrag");
            }

            OnEndDragEvent.Invoke();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnDrop");
            }

            OnDropEvent.Invoke();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnSubmit");
            }

            OnSubmitEvent.Invoke();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnCancel");
            }

            OnCancelEvent.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnPointerDown");
            }

            OnPointerDownEvent.Invoke();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnPointerClick");
            }

            OnPointerClickEvent.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnPointerUp");
            }

            OnPointerUpEvent.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnPointerEnter");
            }

            OnPointerEnterEvent.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnPointerExit");
            }

            OnPointerExitEvent.Invoke();
        }

        public override void OnMove(AxisEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnMove");
            }

            OnMoveEvent.Invoke();
        }

        public override void OnScroll(PointerEventData eventData)
        {
            if (OutputEvent)
            {
                Debug.Log(transform.name + " : OnScroll");
            }

            OnScrollEvent.Invoke();
        }
    }
}