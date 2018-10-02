﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T> where T : struct
{
    private List<List<Cell<T>>> m_Grid = null;

    public void GenerateGridFromTexture(Texture2D texture, Dictionary<Color, T> data)
    {
        m_Grid.Clear();
        m_Grid = new List<List<Cell<T>>>();

        for (int x = 0; x < texture.width; x++)
        {
            m_Grid.Add(new List<Cell<T>>());

            for (int y = 0; y < texture.height; y++)
            {
                Color pixel = texture.GetPixel(x, y);

                if (data.ContainsKey(pixel))
                {
                    m_Grid[x].Add(new Cell<T>(data[pixel], x, y));
                }
                else
                {
                    Debug.Warning("Grid : Obtained unknown color from texture during texture generation.");
                    m_Grid[x].Add(new Cell<T>(default(T), x, y));
                }
            }
        }
    }

    public void GenerateGrid(int width, int height)
    {
        GenerateGrid(width, height, default(T));
    }

    public void GenerateGrid(int width, int height, T commonData)
    {
        m_Grid.Clear();
        m_Grid = new List<List<Cell<T>>>();

        for (int x = 0; x < width; x++)
        {
            m_Grid.Add(new List<Cell<T>>());

            for (int y = 0; y < height; y++)
            {
                m_Grid[x].Add(new Cell<T>(commonData, x, y));
            }
        }
    }

    public CellGroup<T> GetCell(int x, int y)
    {
        return new CellGroup<T>(TryGetCell(x, y));
    }

    public CellGroup<T> GetCells(int[] positions)
    {
        List<Cell<T>> cells = new List<Cell<T>>();

        for (int i = 0; i < positions.Length; i += 2)
        {
            cells.Add(TryGetCell(i, i + 1));
        }

        return new CellGroup<T>(cells.ToArray());
    }

    public CellGroup<T> GetCellRectangle(int x, int y, int w, int h)
    {
        List<Cell<T>> cells = new List<Cell<T>>();

        for (int u = 0; u < x + w; u++)
        {
            for (int v = 0; v < y + h; v++)
            {
                cells.Add(TryGetCell(u, v));
            }
        }

        return new CellGroup<T>(cells.ToArray());
    }

    public CellGroup<T> GetCellCircle(int x, int y, int radius)
    {
        List<Cell<T>> cells = new List<Cell<T>>();

        for (int u = -radius; u < x + radius; u++)
        {
            for (int v = -radius; v < y + radius; v++)
            {
                if (Mathf.sqrt((u * u) + (v * v)) <= radius)
                {
                    cells.Add(TryGetCell(u, v));
                }
            }
        }

        return new CellGroup<T>(cells.ToArray());
    }

    public CellGroup<T> GetCellsRecursive(int x, int y)
    {
        return GetCellsRecursive(x, y, -1);
    }

    public CellGroup<T> GetCellsRecursive(int x, int y, int steps)
    {
        List<Cell<T>> cells = new List<Cell<T>>();

        Cell<T> coreCell = TryGetCell(x, y);

        if (coreCell != null)
        {
            coreCell.GetCellRecursive(ref cells, steps);
            coreCell.ResetCellRecursivity();
        }

        return new CellGroup<T>(cells.ToArray());
    }

    public CellGroup<T> GetAllCells()
    {
        List<Cell<T>> cells = new List<Cell<T>>();

        foreach(List<T> cellRow in m_Grid)
        {
            cells.AddRange(cellRow);
        }

        return new CellGroup<T>(cells.ToArray());
    }

    private Cell<T> TryGetCell(int x, int y)
    {
        if (x < 0 || x >= m_Grid.Count || y < 0 || y >= m_Grid[x].Count)
        {
            return null;
        }

        return m_Grid[x][y];
    }
}