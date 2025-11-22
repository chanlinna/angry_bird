using UnityEngine;
using TMPro; 

public class HighScoreDisplay : MonoBehaviour
{
    // Assign the TextMeshPro component showing the score value in the Inspector
    public TextMeshProUGUI scoreText;

    void Start()
    {
        // 1. Check if the scoreText reference is set
        if (scoreText == null)
        {
            Debug.LogError("Score Text component is not assigned to HighScoreDisplay script!");
            return;
        }

        // 2. Retrieve the static score from the GameManager
        int currentHighScore = GameManager.SessionHighScore;

        // 3. Display the score
        scoreText.text = currentHighScore.ToString();
    }
}
