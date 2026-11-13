using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
   public List<MapCreator> GameLevels = new List<MapCreator>();
   [SerializeField] public int CurrentLevel = 1;

   public bool isFinalLevel = false;


     public void NextLevel()
{
    Debug.Log($"NextLevel called. CurrentLevel={CurrentLevel}, Count={GameLevels.Count}");

    // Finishing the last level in the list ends the game
    if (CurrentLevel >= GameLevels.Count - 1 || isFinalLevel)
    {
        SceneManager.LoadScene("End Scene");
        return;
    }

    ClearGrid(GameLevels[CurrentLevel]);
    GameLevels[CurrentLevel].enabled = false;
    if (GameLevels[CurrentLevel].transform.childCount > 0)
        GameLevels[CurrentLevel].transform.GetChild(0).gameObject.SetActive(false);

    CurrentLevel++;

    var player = FindAnyObjectByType<PlayerProperties>();
    player.RespawnCounter = 0;
    player.playerElement = ElementState.Base;
    player.currentMoves = GameLevels[CurrentLevel].StartingMoves;

    GameLevels[CurrentLevel].enabled = true;
    if (GameLevels[CurrentLevel].transform.childCount > 0)
        GameLevels[CurrentLevel].transform.GetChild(0).gameObject.SetActive(true);

    // Optional: still show the ship on the final level
    Transform playerShip = GameLevels[CurrentLevel].transform.Find("PlayerShip");
    if (playerShip != null)
    {
        playerShip.gameObject.SetActive(true);
        isFinalLevel = true;
    }
}


public void ResetLevel()
{
    ClearGrid(GameLevels[CurrentLevel]);
    GameLevels[CurrentLevel].enabled = false;
    if ( GameLevels[CurrentLevel].gameObject.transform.childCount > 0)
    GameLevels[CurrentLevel].gameObject.transform.GetChild(0).gameObject.SetActive(false);
    GameLevels[CurrentLevel].enabled = true;
    if ( GameLevels[CurrentLevel].gameObject.transform.childCount > 0)
    GameLevels[CurrentLevel].gameObject.transform.GetChild(0).gameObject.SetActive(true);
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
