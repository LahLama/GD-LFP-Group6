using UnityEngine;
using UnityEngine.InputSystem;
public class TileProperties : MonoBehaviour
{
    private int row = 0;
    private int col = 0;
    public Vector2 cords = new Vector2(0,0);
    public Vector2 playerCords = new Vector2(0,0);
    public Vector2 newCords = new Vector2(0,0);
    public string type = "tile";
    
	Ray ray;
	RaycastHit hit;


    void Start()
    {
        // Divide the cordinate vector
        row = (int)cords.x;
        col = (int)cords.y;


    }

// Create a region
#region InputSystem
    
    InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
#endregion
    

    bool CheckAdjacencyOnPlayer(Vector2 playerCords, Vector2 newCords)
    {   
        int playerX = (int)playerCords.x;
        int playerY = (int)playerCords.y;
        // Check if the player can move to an adjacent tile

        if (newCords.x == playerX && newCords.y == playerY + 1)  // Move - Up 
            return true;
        else if (newCords.x == playerX && newCords.y == playerY - 1) // Move - Down
            return true;
        else if (newCords.x == playerX + 1 && newCords.y == playerY) // Move - Right
            return true;
        else if (newCords.x == playerX - 1 && newCords.y == playerY) // Move - Left
            return true;
        else if (newCords.x == playerX + 1 && newCords.y == playerY + 1) // Move - Up Right
            return true;
        else if (newCords.x == playerX - 1 && newCords.y == playerY - 1) // Move - Down Left
            return true;
        else if (newCords.x == playerX + 1 && newCords.y == playerY - 1) // Move - Down Right
            return true;
        else if (newCords.x == playerX - 1 && newCords.y == playerY + 1) // Move - Up Left
            return true;
        else     // Not Adjacent
            return false;
    }


    void Update()
	{
		ray = Camera.main.ScreenPointToRay(inputActions.UI.Point.ReadValue<Vector2>());
		if(Physics.Raycast(ray, out hit))
		{
			if(inputActions.Player.Attack.WasCompletedThisDynamicUpdate()){
				newCords = hit.collider.gameObject.GetComponent<TileProperties>().cords;
                Debug.Log(CheckAdjacencyOnPlayer(playerCords, newCords));
                }
		}
	}
}
