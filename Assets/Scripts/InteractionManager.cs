using System.Collections.Generic;
using UnityEngine;


public enum TileState { Tile,Obstacle, MoveAdd, Element}

public enum ElementState { Fire,Earth,Nature,Air, Tipid }



public class InteractionManager : MonoBehaviour
{
    Ray ray;
	RaycastHit hit;
    PlayerProperties playerProperties;
   public List<Material> elementMaterials = new List<Material>();
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
        // if (tile == null) return;
        // Debug.Log(hit.collider.gameObject.name + " is being hovered over.");

        if (inputActions.UI.Click.WasReleasedThisFrame())
        {
          
            Vector2Int newCords = tile.cords;
            Vector2Int playerCords = playerProperties.playerCords;
            //   Debug.Log("Player Cords: " + playerCords + "\n Tile Cords: " + newCords);
                       
            bool canMove = tile.ExecuteType() && tile.CheckAdjacencyOnPlayer(playerCords, newCords) ;
            
            Debug.Log(canMove);
            if (canMove)
            {
                // Debug.Log("Player moves");
            // Debug.Log("Player Cords: " + playerCords + "\n New Cords: " + newCords);
                playerProperties.SetPlayerCords(newCords, tile.transform.position);
            }
        }
    }
}
}
