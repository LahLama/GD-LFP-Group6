using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
public Vector2Int playerCords = new Vector2Int(1,1);
public Vector3 playerPos;

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
