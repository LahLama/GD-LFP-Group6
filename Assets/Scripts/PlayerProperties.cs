using System;
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

[SerializeField] Vector2Int respawnPoint;
[SerializeField] int respawnMoves;
[SerializeField] Vector3 respawnTransform;

[SerializeField] ElementState respawnElementState;
public ElementState playerElement = ElementState.Base;

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
            Invoke("RespawnPlayer",0.25f);
            
        }
         movesText.text = currentMoves.ToString();
    }

 

public void SetPlayerCords(Vector2Int newCords, Vector3 newPos)
    {
        playerCords = newCords;
        playerPos = newPos;
        gameObject.transform.position = newPos;
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
        currentMoves = respawnMoves;
       
        playerCords = respawnPoint;
        this.transform.position = respawnTransform;
        playerElement = respawnElementState;
        UpdatePlayerColor();
        // Update the text counter
        movesText.text = currentMoves.ToString();


        TileProperties[] tiles = FindObjectsByType<TileProperties>();
        foreach (var tile in tiles)
        {
            tile.RefreshTilesOnRespawn();
            
        }

       FindAnyObjectByType<InteractionManager>().enabled = true;
    }


}
