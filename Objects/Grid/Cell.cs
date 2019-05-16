///====================================================================================================
///
///     Cell by
///     - CantyCanadian
///
///====================================================================================================

namespace Canty
{
	/// <summary>
	/// Simple data container for the Grid class.
	/// </summary>
    public class Cell<T> where T : struct
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