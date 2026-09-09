using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
public Vector2Int playerCords = new Vector2Int(1,1);
public Vector3 playerPos;
public int currentMoves = 6;
int maxMoves = 6;

public void ModifyMoves(int val)
    {
        currentMoves += val;
        if (currentMoves > maxMoves)
        {
            currentMoves = maxMoves;
        }
        else if (currentMoves < 0)
        {
            currentMoves = 0;
        }   
    }

public void SetPlayerCords(Vector2Int newCords, Vector3 newPos)
    {
        playerCords = newCords;
        playerPos = newPos;
        gameObject.transform.position = newPos;
    }

public Vector2 GetPlayerCords()
    {
        return playerCords;
    }
}
