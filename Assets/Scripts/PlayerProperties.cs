 using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerProperties : MonoBehaviour
{
    public Vector2Int playerCords = new Vector2Int(1,1);
    public int currentMoves = 6;
    int movesCheck = 0;
    public int maxMoves = 6;
    public TextMeshProUGUI movesText;
    public Vector3 playerPos;

    public GameObject RespawningTooltip;
    public GameObject ElementIdentifier;

    [SerializeField] Vector2Int respawnPoint;
    [SerializeField] int respawnMoves;
    [SerializeField] Vector3 respawnTransform;

    [SerializeField] ElementState respawnElementState;
    public ElementState playerElement = ElementState.Base;
    public GameObject previousTile;
    public int RespawnCounter = 0;
    public float MovingTime = 0.1f;

    // Cached reference for performance and safety
    private GameFunctions gameFunctions; 

    void Start()
    {
        respawnElementState = playerElement;
        respawnPoint = playerCords;
        respawnMoves = maxMoves;
        respawnTransform = gameObject.transform.position;
        movesText.text = currentMoves.ToString();
        respawnPoint = playerCords;

        // Find the GameFunctions script once at the start
        gameFunctions = FindAnyObjectByType<GameFunctions>(); 
    }

    public bool CanModifyMove(int val)
    {
        movesCheck = currentMoves;
        if ((movesCheck+=val) < 0)
            return false;
        else
            return true;
    }

    public void ModifyMoves(int val)
    {
        currentMoves += val;
        
        if (currentMoves > maxMoves)
        {
            currentMoves = maxMoves;
        }
        //Current problem if the moves used to get an obstcle is at the same time they get to respawn.
        else if( currentMoves <= 0)
        {
            FindAnyObjectByType<InteractionManager>().enabled = false;
            Invoke("RespawnPlayer",0.55f);
        }
        movesText.text = currentMoves.ToString();
    }

    public void SetPlayerCords(Vector2Int newCords, Vector3 newPos)
    {
        playerCords = newCords;
        playerPos = newPos;

        // --- FIX FOR LEVEL CHANGING (SAME SCENE) ---
        if (newCords == new Vector2Int(1, 1)) 
        {
            if (gameFunctions != null) 
            {
                gameFunctions.ResetHUD();
            }
            
            playerElement = ElementState.Base; 
            UpdatePlayerColor(); 
        }
        // -------------------------------------------

        //Move the player smoothly to the new position
        float maxStep = FindAnyObjectByType<PlayerModelAnimator>().stopDelay;
       
        StartCoroutine(MoveTo(newPos, maxStep));
    }

    IEnumerator MoveTo(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.position = target;
    }

    public Vector2Int GetPlayerCords()
    {
        return playerCords;
    }

    public void UpdateRespawnPoint(Vector2Int NewRespawnPoint, int NewRespawnMoves, Vector3 NewRespawnTransform)
    {
        respawnPoint = NewRespawnPoint;
        respawnMoves = NewRespawnMoves;
        respawnTransform = NewRespawnTransform;

        respawnElementState = playerElement;

        TileProperties[] tiles = FindObjectsByType<TileProperties>();
        foreach (var tile in tiles)
        {
            tile.CacheTilesState();
        }
    }

    public void UpdatePlayerColor()
    {
        Renderer identifierRenderer = ElementIdentifier.GetComponent<Renderer>();
        Material identifierMaterial = identifierRenderer.material;
        Color targetColor = Color.white;

        if (playerElement == ElementState.Acid)
        {
            targetColor = new Color(0f, 1f, 0f, 0.25f);
        }
        else if (playerElement == ElementState.Electro)
        {
            targetColor = new Color(1f, 1f, 0f, 0.25f);
        }
        else if (playerElement == ElementState.Fire)
        {
            targetColor = new Color(1f, 0f, 0f, 0.25f);
        }
        else if (playerElement == ElementState.Ice)
        {
            targetColor = new Color(0f, 1f, 1f, 0.25f);
        }
        else
        {
            targetColor = new Color(1f, 1f, 1f, 0.25f);
        }

        identifierMaterial.color = targetColor;
        
        if (gameFunctions != null)
        {
            gameFunctions.GotElement(playerElement);
        }
    }

    public void RespawnPlayer()
    {
        RespawnCounter++;
        // pause the game, show a tooltip then resume the game after a click
        if (RespawnCounter > 1)
        {
            StartCoroutine(RespawnGuideCoroutine());
        }

        currentMoves = respawnMoves;
        playerCords = respawnPoint;
        this.transform.position = respawnTransform;
        
        // Reset player element to whatever the checkpoint was
        playerElement = respawnElementState;
        UpdatePlayerColor();
        
        // --- FIX FOR RUNNING OUT OF MOVES ---
        // Force the HUD to disappear when respawning! 
        if (gameFunctions != null)
        {
            gameFunctions.ResetHUD();
        }
        // ------------------------------------

        // Update the text counter
        movesText.text = currentMoves.ToString();
        FindAnyObjectByType<InteractionManager>().isMovementReversed = false;

        TileProperties[] tiles = FindObjectsByType<TileProperties>();
        foreach (var tile in tiles)
        {
            tile.RefreshTilesOnRespawn();
        }

       FindAnyObjectByType<InteractionManager>().enabled = true;
    }

    IEnumerator RespawnGuideCoroutine()
    {
        RespawningTooltip.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        yield return new WaitUntil(() => FindAnyObjectByType<InteractionManager>().inputActions.UI.Click.ReadValue<float>()>0); // Wait for a click
        Time.timeScale = 1f; // Resume the game
        RespawningTooltip.SetActive(false);
    }
}