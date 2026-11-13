using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<MapCreator> GameLevels = new List<MapCreator>();
    [SerializeField] public int CurrentLevel = 1;

    // Drag the ship object here in the Inspector (recommended)
    [SerializeField] GameObject playerShip;

    public bool isFinalLevel = false;

    void Start()
    {
        UpdateFinalLevelState();
    }

    public void NextLevel()
    {
        Debug.Log($"NextLevel called. CurrentLevel={CurrentLevel}, Count={GameLevels.Count}");

        // Finishing the last level ends the game
        if (isFinalLevel)
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

        UpdateFinalLevelState();
    }

    void UpdateFinalLevelState()
    {
        isFinalLevel = CurrentLevel >= GameLevels.Count - 1;

        if (isFinalLevel)
            ShowShip();
    }

    void ShowShip()
    {
        // Fallback: search all children (including inactive, nested ones) by name
        if (playerShip == null)
        {
            foreach (Transform t in GameLevels[CurrentLevel].GetComponentsInChildren<Transform>(true))
            {
                if (t.CompareTag("PlayerShip"))
                {
                    playerShip = t.gameObject;
                    break;
                }
            }
        }

        if (playerShip != null)
            playerShip.SetActive(true);
        else
            Debug.LogWarning("PlayerShip not found on the final level.");
    }

    public void ResetLevel()
    {
        ClearGrid(GameLevels[CurrentLevel]);
        GameLevels[CurrentLevel].enabled = false;
        if (GameLevels[CurrentLevel].transform.childCount > 0)
            GameLevels[CurrentLevel].transform.GetChild(0).gameObject.SetActive(false);
        GameLevels[CurrentLevel].enabled = true;
        if (GameLevels[CurrentLevel].transform.childCount > 0)
            GameLevels[CurrentLevel].transform.GetChild(0).gameObject.SetActive(true);

        if (isFinalLevel)
            ShowShip();
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