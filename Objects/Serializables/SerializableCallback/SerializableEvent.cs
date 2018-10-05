///====================================================================================================
///
///     SerializableEvent by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

namespace Canty.Serializable
{
    [System.Serializable]
    public class SerializableEvent : SerializableEventBase
    {
        public void Invoke()
        {
            if (Invokable == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableEvent call = Invokable as InvokableEvent;
                call.Invoke();
            }
            else
            {
                Invokable.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Invokable = new InvokableEvent(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Invokable = new InvokableEvent(Target, MethodName);
                }
                else
                {
                    Invokable = GetPersistentMethod();
                }
            }
        }
    }

    public abstract class SerializableEvent<T0> : SerializableEventBase
    {
        public void Invoke(T0 arg0)
        {
            if (Invokable == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableEvent<T0> call = Invokable as InvokableEvent<T0>;
                call.Invoke(arg0);
            }
            else
            {
                Invokable.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Invokable = new InvokableEvent<T0>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Invokable = new InvokableEvent<T0>(Target, MethodName);
                }
                else
                {
                    Invokable = GetPersistentMethod();
                }
            }
        }
    }

    public abstract class SerializableEvent<T0, T1> : SerializableEventBase
    {
        public void Invoke(T0 arg0, T1 arg1)
        {
            if (Invokable == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableEvent<T0, T1> call = Invokable as InvokableEvent<T0, T1>;
                call.Invoke(arg0, arg1);
            }
            else
            {
                Invokable.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Invokable = new InvokableEvent<T0, T1>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Invokable = new InvokableEvent<T0, T1>(Target, MethodName);
                }
                else
                {
                    Invokable = GetPersistentMethod();
                }
            }
        }
    }

    public abstract class SerializableEvent<T0, T1, T2> : SerializableEventBase
    {
        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            if (Invokable == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableEvent<T0, T1, T2> call = Invokable as InvokableEvent<T0, T1, T2>;
                call.Invoke(arg0, arg1, arg2);
            }
            else
            {
                Invokable.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Invokable = new InvokableEvent<T0, T1, T2>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Invokable = new InvokableEvent<T0, T1, T2>(Target, MethodName);
                }
                else
                {
                    Invokable = GetPersistentMethod();
                }
            }
        }
    }

    public abstract class SerializableEvent<T0, T1, T2, T3> : SerializableEventBase
    {
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Invokable == null)
            {
                Cache();
            }

            if (m_Dynamic)
            {
                InvokableEvent<T0, T1, T2, T3> call = Invokable as InvokableEvent<T0, T1, T2, T3>;
                call.Invoke(arg0, arg1, arg2, arg3);
            }
            else
            {
                Invokable.Invoke(Arguments);
            }
        }

        protected override void Cache()
        {
            if (m_Target == null || string.IsNullOrEmpty(m_MethodName))
            {
                Invokable = new InvokableEvent<T0, T1, T2, T3>(null, null);
            }
            else
            {
                if (m_Dynamic)
                {
                    Invokable = new InvokableEvent<T0, T1, T2, T3>(Target, MethodName);
                }
                else
                {
                    Invokable = GetPersistentMethod();
                }
            }
        }
    }
}