using UnityEngine;
using UnityEngine.InputSystem;
public class TileProperties : MonoBehaviour
{
    private int row = 0;
    private int col = 0;
    public Vector2Int cords = new Vector2Int(0,0);
    public Vector2Int playerCords = new Vector2Int(0,0);
    public Vector2Int newCords = new Vector2Int(0,0);
    public State tileState = State.Tile;
    private PlayerProperties playerProperties;

    void Start()
    {
        playerProperties = FindAnyObjectByType<PlayerProperties>();
        // Divide the cordinate vector
        row = cords.x;
        col = cords.y;
    }

    public void ExecuteType()
    {
        switch (tileState)
        {
            case State.Tile:
                Debug.Log("This is a tile");
                playerProperties.ModifyMoves(-1);
                break;
            case State.Obstacle:
                Debug.Log("This is an obstacle");
                playerProperties.ModifyMoves(-2);
                break;
            case State.MoveAdd:
                Debug.Log("This is a move add");
                playerProperties.ModifyMoves(+3);    
                break;
            default:
                Debug.Log("This is a tile");
                break;
        }
        Debug.Log("Current Moves: " + playerProperties.currentMoves );
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
