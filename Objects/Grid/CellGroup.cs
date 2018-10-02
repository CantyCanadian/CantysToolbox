using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroup<T> where T : struct
{
    private Cell<T>[] m_Cells;

    public CellGroup(Cell<T> cell)
    {
        m_Cells = new Cell<T>[] { cell };
    }

    public CellGroup(Cell<T>[] cells)
    {
        m_Cells = cells;
    }

    public Cell<T>[] GetCells()
    {
        return m_Cells;
    }

    public void DoOnAll(Action<Cell<T>> function)
    {
        foreach(Cell<T> cell in m_Cells)
        {
            function.Invoke(cell);
        }
    }
}