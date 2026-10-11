using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.Tilemaps;

public class TileFunctions : MonoBehaviour
{

    TileProperties CurrentTile; // DEBUG
    Material tileMaterial; // DEBUG
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
        if (tile.tileState == TileState.Obstacle && tile.tileElement == ElementState.Acid)
        {
        Debug.Log("=-=-=-=-=-=ACID SPIT=-=-=-=-=-=-=-");
        bool isNearForAcid = CheckIfPlayerNearby(tile.radius, FindAnyObjectByType<PlayerProperties>().playerCords, tile.cords, tile) ;
        Debug.Log("NEAR ACID: " + isNearForAcid);
        FindAnyObjectByType<InteractionManager>().isMovementReversed = isNearForAcid; 
        }
        else
        {
            Debug.Log("Was testing on a" + tile.name + " tile, but it was not an ACID tile.",tile);
             FindAnyObjectByType<InteractionManager>().isMovementReversed = false;
        }
    }

    public void PerMoveChecker(int r,Vector2Int playerPos,Vector2Int tilePos, TileProperties tile)
    {   
        
        ReverseControls(tile);
        
        CheckIfPlayerNearby(r,playerPos,tilePos,tile);
    }


   public bool CheckIfPlayerNearby(int r,Vector2Int playerPos,Vector2Int tilePos,TileProperties tile)
    {
       //set the material to be red for 1 second, then back to normal.
       CurrentTile= tile; // DEBUG
       tileMaterial = tile.GetComponent<Renderer>().material;  // DEBUG
       tile.GetComponent<Renderer>().material.color = Color.red; // DEBUG
        Invoke("ResetTileColor", 1f); // DEBUG


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

    private void ResetTileColor() // DEBUG
    {
        if (CurrentTile != null)
        {
            CurrentTile.GetComponent<Renderer>().material = tileMaterial;
        }
    }
}
