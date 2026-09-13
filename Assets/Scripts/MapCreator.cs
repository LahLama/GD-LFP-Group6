using System;
using System.Collections.Generic;
using UnityEngine;



public enum CellType
{
    Empty,   // e
    Mine,    // m
    Trap,    // t
    End      // END
}

[System.Serializable]
public class RowData
{
    public List<CellType> cells = new List<CellType>();
}

public class MapCreator : MonoBehaviour
{
    static readonly Dictionary<char, CellType> CharMap = new Dictionary<char, CellType>
    {
        { 'e', CellType.Empty },
        { 'm', CellType.Mine },
        { 't', CellType.Trap },
    };

    public List<RowData> rows = new List<RowData>();

    List<RowData> CreateLevel(string[] rowStrings)
    {
        var result = new List<RowData>();
        foreach (var line in rowStrings)
        {
            var row = new RowData();
            // handle "END" as a special multi-char token
            var tokens = line.Split(',');
            foreach (var token in tokens)
            {
                row.cells.Add(token == "END" ? CellType.End : CharMap[token[0]]);
            }
            result.Add(row);
        }
        return result;
    }

    void Awake()
    {
        string[] level =
        {
            ""
        };

        CreateLevel(level);
    }
}


