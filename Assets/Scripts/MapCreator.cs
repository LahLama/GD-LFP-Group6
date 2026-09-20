using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public enum TileType
{
    ElementIce, // EW
    ElementFire, // EF
    ElementElectro, // EE
    ElementAcid, // EN
    MoveAdd, // MA
    ObstacleIce, // OW
    ObstacleFire, // OF
    ObstacleElectro, // OE
    ObstacleAcid, // ON
    NormalTile, // .
    EndTile, // X
    WallTile, // W
    StartTile // W
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
    List<int> customMovementNumbers = new List<int>();
    public string[] level;

    static readonly Dictionary<string, TileType> CharMap = new Dictionary<string, TileType>
    {
        {"i", TileType.ElementIce},
        {"f", TileType.ElementFire},
        {"e", TileType.ElementElectro},
        {"a", TileType.ElementAcid},

        {"I", TileType.ObstacleIce},
        {"F", TileType.ObstacleFire},
        {"E", TileType.ObstacleElectro},
        {"A", TileType.ObstacleAcid},

        {"#", TileType.WallTile},
        {"m", TileType.MoveAdd},
        {".", TileType.NormalTile},
        {"O", TileType.StartTile},
        {"X", TileType.EndTile}
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
                   
                    int index = x + ( y * row.tiles.Count );
                    if (index < customMovementNumbers.Count )
                       {
                    // this sets the value of the correct index,
                    //Examples
                    
                    // 0 1 2  
                    // 3 4 5
                    // 6 7 8

                    // if row 0, tile 0 = x = 0
                    // if row 0, tile 1 = x = 1
                    
                    // if row 1, tile 0 = x = (maxCount ) + 0
                    // if row 1, tile 1 = x = (maxCount ) + 1

                    // if row 2, tile 0 = x = (2*maxCount) + 0
                    // if row 2, tile 1 = x = (2*maxCount) + 1

                    tileProperties.CustomMoveCost = customMovementNumbers[index];
                      
                    }
                }
                Instantiate(prefab, pos, Quaternion.identity, gridParent);
            }
        }
        // for (int i = 0; i < customMovementNumbers.Count; i++)
        // {
        //     Debug.Log(customMovementNumbers[i] + " @ "+ i);
        // }
    }





    public List<RowData> rows = new List<RowData>();

    List<RowData> CreateLevel(string[] rowStrings)
    {
        var result = new List<RowData>();
        foreach (var line in rowStrings)
        {
            var row = new RowData();
            var tokens = line.Split(',');
            foreach (string token in tokens)
            {
                string charToken = token[0].ToShortString();
                int numberToken = 0;

                if (token.Length > 1){
                    numberToken = (int)Char.GetNumericValue(token[1]);
                    // Set the customMoveCost to be this numberToken
                    customMovementNumbers.Add(numberToken);
                    }
                    else
                {
                    customMovementNumbers.Add(0);
                }
                // Debug.Log("Value: " + token + "\n Char: " + charToken + "\t Number: " + numberToken);
                row.tiles.Add(CharMap[charToken]);
            }
            result.Add(row);
            
        }
    
        return result;
    }

    void Start()
    {
    //     string[] level =
    //      {
        
    //     ".,m3,n,N2",
    //     ".,m6,w,W3",
    //     ".,m5,#",


    // };

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


