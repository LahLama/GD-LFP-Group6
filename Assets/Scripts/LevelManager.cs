using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   public List<MapCreator> GameLevels = new List<MapCreator>();
   [SerializeField] int CurrentLevel = 0;

    public void NextLevel()
{
    if (CurrentLevel + 1 < GameLevels.Count)
    {
        ClearGrid(GameLevels[CurrentLevel]);
        GameLevels[CurrentLevel].enabled = false;
        CurrentLevel++;
        GameLevels[CurrentLevel].enabled = true;
    }
}

void ClearGrid(MapCreator level)
{
    Transform parent = level.gridParent.transform;

    for (int i = parent.childCount - 1; i >= 0; i--)
    {
        Transform child = parent.GetChild(i);
        child.SetParent(null);
        Destroy(child.gameObject);
    }
}
}
