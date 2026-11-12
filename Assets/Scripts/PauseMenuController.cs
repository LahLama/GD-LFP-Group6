 using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class PauseMenuController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;   // Drag your main 'Panel' here
    public GameObject settingsPanel; // Drag your 'settingpanelmain' here

    [Header("Canvas References (To force on top)")]
    public Canvas pauseCanvas;       // Drag the Canvas holding the Pause Menu here
    public Canvas settingsCanvas;    // Drag the Canvas holding Settings here

    public static bool GameIsPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false); 
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
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
        if (pauseMenuUI != null) 
        {
            pauseMenuUI.SetActive(true);
            
            // FORCE TO FRONT: Works for ALL levels
            if (pauseCanvas != null) 
            {
                pauseCanvas.sortingOrder = 999; 
            }
            pauseMenuUI.transform.SetAsLastSibling(); 
        }
        Time.timeScale = 0f; 
        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        GameIsPaused = false;
    }

    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        pauseMenuUI.SetActive(false);   
        settingsPanel.SetActive(true);  
        
        // FORCE TO FRONT
        if (settingsCanvas != null) 
        {
            settingsCanvas.sortingOrder = 1000; // Higher than pause menu
        }
        settingsPanel.transform.SetAsLastSibling();
    }

    public void CloseSettings()
    {
        Debug.Log("Closing Settings...");
        settingsPanel.SetActive(false); 
        pauseMenuUI.SetActive(true);    
        
        // FORCE TO FRONT
        if (pauseCanvas != null) 
        {
            pauseCanvas.sortingOrder = 999; 
        }
        pauseMenuUI.transform.SetAsLastSibling();
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