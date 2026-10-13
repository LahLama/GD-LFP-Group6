using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting.InputSystem;
using UnityEditor;
using UnityEngine;


public enum TileState { Tile,Obstacle, MoveAdd, Element, OneWay, Wall, EndPoint, StartTile}

public enum ElementState { Acid,Electro,Fire,Ice, Base }



public class InteractionManager : MonoBehaviour
{
    Ray ray;
	RaycastHit hit;
    PlayerProperties playerProperties;
    public bool isMovementReversed = false;
    public bool isOneWaysActive = false;
    
 MovementChecker movementChecker;
    // Create a global struct with the tile types that i can use in other scripts


  // Create a region
#region InputSystem
    
   public InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerProperties = FindAnyObjectByType<PlayerProperties>();
         movementChecker = FindAnyObjectByType<MovementChecker>();
    }
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion

    void OnDrawGizmos()
    {
        
    Gizmos.color = Color.red;
    Gizmos.DrawRay(ray);
    }

    //Trigger this function when the player presses the WASD keys based on the input system reading


    void Update()
{
        
     if (!inputActions.Player.Move.WasPressedThisFrame()) return;
    Vector2Int playerCords = playerProperties.GetPlayerCords();
    //Per Move, check the situation of the tiles:
    

    int unitDirection = 1;
    if (isMovementReversed) unitDirection = -1;
    else if (!isMovementReversed) unitDirection = 1;
     // Change the direction of the ray here based on unit direction.
    Vector2 playerSuggestedMove  = inputActions.Player.Move.ReadValue<Vector2>() * unitDirection;
    Vector3 translatedMove = new Vector3( playerSuggestedMove.x,0,playerSuggestedMove.y);
    ray = new Ray(playerProperties.transform.position, translatedMove);
    Physics.Raycast(ray, out hit);
    
    if (hit.collider == null) return;
    if (!hit.collider.gameObject.TryGetComponent<TileProperties>(out var tile)) return;
   
    if (tile.tileState == TileState.Wall) return;
        Vector2Int newCords = tile.cords;
   

// Debug.Log("input : " +inputActions.Player.Move.ReadValue<Vector2>());
    switch (playerSuggestedMove)
    {       
    // Check if WASD - then move accordiling.
        case Vector2 v when v == Vector2.up :
            if (movementChecker.CheckAbovePlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                                    playerProperties.turningUp.SetActive(true);
                    playerProperties.turningDown.SetActive(false);
                    playerProperties.turningLeft.SetActive(false);
                    playerProperties.turningRight.SetActive(false);
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
         
            break;
        
        case Vector2 v when v == Vector2.down :
            if (movementChecker.CheckBelowPlayer(playerCords,newCords))          
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {   
                    playerProperties.turningUp.SetActive(false);
                    playerProperties.turningDown.SetActive(true);
                    playerProperties.turningLeft.SetActive(false);
                    playerProperties.turningRight.SetActive(false);
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
        break;

        case Vector2 v when v == Vector2.left :
            if (movementChecker.CheckLeftOfPlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    
                    playerProperties.turningUp.SetActive(false);
                    playerProperties.turningDown.SetActive(false);
                    playerProperties.turningLeft.SetActive(true);
                    playerProperties.turningRight.SetActive(false);
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
            break;
        case Vector2 v when v == Vector2.right :
        if (movementChecker.CheckRightOfPlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    playerProperties.turningUp.SetActive(false);
                    playerProperties.turningDown.SetActive(false);
                    playerProperties.turningLeft.SetActive(false);
                    playerProperties.turningRight.SetActive(true);
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
        
            break;
        
        // Deafault case where it debugs no movement was detected
        default:
            Debug.Log("No movement detected");
            Debug.Log(inputActions.Player.Move.ReadValue<Vector2>());
            break;
    
    }
TileFunctions tileFunctions = FindAnyObjectByType<TileFunctions>();
TileProperties[] tiles = FindObjectsByType<TileProperties>();
bool reversed = false;
foreach (var t in tiles)
{
    if (t.tileState == TileState.Obstacle)
    {
        if (tileFunctions.PerMoveChecker(t.radius, playerCords, t.cords, t))
            reversed = true;
    }

    if (t.tileState == TileState.OneWay)
    {
        //Handles changing the material of the tile depending on the player's element
        if (playerProperties.playerElement == ElementState.Fire)
        {
            
            t.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Achetypes/oneWay");
            isOneWaysActive = true;
        }
        else
        {
            t.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Achetypes/baseTile");   
            isOneWaysActive = false;
        }
        }
isMovementReversed = reversed;

    }






        

       
    
}
}
