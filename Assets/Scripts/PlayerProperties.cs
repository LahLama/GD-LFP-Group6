using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
public Vector2Int playerCords = new Vector2Int(1,1);
public Vector3 playerPos;
public int currentMoves = 6;
int movesCheck =0;
int maxMoves = 6;
public TextMeshProUGUI movesText;
    int playerX ;
    int playerY ;
public ElementState playerElement = ElementState.Base;

    void Start()
    {
         movesText.text = currentMoves.ToString();
    }

    public bool CanModifyMove(int val)
    {
       movesCheck = currentMoves;
        if ((movesCheck+=val) < 0)
        return false;
        else
        return true;
    }

    public void ModifyMoves(int val)
    {
        currentMoves += val;
        
        if (currentMoves > maxMoves)
        {
            currentMoves = maxMoves;
        };
         movesText.text = currentMoves.ToString();
    }

 

public void SetPlayerCords(Vector2Int newCords, Vector3 newPos)
    {
        playerCords = newCords;
        playerPos = newPos;
        gameObject.transform.position = newPos;
    }

public Vector2Int GetPlayerCords()
    {
        return playerCords;
    }


    
    public bool CheckAdjacencyOnPlayer(Vector2Int CurrentPlayerCords, Vector2Int newCords)
    {    
        playerCords = CurrentPlayerCords;
        playerX = CurrentPlayerCords.x;
        playerY = CurrentPlayerCords.y;
        
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
