///====================================================================================================
///
///     InvokableEvent by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

using System;

namespace Canty.Serializable
{
    public class InvokableEvent : InvokableEventBase
    {
        public Action Action;

        public void Invoke()
        {
            Action();
        }

        public override void Invoke(params object[] args)
        {
            Action();
        }

        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Action = () => { };
            }
            else
            {
                Action = (System.Action) System.Delegate.CreateDelegate(typeof(System.Action), target, methodName);
            }
        }
    }

    public class InvokableEvent<T0> : InvokableEventBase
    {
        public Action<T0> Action;

        public void Invoke(T0 arg0)
        {
            Action(arg0);
        }

        public override void Invoke(params object[] args)
        {
            Action((T0) args[0]);
        }

        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Action = x => { };
            }
            else
            {
                Action = (System.Action<T0>) System.Delegate.CreateDelegate(typeof(System.Action<T0>), target,
                    methodName);
            }
        }
    }

    public class InvokableEvent<T0, T1> : InvokableEventBase
    {
        public Action<T0, T1> Action;

        public void Invoke(T0 arg0, T1 arg1)
        {
            Action(arg0, arg1);
        }

        public override void Invoke(params object[] args)
        {
            Action((T0) args[0], (T1) args[1]);
        }

        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Action = (x, y) => { };
            }
            else
            {
                Action = (System.Action<T0, T1>) System.Delegate.CreateDelegate(typeof(System.Action<T0, T1>), target,
                    methodName);
            }
        }
    }

    public class InvokableEvent<T0, T1, T2> : InvokableEventBase
    {
        public Action<T0, T1, T2> Action;

        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            Action(arg0, arg1, arg2);
        }

        public override void Invoke(params object[] args)
        {
            Action((T0) args[0], (T1) args[1], (T2) args[2]);
        }

        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Action = (x, y, z) => { };
            }
            else
            {
                Action = (System.Action<T0, T1, T2>) System.Delegate.CreateDelegate(typeof(System.Action<T0, T1, T2>),
                    target, methodName);
            }
        }
    }

    public class InvokableEvent<T0, T1, T2, T3> : InvokableEventBase
    {
        public Action<T0, T1, T2, T3> Action;

        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            Action(arg0, arg1, arg2, arg3);
        }

        public override void Invoke(params object[] args)
        {
            Action((T0) args[0], (T1) args[1], (T2) args[2], (T3) args[3]);
        }

        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Action = (x, y, z, w) => { };
            }
            else
            {
                Action = (System.Action<T0, T1, T2, T3>) System.Delegate.CreateDelegate(
                    typeof(System.Action<T0, T1, T2, T3>), target, methodName);
            }
        }
    }
}