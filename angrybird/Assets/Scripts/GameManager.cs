using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    private void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene"); // Replace with your Win scene name
    }

    private void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScene"); // Replace with your Lose scene name
    }
}
