using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public class Cell<T> where T : struct
    {
        private T m_Data;
        private Vector2Int m_Position;
        private List<Cell<T>> m_References;

        private bool m_Recursivable = false;

        public Cell(T data, int x, int y)
        {
            m_Data = data;
            m_Position = new Vector2Int(x, y);
            m_References = new List<Cell<T>>();
        }

        public Vector2Int GetPosition()
        {
            return m_Position;
        }

        public T GetData()
        {
            return m_Data;
        }

        public void PopulateCardinalReferences(Grid<T> grid)
        {
            m_References.AddRange(grid.GetCells(m_Position.x, m_Position.y + 1, m_Position.x, m_Position.y - 1, m_Position.x + 1, m_Position.y, m_Position.x - 1, m_Position.y).GetCells());
        }

        public void PopulateCustomCardinalReferences(Grid<T> grid, Func<Cell<T>, Cell<T>, bool> addCondition)
        {
            Cell<T>[] cells = grid.GetCells(m_Position.x, m_Position.y + 1, m_Position.x, m_Position.y - 1, m_Position.x + 1, m_Position.y, m_Position.x - 1, m_Position.y).GetCells();

            foreach (Cell<T> cell in cells)
            {
                if (addCondition.Invoke(this, cell))
                {
                    m_References.Add(cell);
                }
            }
        }

        public void PopulateDiagonalReferences(Grid<T> grid)
        {
            m_References.AddRange(grid.GetCells(m_Position.x - 1, m_Position.y + 1, m_Position.x - 1, m_Position.y - 1, m_Position.x + 1, m_Position.y + 1, m_Position.x + 1, m_Position.y - 1).GetCells());
        }

        public void PopulateCustomDiagonalReferences(Grid<T> grid, Func<Cell<T>, Cell<T>, bool> addCondition)
        {
            Cell<T>[] cells = grid.GetCells(m_Position.x - 1, m_Position.y + 1, m_Position.x - 1, m_Position.y - 1, m_Position.x + 1, m_Position.y + 1, m_Position.x + 1, m_Position.y - 1).GetCells();

            foreach (Cell<T> cell in cells)
            {
                if (addCondition.Invoke(this, cell))
                {
                    m_References.Add(cell);
                }
            }
        }

        public void ResetCellRecursivity()
        {
            if (m_Recursivable == true)
            {
                return;
            }

            m_Recursivable = true;

            foreach (Cell<T> cell in m_References)
            {
                if (cell != null)
                {
                    cell.ResetCellRecursivity();
                }
            }
        }

        public void GetCellRecursive(ref List<Cell<T>> cells, int stepsLeft)
        {
            if (m_Recursivable == false)
            {
                return;
            }

            m_Recursivable = false;

            cells.Add(this);

            if (stepsLeft == 0)
            {
                return;
            }

            foreach (Cell<T> cell in m_References)
            {
                if (cell != null)
                {
                    cell.GetCellRecursive(ref cells, stepsLeft - 1);
                }
            }
        }
    }
}