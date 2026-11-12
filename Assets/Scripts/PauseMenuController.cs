 using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class PauseMenuController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;   // Drag your main 'Panel' here
    public GameObject settingsPanel; // Drag your 'Settings panel' here

    public static bool GameIsPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false); // Ensure settings is hidden at start
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // If settings is open, close it first
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        GameIsPaused = false;
    }

    // --- NEW METHODS FOR SETTINGS ---

    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        pauseMenuUI.SetActive(false);   // Hide the main pause menu
        settingsPanel.SetActive(true);  // Show the settings menu
    }

    public void CloseSettings()
    {
        Debug.Log("Closing Settings...");
        settingsPanel.SetActive(false); // Hide the settings menu
        pauseMenuUI.SetActive(true);    // Show the main pause menu
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
        Application.Quit(); 
    }
}
