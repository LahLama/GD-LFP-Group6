using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int moveAddCost = +2;
    public int CustomMoveCost = 0;
    TextMeshPro moveText;
    [SerializeField]
    TileState preRespawnState = TileState.Tile;




    void OnEnable()
    {
        SetTileMaterial();

        playerProperties = FindAnyObjectByType<PlayerProperties>();
        preRespawnState = tileState;
        
        // Divide the cordinate vector
        row = cords.x;
        col = cords.y;

        if (tileState == TileState.Obstacle || tileState == TileState.Element)
            CustomMoveCost *= -1;

        if (tileState == TileState.MoveAdd)
            CustomMoveCost *= 1;

        if(tileState == TileState.StartTile)
        {
            playerProperties.SetPlayerCords(cords,this.transform.position);
        }
    }
 
 public void CacheTilesState()
    {
        preRespawnState = tileState;
    }

 public void RefreshTilesOnRespawn()
    {
        tileState = preRespawnState;
        SetTileMaterial();
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
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapElectro");
                    break;
                case ElementState.Ice:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapIce");
                   break;
                case ElementState.Electro:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapElectro");
                    break;
                case ElementState.Fire:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapFire");
                    break;
                case ElementState.Acid:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/swapAcid");
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
                case ElementState.Ice:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleIce");
                    break;
                case ElementState.Electro:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleElectro");
                    break;
                case ElementState.Fire:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleFire");
                    break;
                case ElementState.Acid:
                    tileMaterial = Resources.Load<Material>("Materials/Achetypes/obstacleAcid");
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
        else if (tileState == TileState.StartTile)
        {
        tileMaterial = Resources.Load<Material>("Materials/Achetypes/startPoint");    
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
        bool canConquerObstacle = playerProperties.CanModifyMove(CustomMoveCost) 
                        && tileState == TileState.Obstacle
                        && playerProperties.playerElement == tileElement;
        
// -------- Normal Tile ---------
// Check if the player has enough moves to move to the tile
        if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Tile )
        {
            playerProperties.ModifyMoves(tileMoveCost);
            MadeAMove = true;
        }
// -------- Element Tile --------    
// Check if the player has enough moves to move to the tile and if the tile is an Element tile
        else if (playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.Element)
        {
            playerProperties.ModifyMoves(tileMoveCost);
            playerProperties.playerElement = tileElement;
            playerProperties.UpdatePlayerColor();
            
            MadeAMove = true;
        }
//-------- End Point Tile --------        
// Check if the player tries to move to a Endpoint, dont move
        else if ( playerProperties.CanModifyMove(tileMoveCost) && tileState == TileState.EndPoint)
        {
        playerProperties.ModifyMoves(tileMoveCost);
        MadeAMove = true;
        //Load the next scene in the build
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("YOU WIN****************************************");
        }

//-------- Move Adder Tile --------
// Check if the player has enough moves to move to the tile and if the tile is a MoveAdd tile
        else if (playerProperties.CanModifyMove(CustomMoveCost) && tileState == TileState.MoveAdd )
        {
        playerProperties.ModifyMoves(CustomMoveCost);
        MadeAMove = true;
        //if its a moveAdd tile, remove the type and set it to a normal tile
        tileState = TileState.Tile;
        SetTileMaterial();
        }

// -------- Obstacle Tile --------
// Check if the player has enough moves to conquer the obstacle and if the player has the same element as the obstacle
        else if (canConquerObstacle)
        {
        
        playerProperties.ModifyMoves(CustomMoveCost);
        MadeAMove = true;
        //if its a obstacle tile and its broken, remove the type and set it to a normal tile
        tileState = TileState.Tile;
        SetTileMaterial();
        playerProperties.UpdateRespawnPoint(cords,playerProperties.currentMoves,transform.position);
        }

// -------- Wall Tile --------        
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
