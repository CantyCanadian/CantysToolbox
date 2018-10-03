
// Part of SerializableCallback by Siccity on Github : https://github.com/Siccity/SerializableCallback

namespace Canty.Serializable
{
    public abstract class InvokableCallbackBase<TReturn>
    {
        public abstract TReturn Invoke(params object[] args);
    }
}