///====================================================================================================
///
///     InvokableEventBase by
///     - CantyCanadian
///		- Siccity
///
///====================================================================================================

namespace Canty.Serializable
{
    public abstract class InvokableEventBase
    {
        public abstract void Invoke(params object[] args);
    }
}