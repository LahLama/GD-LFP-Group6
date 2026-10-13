 using UnityEngine;
using System.Collections; // Required for Coroutines

public class AutoHidePanel : MonoBehaviour
{
    public float displayTime = 10f; // Time in seconds

    void Start()
    {
        // Start the timer when the game starts
        StartCoroutine(HidePanelRoutine());
    }

    // Alternatively, use OnEnable() instead of Start() if you want the 
    // 10-second timer to restart every time the panel is turned back on.
    /*
    void OnEnable()
    {
        StartCoroutine(HidePanelRoutine());
    }
    */

    IEnumerator HidePanelRoutine()
    {
        // Wait for the specified amount of seconds
        yield return new WaitForSeconds(displayTime);

        // Hide the panel by deactivating the GameObject
        gameObject.SetActive(false);
    }
}
