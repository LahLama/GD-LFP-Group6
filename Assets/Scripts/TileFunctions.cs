using System.Collections;
using UnityEngine;

public class TileFunctions : MonoBehaviour
{

    TileProperties CurrentTile; // DEBUG
    Material tileMaterial; // DEBUG
    public void ElectroPulse(TileProperties tile, Vector3 initScale)
    {
    StartCoroutine(ElectroPulseRoutine(tile,initScale));
    }

    
    private IEnumerator ElectroPulseRoutine(TileProperties tile, Vector3 initScale)
    {
        //if tile is defeated
    if (tile.tileState != TileState.Obstacle)
    {
        // Debug.LogWarning("ElectroPulse called on a non-obstacle tile: " + tile.name, tile);
        yield break;
    }
    GameObject AOE = tile.transform.GetChild(2).gameObject;
    
    const float pulseDuration = 5f - 1f; //Minus 1 for the delay before the pulse starts, which is 1 second.

    PlayerProperties player = FindAnyObjectByType<PlayerProperties>();

    while (tile.tileState == TileState.Obstacle) // stop automatically if the tile is changed
    {
        // Start each pulse from zero
        AOE.transform.localScale = Vector3.zero;
        float elapsed = 0f;

        // Grow from 0 -> initScale over 5 seconds
        while (elapsed < pulseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / pulseDuration);
            AOE.transform.localScale = Vector3.Lerp(Vector3.zero, initScale, t);
            yield return null;
        }

        // Make sure it ends exactly at full size
        AOE.transform.localScale = initScale;

        // Pulse peaks: check for the player
        Debug.Log("**********MAXIMUM PULSE***********");
        bool isNearForShock = CheckIfPlayerNearby(tile.radius, player.playerCords, tile.cords, tile);
        Debug.Log("NEAR SHOCK: " + isNearForShock);

        if (isNearForShock)
        {
            Debug.Log("HIT PLAYER TO RESPAWN");
            player.RespawnPlayer();
        }

        yield return new WaitForSeconds(1f); // Wait for 1 second at full size before shrinking
    }
    }

    public void SetOneWayTile(TileProperties tile)
    {
        if (tile.tileState == TileState.OneWay)
        {
            
        }
    }
private bool IsReversingControls(TileProperties tile)
{
    if (tile.tileState != TileState.Obstacle || tile.tileElement != ElementState.Acid)
        return false;

    PlayerProperties player = FindAnyObjectByType<PlayerProperties>();
    return CheckIfPlayerNearby(tile.radius, player.playerCords, tile.cords, tile);
}

// Returns true if this tile is a live acid obstacle with the player in range
public bool PerMoveChecker(int r, Vector2Int playerPos, Vector2Int tilePos, TileProperties tile)
{
    bool reversing = IsReversingControls(tile);
    CheckIfPlayerNearby(r, playerPos, tilePos, tile); // existing debug flash
    return reversing;
}

   public bool CheckIfPlayerNearby(int r,Vector2Int playerPos,Vector2Int tilePos,TileProperties tile)
    {
       //set the material to be red for 1 second, then back to normal.
    //    CurrentTile= tile; // DEBUG
    //    tileMaterial = tile.GetComponent<Renderer>().material;  // DEBUG
    //    tile.GetComponent<Renderer>().material.color = Color.red; // DEBUG
        // Invoke("ResetTileColor", 1f); // DEBUG


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
