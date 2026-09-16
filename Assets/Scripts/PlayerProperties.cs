using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
public Vector2Int playerCords = new Vector2Int(1,1);
public int currentMoves = 6;
int movesCheck =0;
int maxMoves = 6;
public TextMeshProUGUI movesText;
public Vector3 playerPos;

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


}
