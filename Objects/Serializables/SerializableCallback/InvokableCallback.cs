///====================================================================================================
///
///     InvokableCallback by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

using System;

namespace Canty.Serializable
{
    public class InvokableCallback<TReturn> : InvokableCallbackBase<TReturn>
    {
        public Func<TReturn> Function;

        public TReturn Invoke()
        {
            return Function();
        }

        public override TReturn Invoke(params object[] args)
        {
            return Function();
        }

        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Function = () => default(TReturn);
            }
            else
            {
                Function = (System.Func<TReturn>) System.Delegate.CreateDelegate(typeof(System.Func<TReturn>), target,
                    methodName);
            }
        }
    }

    public class InvokableCallback<T0, TReturn> : InvokableCallbackBase<TReturn>
    {
        public Func<T0, TReturn> Function;

        public TReturn Invoke(T0 arg0)
        {
            return Function(arg0);
        }

        public override TReturn Invoke(params object[] args)
        {
            return Function((T0) args[0]);
        }

        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Function = x => default(TReturn);
            }
            else
            {
                Function = (System.Func<T0, TReturn>) System.Delegate.CreateDelegate(typeof(System.Func<T0, TReturn>),
                    target, methodName);
            }
        }
    }

    public class InvokableCallback<T0, T1, TReturn> : InvokableCallbackBase<TReturn>
    {
        public Func<T0, T1, TReturn> Function;

        public TReturn Invoke(T0 arg0, T1 arg1)
        {
            return Function(arg0, arg1);
        }

        public override TReturn Invoke(params object[] args)
        {
            return Function((T0) args[0], (T1) args[1]);
        }

        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Function = (x, y) => default(TReturn);
            }
            else
            {
                Function = (System.Func<T0, T1, TReturn>) System.Delegate.CreateDelegate(
                    typeof(System.Func<T0, T1, TReturn>), target, methodName);
            }
        }
    }

    public class InvokableCallback<T0, T1, T2, TReturn> : InvokableCallbackBase<TReturn>
    {
        public Func<T0, T1, T2, TReturn> Function;

        public TReturn Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            return Function(arg0, arg1, arg2);
        }

        public override TReturn Invoke(params object[] args)
        {
            return Function((T0) args[0], (T1) args[1], (T2) args[2]);
        }

        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Function = (x, y, z) => default(TReturn);
            }
            else
            {
                Function = (System.Func<T0, T1, T2, TReturn>) System.Delegate.CreateDelegate(
                    typeof(System.Func<T0, T1, T2, TReturn>), target, methodName);
            }
        }
    }

    public class InvokableCallback<T0, T1, T2, T3, TReturn> : InvokableCallbackBase<TReturn>
    {
        public Func<T0, T1, T2, T3, TReturn> Function;

        public TReturn Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            return Function(arg0, arg1, arg2, arg3);
        }

        public override TReturn Invoke(params object[] args)
        {
            return Function((T0) args[0], (T1) args[1], (T2) args[2], (T3) args[3]);
        }

        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                Function = (x, y, z, w) => default(TReturn);
            }
            else
            {
                Function = (System.Func<T0, T1, T2, T3, TReturn>) System.Delegate.CreateDelegate(
                    typeof(System.Func<T0, T1, T2, T3, TReturn>), target, methodName);
            }
        }
    }
}