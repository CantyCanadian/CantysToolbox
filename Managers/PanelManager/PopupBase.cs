using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupBase<I> : MonoBehaviour
{
    private delegate void SubmitDelegate<I>();

    public SubmitDelegate<I> OnSubmit;
}
