using UnityEngine;

public class LevelControls : MonoBehaviour
{
    [Header("Drag these in")]
    public GameObject buttonsRoot;       // parent holding both buttons
    public GameObject skipButtonObject;  // the Skip button (hidden on the last level)

    [Header("Show the buttons on these level numbers")]
    public int firstLevelNumber = 5;
    public int lastLevelNumber = 10;

    LevelManager levels;
    PlayerProperties player;
    InteractionManager interaction;

    void Start()
    {
        levels = FindAnyObjectByType<LevelManager>();
        player = FindAnyObjectByType<PlayerProperties>();
        interaction = FindAnyObjectByType<InteractionManager>();
    }

    void Update()
    {
        if (levels == null || buttonsRoot == null) return;

        int index = CurrentIndex();
        int number = index + 1; // Level 1 is index 0
        bool show = index >= 0 && number >= firstLevelNumber && number <= lastLevelNumber;
        if (buttonsRoot.activeSelf != show) buttonsRoot.SetActive(show);

        if (show && skipButtonObject != null)
        {
            bool isLast = index >= levels.GameLevels.Count - 1;
            if (skipButtonObject.activeSelf == isLast) skipButtonObject.SetActive(!isLast);
        }
    }

   
    public void RestartLevel()
    {
        if (levels == null || player == null) return;
        int index = CurrentIndex();
        if (index < 0) return;
        MapCreator level = levels.GameLevels[index];

        PlayClick();
        PrepareForReset();
        ClearOldTiles(level);
        player.currentMoves = level.StartingMoves;

        level.enabled = false; // off and on rebuilds the level
        level.enabled = true;

        var sound = FindAnyObjectByType<SoundManager>();
        if (sound != null) sound.SendMessage("RefreshAfterLevelReset", SendMessageOptions.DontRequireReceiver);
    }

    public void SkipLevel()
    {
        if (levels == null || player == null) return;
        int index = CurrentIndex();
        if (index < 0 || index >= levels.GameLevels.Count - 1) return;

        PlayClick();
        PrepareForReset();
        ClearOldTiles(levels.GameLevels[index]);
        levels.NextLevel();
    }

    int CurrentIndex()
    {
        for (int i = 0; i < levels.GameLevels.Count; i++)
            if (levels.GameLevels[i] != null && levels.GameLevels[i].enabled) return i;
        return -1;
    }

    void PlayClick()
    {
        var sound = FindAnyObjectByType<SoundManager>();
        if (sound != null) sound.SendMessage("PlayButtonClick", SendMessageOptions.DontRequireReceiver);
    }

    void PrepareForReset()
    {
        Time.timeScale = 1f;
        player.CancelInvoke();
        player.StopAllCoroutines();
        if (player.RespawningTooltip != null) player.RespawningTooltip.SetActive(false);

        player.previousTile = null;
        player.RespawnCounter = 0;
        player.playerElement = ElementState.Base;
        player.UpdatePlayerColor();

        if (interaction != null)
        {
            interaction.enabled = true;
            interaction.isMovementReversed = false;
            interaction.isOneWaysActive = false;
        }
    }

    void ClearOldTiles(MapCreator level)
    {
        if (level == null || level.gridParent == null) return;
        Transform parent = level.gridParent;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            TileProperties tile = child.GetComponent<TileProperties>();
            if (tile == null) continue; // leave anything that isn't a tile alone

            tile.CancelInvoke();
            tile.tileState = TileState.Tile; // stops any Electro pulse on this old tile
            child.SetParent(null);
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
    }
}