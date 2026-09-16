using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEditor;
using UnityEngine;


public enum TileState { Tile,Obstacle, MoveAdd, Element, Wall, EndPoint}

public enum ElementState { Fire,Earth,Nature,Water, Base }



public class InteractionManager : MonoBehaviour
{
    Ray ray;
	RaycastHit hit;
    PlayerProperties playerProperties;
    
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

    Vector2Int newCords = tile.cords;
    Vector2Int playerCords = playerProperties.GetPlayerCords();
// Debug.Log("input : " +inputActions.Player.Move.ReadValue<Vector2>());
    if (!inputActions.Player.Move.WasPressedThisFrame()) return;
    
    
    switch (inputActions.Player.Move.ReadValue<Vector2>())
    {
        
    // Check if WASD - then move accordiling.
        case Vector2 v when v == Vector2.up:
            if (movementChecker.CheckAbovePlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
         
            break;
        
        case Vector2 v when v == Vector2.down:
            if (movementChecker.CheckBelowPlayer(playerCords,newCords))          
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
        break;

        case Vector2 v when v == Vector2.left:
            if (movementChecker.CheckLeftOfPlayer(playerCords,newCords))
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
        
            break;
        case Vector2 v when v == Vector2.right:
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
