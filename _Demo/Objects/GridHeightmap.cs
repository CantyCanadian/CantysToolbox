using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEngine;

public class GridHeightmap : MonoBehaviour
{
    public Vector2Int HeightmapSize;
    public float HeightmapHeight;
    public GameObject CubePrefab;

    private DataGrid<Vector2Int> m_Grid;
    private Dictionary<Cell<Vector2Int>, GameObject> m_Cubes;

    void Start()
    {
        m_Grid = new DataGrid<Vector2Int>();
        m_Cubes = new Dictionary<Cell<Vector2Int>, GameObject>();

        m_Grid.GenerateGrid(HeightmapSize.x, HeightmapSize.y);

        foreach (KeyValuePair<Vector2Int, Cell<Vector2Int>> cell in m_Grid.MainCellGroup.Cells)
        {
            cell.Value.SetData(cell.Key);
            GameObject cube = Instantiate(CubePrefab, transform);
            cube.transform.localPosition = new Vector3(cell.Key.x - HeightmapSize.x / 2, 0.0f, cell.Key.y - HeightmapSize.y / 2);
            m_Cubes.Add(cell.Value, cube);
        }
    }

    void Update()
    {
        foreach (KeyValuePair<Vector2Int, Cell<Vector2Int>> cell in m_Grid.MainCellGroup.Cells)
        {
            m_Cubes[cell.Value].transform.localScale = new Vector3(1.0f, Mathf.PerlinNoise(Time.time + cell.Key.x, Time.time + cell.Key.y) * HeightmapHeight + 0.1f, 1.0f);
        }
    }
}
