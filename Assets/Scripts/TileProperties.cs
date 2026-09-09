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
    public State tileState = State.Tile;
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
                // Debug.Log("This is a tile");
            if (playerProperties.CanModifyMove(tileMoveCost) && tileState == State.Tile )
            {
                playerProperties.ModifyMoves(tileMoveCost);
                MadeAMove = true;
            }
            else if (playerProperties.CanModifyMove(obstacleMoveCost) && tileState == State.Obstacle )
            {
                playerProperties.ModifyMoves(obstacleMoveCost);
                MadeAMove = true;
                }
            else if (playerProperties.CanModifyMove(moveAddCost) && tileState == State.MoveAdd )
            {
                playerProperties.ModifyMoves(moveAddCost);
                MadeAMove = true;
                }
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
