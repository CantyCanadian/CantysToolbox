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
    public class DataGrid<T> where T : new()
    {
        public CellGroup<T> MainCellGroup { get { return m_Group; } }
        private CellGroup<T> m_Group = null;

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

            m_Group = new CellGroup<T>(cells);
        }

        public void GenerateGrid(int width, int height)
        {
            Dictionary<Vector2Int, Cell<T>> cells = new Dictionary<Vector2Int, Cell<T>>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells.Add(new Vector2Int(x, y), new Cell<T>(new T()));
                }
            }

            m_Group = new CellGroup<T>(cells);
        }   
    }
}