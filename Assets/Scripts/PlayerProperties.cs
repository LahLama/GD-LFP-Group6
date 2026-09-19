using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerProperties : MonoBehaviour
{
public Vector2Int playerCords = new Vector2Int(1,1);
public int currentMoves = 6;
int movesCheck =0;
public int maxMoves = 6;
public TextMeshProUGUI movesText;
public Vector3 playerPos;

[SerializeField] Vector2Int respawnPoint;
[SerializeField] int respawnMoves;
[SerializeField] Vector3 respawnTransform;
bool hasRespawned = false;

public ElementState playerElement = ElementState.Base;

    void Start()
    {
        
        respawnPoint = playerCords;
        respawnMoves = maxMoves;
        respawnTransform = gameObject.transform.position;
         movesText.text = currentMoves.ToString();
        respawnPoint = playerCords;
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
        }
        //Current problem if the moves used to get an obstcle is at the same time they get to respawn.
        else if( currentMoves <= 0)
        {
            RespawnPlayer();
        }
         movesText.text = currentMoves.ToString();
    }

 

public void SetPlayerCords(Vector2Int newCords, Vector3 newPos)
    {
        if (hasRespawned)
        {
            // A respawn already placed the player this turn.
            hasRespawned = false;
            return;
        }
        playerCords = newCords;
        playerPos = newPos;
        gameObject.transform.position = newPos;
    }

public Vector2Int GetPlayerCords()
    {
        return playerCords;
    }
public void UpdateRespawnPoint(Vector2Int NewRespawnPoint, int NewRespawnMoves, Vector3 NewRespawnTransform)
    {
        respawnPoint = NewRespawnPoint;
        respawnMoves = NewRespawnMoves;
        respawnTransform = NewRespawnTransform;
    }
    
public void RespawnPlayer()
    {
        currentMoves = respawnMoves;
       
        playerCords = respawnPoint;
        this.transform.position = respawnTransform;
        hasRespawned = true;
        // Update the text counter
         movesText.text = currentMoves.ToString();

    }
}
