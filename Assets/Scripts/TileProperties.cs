using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class TileProperties : MonoBehaviour
{
    private int row = 0;
    private int col = 0;
    public Vector2Int cords = new Vector2Int(0,0);
        public Vector2Int newCords = new Vector2Int(0,0);
    // public Material tileMaterial;

   
    public TileState tileState = TileState.Tile;
    public ElementState tileElement = ElementState.Tipid;
    private bool MadeAMove = false;
    private PlayerProperties playerProperties;
    private int tileMoveCost = -1;
    private int obstacleMoveCost = -2;
    private int moveAddCost = +2;




    void Start()
    {
        SetTileMaterial();

        playerProperties = FindAnyObjectByType<PlayerProperties>();
        // Divide the cordinate vector
        row = cords.x;
        col = cords.y;
    }

    private void SetTileMaterial()
    {
        Material tileMaterial = GetComponent<Renderer>().material;
        if (tileState == TileState.Element)
        {
            switch (tileElement)
            {
                case ElementState.Tipid:
                // Load the material from the assets/materials folder
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapEarth");
                    break;
                case ElementState.Water:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapWater");
                   break;
                case ElementState.Earth:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapEarth");
                    break;
                case ElementState.Fire:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapFire");
                    break;
                case ElementState.Nature:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapNature");
                    break;
                default:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapTipid");
                    break;
            }
        }

        if (tileState == TileState.Obstacle)
        {
            switch (tileElement)
            {
                case ElementState.Tipid:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleTipid");
                    break;
                case ElementState.Water:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleWater");
                    break;
                case ElementState.Earth:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleEarth");
                    break;
                case ElementState.Fire:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleFire");
                    break;
                case ElementState.Nature:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleNature");
                    break;
                default:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleTipid");
                    break;
            }
        }
        if (tileState == TileState.MoveAdd)
        {
            tileMaterial = Resources.Load<Material>("Materials/Achetypes/moveAdd");
        }
        if (tileState == TileState.Wall)
        {
            tileMaterial = Resources.Load<Material>("Materials/Achetypes/wall");
        }
        this.GetComponent<Renderer>().material = tileMaterial;
    }

    public bool ExecuteType()
    {   
        MadeAMove = false; 
// Check if the player has enough moves to conquer the obstacle and if the player has the same element as the obstacle
        bool canConquerObstacle = playerProperties.CanModifyMove(obstacleMoveCost) 
                        && tileState == TileState.Obstacle
                        && playerProperties.playerElement == tileElement;
        

// Check if the player has enough moves to move to the tile
        if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Tile )
        {
            playerProperties.ModifyMoves(tileMoveCost);
            MadeAMove = true;
        }
    
// Check if the player has enough moves to conquer the obstacle and if the player has the same element as the obstacle
        else if (canConquerObstacle)
        {
        playerProperties.ModifyMoves(obstacleMoveCost);
        MadeAMove = true;
        }

// Check if the player has enough moves to move to the tile and if the tile is a MoveAdd tile
        else if (playerProperties.CanModifyMove(moveAddCost) && tileState == TileState.MoveAdd )
        {
        playerProperties.ModifyMoves(moveAddCost);
        MadeAMove = true;
        }

// Check if the player has enough moves to move to the tile and if the tile is an Element tile
        else if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Element)
        {
            playerProperties.ModifyMoves(tileMoveCost);
            playerProperties.playerElement = tileElement;
            MadeAMove = true;
        }
// Check if the player tries to move to a wall, dont move
        else if ( tileState == TileState.Wall)
        {
               MadeAMove = true;
        }
// If the player does not have enough moves to move to the tile, do not allow the player to move
        else 
        {
            MadeAMove = false;
        }
    // Debug.Log("Can move? " + MadeAMove);
    return MadeAMove;
        // Debug.Log("Current Moves: " + playerProperties.currentMoves );
        
    }




}
