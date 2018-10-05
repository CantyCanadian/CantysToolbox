///====================================================================================================
///
///     InvokableCallbackBase by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

namespace Canty.Serializable
{
    public abstract class InvokableCallbackBase<TReturn>
    {
        public abstract TReturn Invoke(params object[] args);
    }
}