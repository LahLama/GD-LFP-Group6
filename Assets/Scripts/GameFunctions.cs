 using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class GameFunctions : MonoBehaviour
{
    [Header("HUD Setup")]
    [Tooltip("The parent object holding the blue beam and icon.")]
    public GameObject hudContainer; 
    public Image centerIcon; 
    public Image beamGlow; 

    [Header("Element Icons")]
    public Sprite fireIcon;
    public Sprite iceIcon;     
    public Sprite acidIcon;
    public Sprite electroIcon; 

    // --- LEVEL & SCENE MANAGEMENT ---
    private void OnEnable()
    {
        // Automatically triggers when a new level loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hide the HUD immediately when a new level starts
        ResetHUD();
    }

    private void Start()
    {
        // Hide the HUD when the game first starts
        ResetHUD();
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    // --- ELEMENT COLLECTION ---
    public void GotElement(ElementState TileElement)
    {
        // Debug.Log("the player got this element: " + TileElement);

        // If the player respawns to Base (no element), force the HUD to hide
        if (TileElement == ElementState.Base)
        {
            ResetHUD();
            return; 
        }

        Sprite iconToShow = null;

        // Match your exact elements: Ice, Acid, Fire, Electro
        switch (TileElement)
        {
            case ElementState.Fire:
                iconToShow = fireIcon;
                break;
            case ElementState.Ice:
                iconToShow = iceIcon;
                break;
            case ElementState.Acid:
                iconToShow = acidIcon;
                break;
            case ElementState.Electro:
                iconToShow = electroIcon;
                break;
            default:
                Debug.LogWarning("No icon set up for element: " + TileElement);
                break;
        }

        // If we found an icon, show it on the HUD!
        if (iconToShow != null)
        {
            ShowHUD(iconToShow);
        }
        else
        {
            // If somehow no icon matches, just hide it to be safe
            ResetHUD(); 
        }
    }

    // --- VISUAL UI LOGIC ---
    private void ShowHUD(Sprite collectedSprite)
    {
        if (centerIcon != null)
        {
            // 1. Turn the whole panel ON (makes the blue beam appear)
            if (hudContainer != null) hudContainer.SetActive(true);

            // 2. Change the sprite in the middle
            centerIcon.sprite = collectedSprite;
            
            // 3. Make the icon visible
            centerIcon.gameObject.SetActive(true);

            // Optional: Make the beam flash brightly
            if (beamGlow != null)
            {
                beamGlow.color = new Color(beamGlow.color.r, beamGlow.color.g, beamGlow.color.b, 1f); 
            }
        }
    }

    public void ResetHUD()
    {
        // 1. Clear the icon
        if (centerIcon != null)
        {
            centerIcon.sprite = null;
        }

        // 2. Turn the whole panel OFF (hides the blue beam completely)
        if (hudContainer != null)
        {
            hudContainer.SetActive(false); 
        }
    }
}