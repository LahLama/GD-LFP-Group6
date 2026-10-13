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
int movesCheck =0;
public int maxMoves = 6;
public TextMeshProUGUI movesText;
public Vector3 playerPos;
public GameObject turningUp;
public GameObject turningDown;
public GameObject turningLeft;
public GameObject turningRight;
public GameObject RespawningTooltip;

[SerializeField] Vector2Int respawnPoint;
[SerializeField] int respawnMoves;
[SerializeField] Vector3 respawnTransform;

[SerializeField] ElementState respawnElementState;
public ElementState playerElement = ElementState.Base;
public GameObject previousTile;
int RespawnCounter = 0;

    void Start()
    {
        respawnElementState = playerElement;
        respawnPoint = playerCords;
        respawnMoves = maxMoves;
        respawnTransform = gameObject.transform.position;
         movesText.text = currentMoves.ToString();
        respawnPoint = playerCords;
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

        //Move the player smoothly to the new position
        StartCoroutine(MoveTo(newPos, 0.3f));
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
        
            if (playerElement == ElementState.Acid)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else if (playerElement == ElementState.Electro)
            {
                GetComponent<Renderer>().material.color = Color.yellow;
            }           
             else if (playerElement == ElementState.Fire)
            {
            GetComponent<Renderer>().material.color = Color.red;
            }
            else if (playerElement == ElementState.Ice)
            {
               GetComponent<Renderer>().material.color = Color.cyan;
            }
            else
            {
               GetComponent<Renderer>().material.color = Color.white;
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
        playerElement = respawnElementState;
        UpdatePlayerColor();
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
