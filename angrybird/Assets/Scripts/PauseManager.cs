using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Tooltip("The entire Pause Menu Canvas GameObject.")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the UI
        Time.timeScale = 1f;          // Resume normal speed (VERY IMPORTANT)
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);  // Show the UI
        Time.timeScale = 0f;          // Freeze the game (VERY IMPORTANT)
        isPaused = true;
    }

    public void ReturnToMainMenu()
    {
        // Must reset time scale before loading the next scene!
        Time.timeScale = 1f;

        SceneManager.LoadScene("StartMenu");
    }
}
