using UnityEngine;
using UnityEngine.Events;

namespace Canty.Managers
{
    public abstract class PopupBase : MonoBehaviour
    {
    }

    public abstract class PopupBase<I> : PopupBase
    {
        public UnityEvent<I> OnSubmitWithData;
        public UnityEvent OnSubmit;
    }
}