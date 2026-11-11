 using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;  // The object holding Play, Settings, Help, Exit buttons
    public GameObject settingsPanel;  // The Settings panel
    public GameObject helpPanel;      // The Help panel

    void Start()
    {
        // Make sure the game starts with normal time (in case it was paused before)
        Time.timeScale = 1f;

        // Ensure panels are closed at the start
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);
    }

    // --- MAIN MENU BUTTONS ---

    public void PlayGame()
    {
        // Replace "GameScene" with your actual game scene name
        SceneManager.LoadScene("GameScene"); 
    }

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void OpenHelp()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    // --- CLOSE BUTTONS (Inside the Settings and Help panels) ---

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        if (helpPanel != null) helpPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
}
