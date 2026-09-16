using System;
using System.Collections.Generic;
using UnityEngine;



public enum TileType
{
    ElementWater, // EW
    ElementFire, // EF
    ElementEarth, // EE
    ElementNature, // EN
    MoveAdd, // MA
    ObstacleWater, // OW
    ObstacleFire, // OF
    ObstacleEarth, // OE
    ObstacleNature, // ON
    NormalTile, // .
    EndTile, // X
    WallTile // W
}

[System.Serializable]
public class TilePrefabEntry
{
    public TileType type;
    public GameObject prefab;
}

[System.Serializable]
public class RowData
{
    public List<TileType> tiles = new List<TileType>();
}

public class MapCreator : MonoBehaviour
{
    static readonly Dictionary<string, TileType> CharMap = new Dictionary<string, TileType>
    {
        {"w", TileType.ElementWater},
        {"f", TileType.ElementFire},
        {"e", TileType.ElementEarth},
        {"n", TileType.ElementNature},

        {"W", TileType.ObstacleWater},
        {"F", TileType.ObstacleFire},
        {"E", TileType.ObstacleEarth},
        {"N", TileType.ObstacleNature},

        {"#", TileType.WallTile},
        {"m", TileType.MoveAdd},
        {".", TileType.NormalTile},
        {"END", TileType.EndTile}
    };


    [Header("Tile Prefabs")]
    public List<TilePrefabEntry> tilePrefabs = new List<TilePrefabEntry>();

    Dictionary<TileType, GameObject> _prefabLookup;

    [Header("Grid Settings")]
    public float cellSize = 1f;
    public Transform gridParent;

    void BuildPrefabLookup()
    {
        _prefabLookup = new Dictionary<TileType, GameObject>();
        foreach (var entry in tilePrefabs)
        {
            if (entry.prefab == null)
            {
                Debug.LogWarning($"No prefab assigned for {entry.type}");
                continue;
            }
            _prefabLookup[entry.type] = entry.prefab;

        }
    }


     void SpawnGrid()
    {
        for (int y = 0; y < rows.Count; y++)
        {
            var row = rows[y];
            for (int x = 0; x < row.tiles.Count; x++)
            {
                var type = row.tiles[x];

                if (!_prefabLookup.TryGetValue(type, out var prefab))
                {
                    Debug.LogError($"No prefab mapped for {type}");
                    continue;
                }

                Vector3 pos = new Vector3(x * cellSize, 0f, -y * cellSize);
            prefab.TryGetComponent<TileProperties>(out var tileProperties);
                if (tileProperties != null)
                {
                    // this arrangement will make it be row, col according to TileProperties script
                    tileProperties.cords = new Vector2Int(x+1,y+1);
                }
                Instantiate(prefab, pos, Quaternion.identity, gridParent);
            }
        }
    }





    public List<RowData> rows = new List<RowData>();

    List<RowData> CreateLevel(string[] rowStrings)
    {
        var result = new List<RowData>();
        foreach (var line in rowStrings)
        {
            var row = new RowData();
            var tokens = line.Split(',');
            foreach (var token in tokens)
            {
                row.tiles.Add(CharMap[token]);
            }
            result.Add(row);
        }
        return result;
    }

    void Awake()
    {
        string[] level =
         {
        
        ".,m,n,N,m,m,m,.,.",
        ".,m,m,w,W,m,m,.,.",
        ".,m,m,m,m,m,m,.,.",


    };

        rows = CreateLevel(level);
        BuildPrefabLookup();
        SpawnGrid();

        foreach (var row in rows)
        {
            foreach (var tile in row.tiles)
            {
                // Debug.Log(tile);
            }
        }
    }
}


