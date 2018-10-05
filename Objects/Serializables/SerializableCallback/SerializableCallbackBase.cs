///====================================================================================================
///
///     SerializableCallbackBase by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Canty.Serializable
{
    public abstract class SerializableCallbackBase<TReturn> : SerializableCallbackBase
    {
        public InvokableCallbackBase<TReturn> Function;

        public override void ClearCache()
        {
            base.ClearCache();
            Function = null;
        }

        protected InvokableCallbackBase<TReturn> GetPersistentMethod()
        {
            Type[] types = new Type[ArgumentTypes.Length + 1];
            Array.Copy(ArgumentTypes, types, ArgumentTypes.Length);
            types[types.Length - 1] = typeof(TReturn);

            Type genericType = null;
            switch (types.Length)
            {
                case 1:
                    genericType = typeof(InvokableCallback<>).MakeGenericType(types);
                    break;
                case 2:
                    genericType = typeof(InvokableCallback<,>).MakeGenericType(types);
                    break;
                case 3:
                    genericType = typeof(InvokableCallback<,,>).MakeGenericType(types);
                    break;
                case 4:
                    genericType = typeof(InvokableCallback<,,,>).MakeGenericType(types);
                    break;
                case 5:
                    genericType = typeof(InvokableCallback<,,,,>).MakeGenericType(types);
                    break;
                default:
                    throw new ArgumentException(types.Length + "args");
            }

            return Activator.CreateInstance(genericType, new object[] {Target, MethodName}) as
                InvokableCallbackBase<TReturn>;
        }
    }

    /// <summary> An inspector-friendly serializable function </summary>
    [System.Serializable]
    public abstract class SerializableCallbackBase : ISerializationCallbackReceiver
    {
        /// <summary> Target object </summary>
        public Object Target
        {
            get { return m_Target; }
            set
            {
                m_Target = value;
                ClearCache();
            }
        }

        /// <summary> Target method name </summary>
        public string MethodName
        {
            get { return m_MethodName; }
            set
            {
                m_MethodName = value;
                ClearCache();
            }
        }

        public object[] Arguments
        {
            get
            {
                return ArgumentsBuffer != null
                    ? ArgumentsBuffer
                    : ArgumentsBuffer = m_Arguments.Select(x => x.GetValue()).ToArray();
            }
        }

        public object[] ArgumentsBuffer;

        public Type[] ArgumentTypes
        {
            get
            {
                return ArgumentTypesBuffer != null
                    ? ArgumentTypesBuffer
                    : ArgumentTypesBuffer = m_Arguments.Select(x => Argument.RealType(x.Type)).ToArray();
            }
        }

        public Type[] ArgumentTypesBuffer;

        public bool Dynamic
        {
            get { return m_Dynamic; }
            set
            {
                m_Dynamic = value;
                ClearCache();
            }
        }

        [SerializeField] protected Object m_Target;
        [SerializeField] protected string m_MethodName;
        [SerializeField] protected Argument[] m_Arguments;
        [SerializeField] protected bool m_Dynamic;

#if UNITY_EDITOR
#pragma warning disable 0414
        [SerializeField] private string m_TypeName;
#pragma warning restore 0414
#endif

#if UNITY_EDITOR
        [SerializeField] private bool m_Dirty;
#endif

#if UNITY_EDITOR
        protected SerializableCallbackBase()
        {
            m_TypeName = GetType().AssemblyQualifiedName;
        }
#endif

        public virtual void ClearCache()
        {
            ArgumentTypesBuffer = null;
            ArgumentsBuffer = null;
        }

        public void SetMethod(Object target, string methodName, bool dynamic, params Argument[] args)
        {
            m_Target = target;
            m_MethodName = methodName;
            m_Dynamic = dynamic;
            m_Arguments = args;
            ClearCache();
        }

        protected abstract void Cache();

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (m_Dirty)
            {
                ClearCache();
                m_Dirty = false;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            m_TypeName = GetType().AssemblyQualifiedName;
#endif
        }
    }

    [System.Serializable]
    public struct Argument
    {
        public enum ArgumentType
        {
            Unsupported,
            Bool,
            Int,
            Float,
            String,
            Object
        }

        public bool BoolValue;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
        public Object ObjectValue;
        public ArgumentType Type;

        public object GetValue()
        {
            return GetValue(Type);
        }

        public object GetValue(ArgumentType type)
        {
            switch (type)
            {
                case ArgumentType.Bool:
                    return BoolValue;

                case ArgumentType.Int:
                    return IntValue;

                case ArgumentType.Float:
                    return FloatValue;

                case ArgumentType.String:
                    return StringValue;

                case ArgumentType.Object:
                    return ObjectValue;

                default:
                    return null;
            }
        }

        public static Type RealType(ArgumentType type)
        {
            switch (type)
            {
                case ArgumentType.Bool:
                    return typeof(bool);

                case ArgumentType.Int:
                    return typeof(int);

                case ArgumentType.Float:
                    return typeof(float);

                case ArgumentType.String:
                    return typeof(string);

                case ArgumentType.Object:
                    return typeof(Object);

                default:
                    return null;
            }
        }

        public static ArgumentType FromRealType(Type type)
        {
            if (type == typeof(bool))
            {
                return ArgumentType.Bool;
            }
            else if (type == typeof(int))
            {
                return ArgumentType.Int;
            }
            else if (type == typeof(float))
            {
                return ArgumentType.Float;
            }
            else if (type == typeof(String))
            {
                return ArgumentType.String;
            }
            else if (type == typeof(Object))
            {
                return ArgumentType.Object;
            }
            else
            {
                return ArgumentType.Unsupported;
            }
        }

        public static bool IsSupported(Type type)
        {
            return FromRealType(type) != ArgumentType.Unsupported;
        }
    }
}