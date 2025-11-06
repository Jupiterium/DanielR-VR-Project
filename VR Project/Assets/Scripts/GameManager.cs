using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public float totalTime = 120f; // Countdown timer for game loop
    public int score = 0;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    private float currentTime;
    private bool gameIsActive = false;

    void Start()
    {
        // Initialize UI
        scoreText.text = "Score: 0";
        currentTime = totalTime;
        gameIsActive = true;
    }

    void Update()
    {
        if (gameIsActive)
        {
            // --- Timer Countdown ---
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                gameIsActive = false;
                GameOver();
            }

            // --- Update UI ---
            timerText.text = string.Format("{0:00}:{1:00}",
                             Mathf.FloorToInt(currentTime / 60),
                             Mathf.FloorToInt(currentTime % 60));
        }
    }

    // Function to be called by the sockets when the cores get placed in
    public void AddPoint(int pointsToAdd)
    {
        if (!gameIsActive) return;

        score += pointsToAdd;
        scoreText.text = "Score: " + score;

        // Maybe add SFX
    }

    // This can be called by your UnstableCore
    public void LosePoint(int pointsToLose)
    {
        if (!gameIsActive) return;

        score -= pointsToLose;
        scoreText.text = "Score: " + score;

        // Maybe add SFX
    }

    void GameOver()
    {
        timerText.text = "GAME OVER";
        // Show a "Final Score" panel, etc.
    }
}