using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.Tilemaps;

public class TileFunctions : MonoBehaviour
{


    public void ElectroPulse(TileProperties tile)
    {
        // The pulse happens every 5 seconds
        Debug.Log("**********MAXIMUM PULSE***********");
        bool isNearForShock = CheckIfPlayerNearby(tile.radius, FindAnyObjectByType<PlayerProperties>().playerCords, tile.cords, tile) ;
        Debug.Log("NEAR SHOCK: " + isNearForShock);
        if (isNearForShock)
        {
            Debug.Log("HIT PLAYER TO RESPAWN");
 FindAnyObjectByType<PlayerProperties>().RespawnPlayer();
        }
    }

    private void ReverseControls(TileProperties tile)
    {
        //This will be checked every move to see if the player is still in range.
        
        Debug.Log("=-=-=-=-=-=ACID SPIT=-=-=-=-=-=-=-");
        bool isNearForAcid = CheckIfPlayerNearby(tile.radius, FindAnyObjectByType<PlayerProperties>().playerCords, tile.cords, tile) ;
        Debug.Log("NEAR ACID: " + isNearForAcid);
        if (CheckIfPlayerNearby(tile.radius, FindAnyObjectByType<PlayerProperties>().playerCords, tile.cords, tile))
        {
            FindAnyObjectByType<InteractionManager>().isMovementReversed = !FindAnyObjectByType<InteractionManager>().isMovementReversed; 
            Debug.Log(FindAnyObjectByType<InteractionManager>().isMovementReversed);
            Debug.Log("REVERSE THE CONTROLE");
        }
    }

    public void PerMoveChecker(int r,Vector2Int playerPos,Vector2Int tilePos, TileProperties tile)
    {   
        if (tile.tileElement == ElementState.Acid)
        ReverseControls(tile);
        
        CheckIfPlayerNearby(r,playerPos,tilePos,tile);
    }


   public bool CheckIfPlayerNearby(int r,Vector2Int playerPos,Vector2Int tilePos,TileProperties tile)
    {
       
        // Debug.Log("Player Cords: "+playerPos);
        for (int i = -r; i < r+1; i++)
        {
            for (int j = -r; j < r+1; j++)
            {
                Vector2Int currentCords = new Vector2Int(tilePos.x + i, tilePos.y+j);
                // Debug.Log(currentCords);
                if (playerPos == currentCords && tile.tileState == TileState.Obstacle)
                {
                // Debug.Log("Player is nearby the " + tile.name + " tile");
               return true;
               
                }
                
                

            }
        }


        return false;
    }
}
