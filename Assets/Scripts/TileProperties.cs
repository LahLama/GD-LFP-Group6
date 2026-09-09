using UnityEngine;
using UnityEngine.InputSystem;
public class TileProperties : MonoBehaviour
{
    private int row = 0;
    private int col = 0;
    public Vector2Int cords = new Vector2Int(0,0);
    public Vector2Int playerCords = new Vector2Int(0,0);
    public Vector2Int newCords = new Vector2Int(0,0);
    public string type = "tile";


    void Start()
    {
        // Divide the cordinate vector
        row = cords.x;
        col = cords.y;
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
