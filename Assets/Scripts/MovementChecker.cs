using UnityEngine;

public class MovementChecker : MonoBehaviour
{   
 int playerX ;
    int playerY ;
    Vector2Int playerCords;

    public bool CheckLeftOfPlayer(Vector2Int CurrentPlayerCords, Vector2Int newCords)
    {
        playerCords = CurrentPlayerCords;
        playerX = CurrentPlayerCords.x;
        playerY = CurrentPlayerCords.y;

        if (newCords.x == playerX - 1 && newCords.y == playerY) // Move - Left
            return true;
        else
            return false;
    }

    public bool CheckRightOfPlayer(Vector2Int CurrentPlayerCords, Vector2Int newCords)
    {
        playerCords = CurrentPlayerCords;
        playerX = CurrentPlayerCords.x;
        playerY = CurrentPlayerCords.y;

        if (newCords.x == playerX + 1 && newCords.y == playerY) // Move - Right
            return true;
        else
            return false;
    }

    public bool CheckAbovePlayer(Vector2Int CurrentPlayerCords, Vector2Int newCords)
    {
        playerCords = CurrentPlayerCords;
        playerX = CurrentPlayerCords.x;
        playerY = CurrentPlayerCords.y;

        if (newCords.x == playerX && newCords.y == playerY - 1) // Move - Up
            return true;
        else
            return false;
    }

    public bool CheckBelowPlayer(Vector2Int CurrentPlayerCords, Vector2Int newCords)
    {
        playerCords = CurrentPlayerCords;
        playerX = CurrentPlayerCords.x;
        playerY = CurrentPlayerCords.y;

        
        if (newCords.x == playerX && newCords.y == playerY + 1) // Move - Down
            return true;
        else
            return false;
    }  
    
}
