 using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class MainMenuController : MonoBehaviour
{
    // Method for the Play button
    public void PlayGame()
    {
        // IMPORTANT: Replace "GameScene" with the exact name of your actual game scene
        // Make sure your game scene is added to File > Build Settings > Scenes In Build
        SceneManager.LoadScene("GameScene"); 
    }

    // Method for the Settings button
    public void OpenSettings()
    {
        Debug.Log("Settings button clicked!");
        // If you have a settings panel, you would activate it here, like:
        // settingsPanel.SetActive(true);
    }

    // Method for the Help button
    public void OpenHelp()
    {
        Debug.Log("Help button clicked!");
        // If you have a help panel, you would activate it here, like:
        // helpPanel.SetActive(true);
    }

    // Method for the Exit button
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        // This stops the game when you play it in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        // This quits the game when it is built
        Application.Quit();
    }
}
