using UnityEngine;

public class GameFunctions : MonoBehaviour
{
    public void LeaveGame()
    {
        Application.Quit();
    }

    public void GotElement(ElementState TileElement)
    {
        //Whatever codes goes here
        Debug.Log("the player got this element: " + TileElement);
    }

}

