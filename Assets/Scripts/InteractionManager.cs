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
    
 MovementChecker movementChecker;
    // Create a global struct with the tile types that i can use in other scripts


  // Create a region
#region InputSystem
    
    InputSystem_Actions inputActions;
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
        
    Vector2 playerSuggestedMove  = inputActions.Player.Move.ReadValue<Vector2>();
    Vector3 translatedMove = new Vector3( playerSuggestedMove.x,0,playerSuggestedMove.y);
    ray = new Ray(playerProperties.transform.position, translatedMove);
    Physics.Raycast(ray, out hit);
    
    if (hit.collider == null) return;
    if (!hit.collider.gameObject.TryGetComponent<TileProperties>(out var tile)) return;
    if (!inputActions.Player.Move.WasPressedThisFrame()) return;
    if (tile.tileState == TileState.Wall) return;
    
    Vector2Int newCords = tile.cords;
    Vector2Int playerCords = playerProperties.GetPlayerCords();
// Debug.Log("input : " +inputActions.Player.Move.ReadValue<Vector2>());



    int unitDirection = 1;
    if (isMovementReversed) unitDirection = -1;
    else if (!isMovementReversed) unitDirection = 1;


        //Per Move, check the situation of the tiles:
        TileProperties[] tiles = FindObjectsByType<TileProperties>();
        foreach (var t in tiles)
        {
            FindAnyObjectByType<TileFunctions>().PerMoveChecker(t.radius,playerCords,t.cords,t);
        }

    switch (unitDirection * inputActions.Player.Move.ReadValue<Vector2>())
    {       
    // Check if WASD - then move accordiling.
        case Vector2 v when v == Vector2.up * unitDirection:
            if (movementChecker.CheckAbovePlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
         
            break;
        
        case Vector2 v when v == Vector2.down * unitDirection:
            if (movementChecker.CheckBelowPlayer(playerCords,newCords))          
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
        break;

        case Vector2 v when v == Vector2.left * unitDirection:
            if (movementChecker.CheckLeftOfPlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
            break;
        case Vector2 v when v == Vector2.right * unitDirection:
        if (movementChecker.CheckRightOfPlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
        
            break;
    
    }







        

       
    
}
}
