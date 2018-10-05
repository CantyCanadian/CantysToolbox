///====================================================================================================
///
///     SerializableCallback by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

using Canty.Serializable;

namespace Canty
{
    /// <summary>
    /// To use, create a custom implementation locally with set types, serialize it and voila. Ex. [System.Serializable] public class IntEvent : SerializableCallback&lt;int&gt; { }
    /// </summary>
    public abstract class SerializableCallback<TReturn> : SerializableCallbackBase<TReturn>
    {
        public TReturn Invoke()
        {
            if (Function == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableCallback<TReturn> call = Function as InvokableCallback<TReturn>;
                return call.Invoke();
            }
            else
            {
                return Function.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Function = new InvokableCallback<TReturn>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Function = new InvokableCallback<TReturn>(Target, MethodName);
                }
                else
                {
                    Function = GetPersistentMethod();
                }
            }
        }
    }

    /// <summary>
    /// To use, create a custom implementation locally with set types, serialize it and voila. Ex. [System.Serializable] public class IntEvent : SerializableCallback&lt;int, int&gt; { }
    /// </summary>
    public abstract class SerializableCallback<T0, TReturn> : SerializableCallbackBase<TReturn>
    {
        public TReturn Invoke(T0 arg0)
        {
            if (Function == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableCallback<T0, TReturn> call = Function as InvokableCallback<T0, TReturn>;
                return call.Invoke(arg0);
            }
            else
            {
                return Function.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Function = new InvokableCallback<T0, TReturn>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Function = new InvokableCallback<T0, TReturn>(Target, MethodName);
                }
                else
                {
                    Function = GetPersistentMethod();
                }
            }
        }
    }

    /// <summary>
    /// To use, create a custom implementation locally with set types, serialize it and voila. Ex. [System.Serializable] public class IntEvent : SerializableCallback&lt;int, int, int&gt; { }
    /// </summary>
    public abstract class SerializableCallback<T0, T1, TReturn> : SerializableCallbackBase<TReturn>
    {
        public TReturn Invoke(T0 arg0, T1 arg1)
        {
            if (Function == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableCallback<T0, T1, TReturn> call = Function as InvokableCallback<T0, T1, TReturn>;
                return call.Invoke(arg0, arg1);
            }
            else
            {
                return Function.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Function = new InvokableCallback<T0, T1, TReturn>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Function = new InvokableCallback<T0, T1, TReturn>(Target, MethodName);
                }
                else
                {
                    Function = GetPersistentMethod();
                }
            }
        }
    }

    /// <summary>
    /// To use, create a custom implementation locally with set types, serialize it and voila. Ex. [System.Serializable] public class IntEvent : SerializableCallback&lt;int, int, int, int&gt; { }
    /// </summary>
    public abstract class SerializableCallback<T0, T1, T2, TReturn> : SerializableCallbackBase<TReturn>
    {
        public TReturn Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            if (Function == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableCallback<T0, T1, T2, TReturn> call = Function as InvokableCallback<T0, T1, T2, TReturn>;
                return call.Invoke(arg0, arg1, arg2);
            }
            else
            {
                return Function.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Function = new InvokableCallback<T0, T1, T2, TReturn>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Function = new InvokableCallback<T0, T1, T2, TReturn>(Target, MethodName);
                }
                else
                {
                    Function = GetPersistentMethod();
                }
            }
        }
    }

    /// <summary>
    /// To use, create a custom implementation locally with set types, serialize it and voila. Ex. [System.Serializable] public class IntEvent : SerializableCallback&lt;int, int, int, int, int&gt; { }
    /// </summary>
    public abstract class SerializableCallback<T0, T1, T2, T3, TReturn> : SerializableCallbackBase<TReturn>
    {
        public TReturn Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Function == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableCallback<T0, T1, T2, T3, TReturn>
                    call = Function as InvokableCallback<T0, T1, T2, T3, TReturn>;
                return call.Invoke(arg0, arg1, arg2, arg3);
            }
            else
            {
                return Function.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Function = new InvokableCallback<T0, T1, T2, T3, TReturn>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Function = new InvokableCallback<T0, T1, T2, T3, TReturn>(Target, MethodName);
                }
                else
                {
                    Function = GetPersistentMethod();
                }
            }
        }
    }
}