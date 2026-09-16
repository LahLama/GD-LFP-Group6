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
    public ElementState tileElement = ElementState.Base;
    private bool MadeAMove = false;
    private PlayerProperties playerProperties;
    private int tileMoveCost = -1;
    private int obstacleMoveCost = -2;
    public int waterObstacleMoveCost = -4;
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
                case ElementState.Base:
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
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapBase");
                    break;
            }
        }

        else if (tileState == TileState.Obstacle)
        {
            switch (tileElement)
            {
                case ElementState.Base:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleBase");
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
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleBase");
                    break;
            }
        }
        else if (tileState == TileState.MoveAdd)
        {
            tileMaterial = Resources.Load<Material>("Materials/Achetypes/moveAdd");
        }
        else if (tileState == TileState.Wall)
        {
            tileMaterial = Resources.Load<Material>("Materials/Achetypes/wall");
        }
        else if (tileState == TileState.EndPoint)
        {
        tileMaterial = Resources.Load<Material>("Materials/Achetypes/endPoint");    
        }
        else
        {
            tileMaterial = Resources.Load<Material>("Materials/Achetypes/baseTile");
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
        //if its a obstacle tile and its broken, remove the type and set it to a normal tile
        tileState = TileState.Tile;
        SetTileMaterial();
        }

// Check if the player has enough moves to move to the tile and if the tile is a MoveAdd tile
        else if (playerProperties.CanModifyMove(moveAddCost) && tileState == TileState.MoveAdd )
        {
        playerProperties.ModifyMoves(moveAddCost);
        MadeAMove = true;
        //if its a moveAdd tile, remove the type and set it to a normal tile
        tileState = TileState.Tile;
        SetTileMaterial();
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
// Check if the player tries to move to a Endpoint, dont move
        else if ( playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.EndPoint)
        {
        playerProperties.ModifyMoves(tileMoveCost);
        MadeAMove = true;
        Debug.Log("YOU WIN****************************************");
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
