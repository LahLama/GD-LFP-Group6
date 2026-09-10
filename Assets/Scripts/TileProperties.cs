using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
public class TileProperties : MonoBehaviour
{
    private int row = 0;
    private int col = 0;
    public Vector2Int cords = new Vector2Int(0,0);
    public Vector2Int playerCords = new Vector2Int(0,0);
    public Vector2Int newCords = new Vector2Int(0,0);

   
    public TileState tileState = TileState.Tile;
    public ElementState tileElement = ElementState.Tipid;
    private bool MadeAMove = false;
    private PlayerProperties playerProperties;
    private int tileMoveCost = -1;
    private int obstacleMoveCost = -2;
    private int moveAddCost = +2;


    void Start()
    {
        playerProperties = FindAnyObjectByType<PlayerProperties>();
        // Divide the cordinate vector
        row = cords.x;
        col = cords.y;
    }

    public bool ExecuteType()
    {   
        MadeAMove = false; 
// Check if the player has enough moves to conquer the obstacle and if the player has the same element as the obstacle
        bool canConquerObstacle = playerProperties.CanModifyMove(obstacleMoveCost) 
                        && tileState == TileState.Obstacle
                        && playerProperties.playerElement == tileElement;
        

// Check if the player has enough moves to move to the tile
        if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Tile )
        {
            playerProperties.ModifyMoves(tileMoveCost);
            MadeAMove = true;
        }
    
// Check if the player has enough moves to conquer the obstacle and if the player has the same element as the obstacle
        else if (canConquerObstacle)
        {
        playerProperties.ModifyMoves(obstacleMoveCost);
        MadeAMove = true;
        }

// Check if the player has enough moves to move to the tile and if the tile is a MoveAdd tile
        else if (playerProperties.CanModifyMove(moveAddCost) && tileState == TileState.MoveAdd )
        {
        playerProperties.ModifyMoves(moveAddCost);
        MadeAMove = true;
        }

// Check if the player has enough moves to move to the tile and if the tile is an Element tile
        else if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Element)
        {
            playerProperties.ModifyMoves(tileMoveCost);
            playerProperties.playerElement = tileElement;
            MadeAMove = true;
        }
// If the player does not have enough moves to move to the tile, do not allow the player to move
        else 
        {
            MadeAMove = false;
        }
       
    return MadeAMove;
        // Debug.Log("Current Moves: " + playerProperties.currentMoves );
        
    }

    public bool CheckAdjacencyOnPlayer(Vector2Int playerCords, Vector2Int newCords)
    {   
        int playerX = playerCords.x;
        int playerY = playerCords.y;
        // Check if the player can move to an adjacent tile

        if (newCords.x == playerX && newCords.y == playerY + 1)  // Move - Up 
            return true;
        else if (newCords.x == playerX && newCords.y == playerY - 1) // Move - Down
            return true;
        else if (newCords.x == playerX + 1 && newCords.y == playerY) // Move - Right
            return true;
        else if (newCords.x == playerX - 1 && newCords.y == playerY) // Move - Left
            return true;
        else if (newCords.x == playerX + 1 && newCords.y == playerY + 1) // Move - Up Right
            return true;
        else if (newCords.x == playerX - 1 && newCords.y == playerY - 1) // Move - Down Left
            return true;
        else if (newCords.x == playerX + 1 && newCords.y == playerY - 1) // Move - Down Right
            return true;
        else if (newCords.x == playerX - 1 && newCords.y == playerY + 1) // Move - Up Left
            return true;
        else     // Not Adjacent
            return false;
    }


}
