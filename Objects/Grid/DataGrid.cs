///====================================================================================================
///
///     DataGrid by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public class DataGrid<T> where T : struct
    {
        public CellGroup<T> Cells { get { return m_Grid; } }
        private CellGroup<T> m_Grid = null;

        public void GenerateGridFromTexture(Texture2D texture, Dictionary<Color, T> data)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color pixel = texture.GetPixel(x, y);

                    if (data.ContainsKey(pixel))
                    {
                        cells.Add(new Vector2Int(x, y), new Cell<T>(data[pixel]));
                    }
                    else
                    {
                        Debug.LogWarning("Grid : Obtained unknown color from texture during texture generation.");
                        cells.Add(new Vector2Int(x, y), new Cell<T>(default(T)));
                    }
                }
            }

            m_Grid = new CellGroup<T>(cells);
        }

        public void GenerateGrid(int width, int height, T commonData)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells.Add(new Vector2Int(x, y), new Cell<T>(commonData));
                }
            }

            m_Grid = new CellGroup<T>(cells);
        }

        public void GenerateGrid(int width, int height)
        {
            GenerateGrid(width, height, default(T));
        }        
    }
}