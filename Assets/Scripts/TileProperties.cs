using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProperties : MonoBehaviour
{
    private const string MaterialFolder = "Materials/Achetypes/";

    // ---------- Grid position ----------
    public Vector2Int cords = new Vector2Int(0, 0);
    public Vector2Int newCords = new Vector2Int(0, 0);
    private int row = 0;
    private int col = 0;
    public int radius =0;

    // ---------- Tile state ----------
    public TileState tileState = TileState.Tile;
    public ElementState tileElement = ElementState.Base;

    [SerializeField]
    private TileState preRespawnState = TileState.Tile;

    // ---------- Move costs ----------
    private int tileMoveCost = -1;
    private int obstacleMoveCost = -2;
    private int moveAddCost = +2;
    public int CustomMoveCost = 1;

    // ---------- References / runtime ----------
    private PlayerProperties playerProperties;
    private LevelManager levelManager;
    private TextMeshPro moveText;
    private bool MadeAMove = false;

    // =====================================================================
    // Setup
    // =====================================================================

    void OnEnable()
    {
        SetTileMaterial();

        playerProperties = FindAnyObjectByType<PlayerProperties>();
        levelManager = FindAnyObjectByType<LevelManager>();
        preRespawnState = tileState;

        // Split the coordinate vector
        row = cords.x;
        col = cords.y;

        if (tileState == TileState.Obstacle || tileState == TileState.Element)
        {
            CustomMoveCost *= -1;
            if (transform.childCount > 0)
                SetMoveText();
        }

        // Check if its a obstacle and electro, then enable the timing function
        if (tileState == TileState.Obstacle && tileElement == ElementState.Electro)
        {
            InvokeRepeating(nameof(StartElectroPulse), 5f, 5f);
        }

        if (tileState == TileState.MoveAdd)
        {
            CustomMoveCost *= 1;
            SetMoveText();
        }

        if (tileState == TileState.StartTile)
        {
            playerProperties.UpdateRespawnPoint(cords, playerProperties.maxMoves, transform.position);
            playerProperties.SetPlayerCords(cords, transform.position);
            playerProperties.RespawnPlayer();
        }

        SetTileRadius();
    }

    private void StartElectroPulse()
    {
        FindAnyObjectByType<TileFunctions>().ElectroPulse(this);
    }

    private void SetMoveText()
    {
        transform.GetChild(0).GetComponent<TextMeshPro>().text = CustomMoveCost.ToString();
    }

    // =====================================================================
    // Respawn state
    // =====================================================================

    public void CacheTilesState()
    {
        preRespawnState = tileState;
    }

    public void RefreshTilesOnRespawn()
    {
        tileState = preRespawnState;
        SetTileMaterial();
    }

    // =====================================================================
    // Material
    // =====================================================================

    private void SetTileMaterial()
    {
        GetComponent<Renderer>().material = Resources.Load<Material>(MaterialFolder + GetMaterialName());
    }

    public void SetTileRadius()
    {
        
            if (tileElement == ElementState.Acid)
            {
                //Reverse the movement of the player
                radius = 4;
            }
            else if (tileElement == ElementState.Electro)
            {
                //Send out a 1 block shockwave out that respawns player
                radius = 1;
            }
            else if (tileElement == ElementState.Ice)
            {
                //Simple lock and Key puzzle, no effect here.
                // radius = 4;
            }
            else if (tileElement == ElementState.Fire)
            {
                //Has effected the map already, no effect here.
                // radius = 4;
            }
    }

    private string GetMaterialName()
    {
        switch (tileState)
        {
            case TileState.Element:
                switch (tileElement)
                {
                    case ElementState.Base:    return "swapElectro"; // Base intentionally uses the Electro material
                    case ElementState.Ice:     return "swapIce";
                    case ElementState.Electro: return "swapElectro";
                    case ElementState.Fire:    return "swapFire";
                    case ElementState.Acid:    return "swapAcid";
                    default:                   return "swapBase";
                }

            case TileState.Obstacle:
                switch (tileElement)
                {
                    case ElementState.Base:    return "obstacleBase";
                    case ElementState.Ice:     return "obstacleIce";
                    case ElementState.Electro: return "obstacleElectro";
                    case ElementState.Fire:    return "obstacleFire";
                    case ElementState.Acid:    return "obstacleAcid";
                    default:                   return "obstacleBase";
                }

            case TileState.OneWay:    return "obstacleBase";
            case TileState.MoveAdd:   return "moveAdd";
            case TileState.Wall:      return "wall";
            case TileState.EndPoint:  return "endPoint";
            case TileState.StartTile: return "startPoint";
            default:                  return "baseTile";
        }
    }

    // =====================================================================
    // Move handling
    // =====================================================================

    public bool ExecuteType()
    {
        MadeAMove = false;

        // If the previous tile was a OneWay, it disappears
                if (playerProperties.previousTile != null){
        if (playerProperties.previousTile.GetComponent<TileProperties>().tileState == TileState.OneWay){
            Debug.Log("Previous tile was DISAPPEAR ");
            playerProperties.previousTile.gameObject.SetActive(false);
        }
                }

        // Player needs enough moves AND the same element as the obstacle to conquer it
        bool canConquerObstacle = playerProperties.CanModifyMove(CustomMoveCost)
                                  && tileState == TileState.Obstacle
                                  && playerProperties.playerElement == tileElement;

        // -------- Normal Tile --------
        if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Tile)
        {
            playerProperties.ModifyMoves(tileMoveCost);
            MadeAMove = true;
        }
        // -------- Element Tile --------
        else if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Element)
        {
            playerProperties.ModifyMoves(tileMoveCost);
            playerProperties.playerElement = tileElement;
            playerProperties.UpdatePlayerColor();

            MadeAMove = true;
        }
        // -------- End Point Tile --------
        // Player doesn't move onto the endpoint; the next level takes over
        else if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.EndPoint)
        {
            playerProperties.ModifyMoves(tileMoveCost);
            levelManager.NextLevel();   // new level's StartTile places the player
            Debug.Log("YOU WIN");
            MadeAMove = false;          // don't let InteractionManager move the player onto this old tile
        }
        // -------- Move Adder Tile --------
        else if (playerProperties.CanModifyMove(CustomMoveCost) && tileState == TileState.MoveAdd)
        {
            playerProperties.ModifyMoves(CustomMoveCost);
            MadeAMove = true;

            SetTileMaterial();
        }
        // -------- Obstacle Tile --------
        else if (canConquerObstacle)
        {
            playerProperties.ModifyMoves(CustomMoveCost);
            MadeAMove = true;

//only if it has a post capture effect:
            if (tileElement == ElementState.Acid)
            {
                //Reverse the movement of the player
            }
            else if (tileElement == ElementState.Electro)
            {
                //Send out a 1 block shockwave out that respawns player
            }
            else if (tileElement == ElementState.Ice)
            {
                //Simple lock and Key puzzle, no effect here.
            }
            else if (tileElement == ElementState.Fire)
            {
                //Has effected the map already, no effect here.
            }

            // Obstacle is broken: turn it into a normal tile
            tileState = TileState.Tile;
            SetTileMaterial();
            playerProperties.UpdateRespawnPoint(cords, playerProperties.currentMoves, transform.position);
        }
        // -------- Wall Tile --------
        else if (tileState == TileState.Wall)
        {
            MadeAMove = true;
        }
        // -------- One Way Tile --------
        else if (tileState == TileState.OneWay)
        {
            MadeAMove = true;
        }
        // -------- Start Tile --------
        else if (tileState == TileState.StartTile)
        {
            MadeAMove = true;
        }
        // Not enough moves: the player can't move here
        else
        {
            MadeAMove = false;
        }

        playerProperties.previousTile = gameObject;
        return MadeAMove;
    }


}