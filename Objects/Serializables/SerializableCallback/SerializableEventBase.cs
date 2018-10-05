///====================================================================================================
///
///     SerializableEventBase by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

using System;

namespace Canty.Serializable
{
    public abstract class SerializableEventBase : SerializableCallbackBase
    {
        public InvokableEventBase Invokable;

        public override void ClearCache()
        {
            base.ClearCache();
            Invokable = null;
        }

        protected InvokableEventBase GetPersistentMethod()
        {
            Type[] types = new Type[ArgumentTypes.Length];
            Array.Copy(ArgumentTypes, types, ArgumentTypes.Length);

            Type genericType = null;
            switch (types.Length)
            {
                case 0:
                    genericType = typeof(InvokableEvent).MakeGenericType(types);
                    break;

                case 1:
                    genericType = typeof(InvokableEvent<>).MakeGenericType(types);
                    break;

                case 2:
                    genericType = typeof(InvokableEvent<,>).MakeGenericType(types);
                    break;

                case 3:
                    genericType = typeof(InvokableEvent<,,>).MakeGenericType(types);
                    break;

                case 4:
                    genericType = typeof(InvokableEvent<,,,>).MakeGenericType(types);
                    break;

                default:
                    throw new ArgumentException(types.Length + "args");
            }

            return Activator.CreateInstance(genericType, new object[] {Target, MethodName}) as InvokableEventBase;
        }
    }
}