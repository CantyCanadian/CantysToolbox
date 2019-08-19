///====================================================================================================
///
///     CellGroup by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public class CellGroup<T> where T : new()
    {
        public Dictionary<Vector2Int, Cell<T>> Cells { get { return m_Cells; } }
        private Dictionary<Vector2Int, Cell<T>> m_Cells = null;

        public CellGroup(Dictionary<Vector2Int, Cell<T>> cells)
        {
            m_Cells = cells;
        }

        #region Simple Getters

        /// <summary>
        /// Returns a specific cell.
        /// </summary>
        public Cell<T> GetSingleCell(int x, int y)
        {
            Cell<T> cell = TryGetCell(x, y);

            if (cell == null)
            {
                return null;
            }

            return cell;
        }

        /// <summary>
        /// Returns a group containing a specific cell.
        /// </summary>
        public CellGroup<T> GetCell(int x, int y)
        {
            Cell<T> cell = TryGetCell(x, y);

            if (cell == null)
            {
                return null;
            }

            Dictionary<Vector2Int, Cell<T>> newCell = new Dictionary<Vector2Int, Cell<T>>();
            newCell.Add(new Vector2Int(x, y), cell);

            return new CellGroup<T>(newCell);
        }

        /// <summary>
        /// Returns a group containing a set of given cell.
        /// </summary>
        public CellGroup<T> GetCells(params int[] positions)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int i = 0; i < positions.Length; i += 2)
            {
                Cell<T> cell = TryGetCell(i, i + 1);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(i, i + 1), cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        #endregion

        #region Rectangle

        /// <summary>
        /// Returns a group containing a rectangle of cells.
        /// </summary>
        public CellGroup<T> GetCellRectangle(int x, int y, int w, int h)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int u = x; u < x + w; u++)
            {
                for (int v = y; v < y + h; v++)
                {
                    Cell<T> cell = TryGetCell(u, v);

                    if (cell != null)
                    {
                        cells.Add(new Vector2Int(u, v), cell);
                    }
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing the sides of a rectangle of cells.
        /// </summary>
        public CellGroup<T> GetCellRectangleHollow(int x, int y, int w, int h)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int u = x; u < x + w; u++)
            {
                Cell<T> cellTop = TryGetCell(u, y);

                if (cellTop != null)
                {
                    cells.Add(new Vector2Int(u, y), cellTop);
                }

                Cell<T> cellBot = TryGetCell(u, y + h - 1);

                if (cellBot != null)
                {
                    cells.Add(new Vector2Int(u, y + h - 1), cellBot);
                }
            }

            for (int v = y + 1; v < y + h - 1; v++)
            {
                Cell<T> cellTop = TryGetCell(x, v);

                if (cellTop != null)
                {
                    cells.Add(new Vector2Int(x, v), cellTop);
                }

                Cell<T> cellBot = TryGetCell(x + w - 1, v);

                if (cellBot != null)
                {
                    cells.Add(new Vector2Int(x + w - 1, v), cellBot);
                }
            }

            return new CellGroup<T>(cells);
        }

        #endregion

        #region Circle

        /// <summary>
        /// Returns a group containing a circle of cells.
        /// </summary>
        public CellGroup<T> GetCellCircle(int x, int y, int radius)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int u = x - radius; u < x + radius; u++)
            {
                for (int v = y - radius; v < y + radius; v++)
                {
                    if (Mathf.Sqrt((u * u) + (v * v)) <= radius)
                    {
                        Cell<T> cell = TryGetCell(u, v);

                        if (cell != null)
                        {
                            cells.Add(new Vector2Int(u, v), cell);
                        }
                    }
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing the sides of a circle of cells.
        /// </summary>
        public CellGroup<T> GetCellCircleHollow(int x, int y, int radius)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int u = x - radius; u < x + radius; u++)
            {
                for (int v = y - radius; v < y + radius; v++)
                {
                    if (Mathf.Sqrt((u * u) + (v * v)) <= radius)
                    {
                        Cell<T> cell = TryGetCell(u, v);

                        if (cell != null)
                        {
                            cells.Add(new Vector2Int(u, v), cell);
                        }

                        break;
                    }
                }

                for (int v = y + radius; v > y - radius; v--)
                {
                    if (Mathf.Sqrt((u * u) + (v * v)) <= radius)
                    {
                        Cell<T> cell = TryGetCell(u, v);

                        if (cell != null)
                        {
                            cells.Add(new Vector2Int(u, v), cell);
                        }

                        break;
                    }
                }
            }

            return new CellGroup<T>(cells);
        }

        #endregion

        #region Diamond

        /// <summary>
        /// Returns a group containing a diamond of cells.
        /// </summary>
        public CellGroup<T> GetCellDiamond(int x, int y, int diameter)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            if (diameter % 2 == 1)
            {
                int radius = (diameter - 1) / 2;

                for (int u = x - radius; u <= x + radius; u++)
                {
                    for (int v = y - radius; v <= y + radius; v++)
                    {
                        if (Mathf.Abs(x - u) + Mathf.Abs(y - v) <= diameter)
                        {
                            Cell<T> cell = TryGetCell(u, v);

                            if (cell != null)
                            {
                                cells.Add(new Vector2Int(u, v), cell);
                            }
                        }
                    }
                }
            }
            else
            {
                int radius = diameter / 2;

                for (int u = x - radius; u < x + radius; u++)
                {
                    for (int v = y - radius; v < y + radius; v++)
                    {
                        if (Mathf.Abs(x - u) + Mathf.Abs(y - v) <= diameter)
                        {
                            Cell<T> cell = TryGetCell(u, v);

                            if (cell != null)
                            {
                                cells.Add(new Vector2Int(u, v), cell);
                            }
                        }
                    }
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing the sides of a diamond of cells.
        /// </summary>
        public CellGroup<T> GetCellDiamondHollow(int x, int y, int diameter)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            if (diameter % 2 == 1)
            {
                int radius = (diameter - 1) / 2;

                for (int u = x - radius; u <= x + radius; u++)
                {
                    for (int v = y - radius; v <= y + radius; v++)
                    {
                        if (Mathf.Abs(u) + Mathf.Abs(v) <= diameter)
                        {
                            Cell<T> cell = TryGetCell(u, v);

                            if (cell != null)
                            {
                                cells.Add(new Vector2Int(u, v), cell);
                            }

                            break;
                        }
                    }

                    for (int v = y + radius; v >= y - radius; v--)
                    {
                        if (Mathf.Abs(u) + Mathf.Abs(v) <= diameter)
                        {
                            Cell<T> cell = TryGetCell(u, v);

                            if (cell != null)
                            {
                                cells.Add(new Vector2Int(u, v), cell);
                            }

                            break;
                        }
                    }
                }
            }
            else
            {
                int radius = diameter / 2;

                for (int u = x - radius; u < x + radius; u++)
                {
                    for (int v = y - radius; v < y + radius; v++)
                    {
                        if (Mathf.Abs(u) + Mathf.Abs(v) <= diameter)
                        {
                            Cell<T> cell = TryGetCell(u, v);

                            if (cell != null)
                            {
                                cells.Add(new Vector2Int(u, v), cell);
                            }

                            break;
                        }
                    }

                    for (int v = y + radius; v > y - radius; v--)
                    {
                        if (Mathf.Abs(u) + Mathf.Abs(v) <= diameter)
                        {
                            Cell<T> cell = TryGetCell(u, v);

                            if (cell != null)
                            {
                                cells.Add(new Vector2Int(u, v), cell);
                            }

                            break;
                        }
                    }
                }
            }

            return new CellGroup<T>(cells);
        }


        #endregion

        #region Directional

        /// <summary>
        /// Returns a group containing lines of cells in all four cardinal directions.
        /// </summary>
        public CellGroup<T> GetCellCardinal(int x, int y, int sideLength)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();
            Cell<T> cell = TryGetCell(x, y);

            if (cell != null)
            {
                cells.Add(new Vector2Int(x, y), cell);
            }

            for (int i = 0; i < sideLength; i++)
            {
                cell = TryGetCell(x + i, y);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x + i, y), cell);
                }

                cell = TryGetCell(x - i, y);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x - i, y), cell);
                }

                cell = TryGetCell(x, y + i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x, y + i), cell);
                }

                cell = TryGetCell(x, y - i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x, y - i), cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing lines of cells in all four diagonal directions.
        /// </summary>
        public CellGroup<T> GetCellDiagonal(int x, int y, int sideLength)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();
            Cell<T> cell = TryGetCell(x, y);

            if (cell != null)
            {
                cells.Add(new Vector2Int(x, y), cell);
            }

            for (int i = 0; i < sideLength; i++)
            {
                cell = TryGetCell(x + i, y + i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x + i, y + i), cell);
                }

                cell = TryGetCell(x - i, y + i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x - i, y + i), cell);
                }

                cell = TryGetCell(x + i, y - i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x + i, y - i), cell);
                }

                cell = TryGetCell(x - i, y - i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x - i, y - i), cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing lines of cells in all eight directions.
        /// </summary>
        public CellGroup<T> GetCellCardinalDiagonal(int x, int y, int sideLength)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();
            Cell<T> cell = TryGetCell(x, y);

            if (cell != null)
            {
                cells.Add(new Vector2Int(x, y), cell);
            }

            for (int i = 0; i < sideLength; i++)
            {
                cell = TryGetCell(x + i, y);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x + i, y), cell);
                }

                cell = TryGetCell(x - i, y);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x - i, y), cell);
                }

                cell = TryGetCell(x, y + i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x, y + i), cell);
                }

                cell = TryGetCell(x, y - i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x, y - i), cell);
                }

                cell = TryGetCell(x + i, y + i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x + i, y + i), cell);
                }

                cell = TryGetCell(x - i, y + i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x - i, y + i), cell);
                }

                cell = TryGetCell(x + i, y - i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x + i, y - i), cell);
                }

                cell = TryGetCell(x - i, y - i);

                if (cell != null)
                {
                    cells.Add(new Vector2Int(x - i, y - i), cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        #endregion

        #region Recursive/Conditional

        /// <summary>
        /// Returns a group containing cells found recursively.
        /// </summary>
        public CellGroup<T> GetCellsRecursive(int x, int y, int steps)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            RecursiveGet(ref cells, x, y, steps);

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing all the cells that fits the condition.
        /// </summary>
        public CellGroup<T> GetCellsConditional(System.Func<T, bool> condition)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            foreach (KeyValuePair<Vector2Int, Cell<T>> cell in m_Cells)
            {
                if (condition.Invoke(cell.Value.GetData()))
                {
                    cells.Add(cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing cells found recursively that fits the condition.
        /// </summary>
        public CellGroup<T> GetCellsConditionalRecursive(int x, int y, int steps, System.Func<T, bool> condition)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            RecursiveGet(ref cells, x, y, steps, condition);

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Using the original cell group, returns a new group containing this cell group with its sides expanded.
        /// </summary>
        public CellGroup<T> ExpandGroupRecursive(int steps, CellGroup<T> originalCellGroup)
        {
            CellGroup<T> finalGroup = new CellGroup<T>(new Dictionary<Vector2Int, Cell<T>>());

            foreach (KeyValuePair<Vector2Int, Cell<T>> cell in m_Cells)
            {
                finalGroup = finalGroup.Plus(originalCellGroup.GetCellDiamond(cell.Key.x, cell.Key.y, steps));
            }

            return finalGroup;
        }

        #endregion

        #region Math

        /// <summary>
        /// Returns a group containing both cell groups.
        /// </summary>
        public CellGroup<T> Plus(CellGroup<T> toAdd)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>(m_Cells);

            foreach (KeyValuePair<Vector2Int, Cell<T>> cell in toAdd.Cells)
            {
                if (!cells.ContainsKey(cell.Key))
                {
                    cells.Add(cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing this cell group minus the given one.
        /// </summary>
        public CellGroup<T> Minus(CellGroup<T> toRemove)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>(m_Cells);

            foreach (Vector2Int cell in toRemove.Cells.Keys)
            {
                if (cells.ContainsKey(cell))
                {
                    cells.Remove(cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        /// <summary>
        /// Returns a group containing the given cell group minus this one.
        /// </summary>
        public CellGroup<T> Invert(CellGroup<T> original)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>(original.Cells);
            CellGroup<T> inverted = new CellGroup<T>(cells);

            inverted = inverted.Minus(this);

            return inverted;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Edit the data of all this group's cell.
        /// </summary>
        public void EditCells(System.Action<T> editAction)
        {
            foreach (Cell<T> cellData in m_Cells.Values)
            {
                editAction.Invoke(cellData.GetData());
            }
        }

        /// <summary>
        /// Check if the given position is found in this group.
        /// </summary>
        public bool ContainsCell(Vector2Int position)
        {
            return m_Cells.ContainsKey(position);
        }

        #endregion

        #region Private

        private Cell<T> TryGetCell(int x, int y)
        {
            Vector2Int position = new Vector2Int(x, y);

            if (m_Cells.ContainsKey(position))
            {
                return m_Cells[position];
            }

            return null;
        }

        private void RecursiveGet(ref Dictionary<Vector2Int, Cell<T>> list, int x, int y, int steps)
        {
            Cell<T> cell = TryGetCell(x, y);

            if (steps <= 0 || list.ContainsKey(new Vector2Int(x, y)))
            {
                return;
            }

            if (cell != null)
            {
                list.Add(new Vector2Int(x, y), cell);

                RecursiveGet(ref list, x - 1, y, steps - 1);
                RecursiveGet(ref list, x + 1, y, steps - 1);
                RecursiveGet(ref list, x, y - 1, steps - 1);
                RecursiveGet(ref list, x, y + 1, steps - 1);
            }
        }

        private void RecursiveGet(ref Dictionary<Vector2Int, Cell<T>> list, int x, int y, int steps, System.Func<T, bool> condition)
        {
            Cell<T> cell = TryGetCell(x, y);

            if (steps <= 0 || !condition.Invoke(cell.GetData()) || list.ContainsKey(new Vector2Int(x, y)))
            {
                return;
            }

            if (cell != null)
            {
                list.Add(new Vector2Int(x, y), cell);

                RecursiveGet(ref list, x - 1, y, steps - 1, condition);
                RecursiveGet(ref list, x + 1, y, steps - 1, condition);
                RecursiveGet(ref list, x, y - 1, steps - 1, condition);
                RecursiveGet(ref list, x, y + 1, steps - 1, condition);
            }
        }

        #endregion
    }
}