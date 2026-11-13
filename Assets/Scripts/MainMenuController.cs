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
        Time.timeScale = 1f;

        // Ensure panels are closed at the start
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Final Main Level"); 
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
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    // --- CLOSE BUTTONS ---
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