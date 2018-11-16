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
    public class CellGroup<T> where T : struct
    {
        public Dictionary<Vector2Int, Cell<T>> Cells { get { return m_Cells; } }
        private Dictionary<Vector2Int, Cell<T>> m_Cells;

        public CellGroup(Dictionary<Vector2Int, Cell<T>> cells)
        {
            m_Cells = cells;
        }

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
                        if (Mathf.Abs(u) + Mathf.Abs(v) <= diameter)
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
                        if (Mathf.Abs(u) + Mathf.Abs(v) <= diameter)
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

        public CellGroup<T> GetCellsRecursive(int x, int y, int steps)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            RecursiveGet(ref cells, x, y, steps);

            return new CellGroup<T>(cells);
        }

        public CellGroup<T> GetCellsConditional(System.Func<T, bool> condition)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            foreach(KeyValuePair<Vector2Int, Cell<T>> cell in m_Cells)
            {
                if (condition.Invoke(cell.Value.GetData()))
                {
                    cells.Add(cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        public CellGroup<T> GetCellsConditionalRecursive(int x, int y, int steps, System.Func<T, bool> condition)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            RecursiveGet(ref cells, x, y, steps, condition);

            return new CellGroup<T>(cells);
        }

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

        public CellGroup<T> Minus(CellGroup<T> toRemove)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>(m_Cells);

            foreach(Vector2Int cell in toRemove.Cells.Keys)
            {
                if (cells.ContainsKey(cell))
                {
                    cells.Remove(cell);
                }
            }

            return new CellGroup<T>(cells);
        }

        public CellGroup<T> Invert(CellGroup<T> original)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>(original.Cells);
            CellGroup<T> inverted = new CellGroup<T>(cells);

            inverted = inverted.Minus(this);

            return inverted;
        }

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

            if (steps <= 0)
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

            if (steps <= 0 || !condition.Invoke(cell.GetData()))
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
    }
}