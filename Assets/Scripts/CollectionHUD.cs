 using UnityEngine;
using UnityEngine.UI; 

public class CollectionHUD : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The parent object holding the beam and the icon.")]
    public GameObject hudContainer; // NEW: Reference to the whole panel

    [Tooltip("The Image component in the center of the beam.")]
    public Image centerIcon; 

    [Tooltip("The glowing tube Image.")]
    public Image beamGlow; 

    private void Start()
    {
        // Ensure the HUD starts completely hidden when the level loads
        ResetHUD();
    }

    // This method is called by your TileFunctions script
    public void OnElementCollected(Sprite collectedSprite)
    {
        if (centerIcon != null)
        {
            // 1. Turn the whole panel ON
            if (hudContainer != null) hudContainer.SetActive(true);

            // 2. Change the sprite
            centerIcon.sprite = collectedSprite;
            
            // 3. Make the icon visible
            centerIcon.gameObject.SetActive(true);

            // Optional: Make the beam flash
            if (beamGlow != null)
            {
                beamGlow.color = new Color(beamGlow.color.r, beamGlow.color.g, beamGlow.color.b, 1f); 
            }

            Debug.Log("Icon updated and HUD shown: " + collectedSprite.name);
        }
    }

    // Call this when a level completes or the player restarts
    public void ResetHUD()
    {
        // 1. Clear the icon
        if (centerIcon != null)
        {
            centerIcon.sprite = null;
        }

        // 2. Turn the whole panel OFF
        if (hudContainer != null)
        {
            hudContainer.SetActive(false); 
        }
    }
}