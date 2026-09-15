using System.Collections.Generic;
using UnityEngine;


public enum TileState { Tile,Obstacle, MoveAdd, Element, Wall, EndPoint}

public enum ElementState { Fire,Earth,Nature,Water, Base }



public class InteractionManager : MonoBehaviour
{
    Ray ray;
	RaycastHit hit;
    PlayerProperties playerProperties;
    // Create a global struct with the tile types that i can use in other scripts


  // Create a region
#region InputSystem
    
    InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerProperties = FindAnyObjectByType<PlayerProperties>();
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
      

void Update()
{
    ray = Camera.main.ScreenPointToRay(inputActions.UI.Point.ReadValue<Vector2>());
    if (Physics.Raycast(ray, out hit))
    {
        TileProperties tile = hit.collider.gameObject.GetComponent<TileProperties>();
        if (tile == null) return;
        // Debug.Log(hit.collider.gameObject.name + " is being hovered over.");

        if (inputActions.UI.Click.WasReleasedThisFrame())
        {
          if (tile.tileState == TileState.Wall)
          return;

            Vector2Int newCords = tile.cords;
            Vector2Int playerCords = playerProperties.GetPlayerCords();
              Debug.Log("Player Cords: " + playerCords + "\n Tile Cords: " + newCords);
                       

            // Check if the tile is adjacent to the player
            if (playerProperties.CheckAdjacencyOnPlayer(playerCords, newCords) )
                {
                // Check if the player has enough moves to move to the tile
                if( tile.ExecuteType())
                {
                    // Update the player's position and coordinates
                    playerProperties.SetPlayerCords(newCords, tile.transform.position);
                }
            }
        
      
        }
    }
}
}
