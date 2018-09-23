using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PopupBase : MonoBehaviour { }

public abstract class PopupBase<I> : PopupBase
{
    public UnityEvent<I> OnSubmitWithData;
    public UnityEvent OnSubmit;
}
