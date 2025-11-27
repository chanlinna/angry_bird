using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int SessionHighScore = 0;
    private int currentRoundScore = 0;

    [Header("Game Setup")]
    public GameObject[] pigs;   // Assign all pigs in the scene
    public BirdManager birdManager; // Assign your BirdManager here

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        CheckWinLose();
    }

    private void CheckWinLose()
    {
        // Count alive pigs
        int alivePigs = 0;
        foreach (var pig in pigs)
        {
            if (pig != null) alivePigs++;
        }

        // WIN condition
        if (alivePigs == 0)
        {
            LoadWinScene();
        }
        // LOSE condition
        else if (alivePigs > 0 && birdManager.AllBirdsUsed())
        {
            LoadLoseScene();
        }
    }

    public void AddScore(int points)
    {
        currentRoundScore += points;
    }

    private void UpdateSessionHighScore(int finalScore)
    {
        // Only update if the score from the finished round is higher than the current session high
        if (finalScore > SessionHighScore)
        {
            SessionHighScore = finalScore;
            Debug.Log("New Session High Score: " + SessionHighScore);
        }
    }

    private void LoadWinScene()
    {
        // Update the Session High Score before loading the next scene
        UpdateSessionHighScore(currentRoundScore);

        // Clear the round score for the next game
        currentRoundScore = 0;
        SceneManager.LoadScene("WinScene"); // Replace with your Win scene name
    }

    private void LoadLoseScene()
    {
        UpdateSessionHighScore(currentRoundScore);
        currentRoundScore = 0;
        SceneManager.LoadScene("LoseScene"); // Replace with your Lose scene name
    }

}
