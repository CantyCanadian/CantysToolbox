///====================================================================================================
///
///     Cell by
///     - CantyCanadian
///
///====================================================================================================

namespace Canty
{
    public class Cell<T> where T : new()
    {
        private T m_Data;

        public Cell(T data)
        {
            m_Data = data;
        }

        public void SetData(T data)
        {
            m_Data = data;
        }

        public T GetData()
        {
            return m_Data;
        }
    }
}