using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public float totalTime = 120f; // 2 minutes
    public int score = 0;
    private int totalCoresToWin = 3; // The game will end when score reaches this

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    [Header("Spawning")]
    public List<GameObject> corePrefabs;
    public List<Transform> spawnPoints;

    private float currentTime;
    private bool gameIsActive = false;

    void Start()
    {
        // Start the game loop
        gameIsActive = true;
        currentTime = totalTime;
        score = 0;
        scoreText.text = "Score: 0";
        UpdateTimerText(); // Set the timer text on the first frame

        // Spawn cores
        SpawnCores();
    }

    void Update()
    {
        if (gameIsActive)
        {
            // Timer Countdown
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                gameIsActive = false;
                GameOver();
            }
            UpdateTimerText();
        }
    }

    void SpawnCores()
    {
        for (int i = 0; i < corePrefabs.Count; i++)
        {
            if (i < spawnPoints.Count)
            {
                Instantiate(corePrefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    }

    // Called by the SortingSocket
    public void AddPoint(int pointsToAdd)
    {
        if (!gameIsActive) return;

        score += pointsToAdd;
        scoreText.text = "Score: " + score;

        if (score >= totalCoresToWin)
        {
            WinGame();
        }
    }

    // Called by UnstableCore and FragileCore
    public void LosePoint(int pointsToLose)
    {
        if (!gameIsActive) return;

        score += pointsToLose;
        //if (score < 0) score = 0;

        scoreText.text = "Score: " + score;
    }

    void GameOver()
    {
        gameIsActive = false;
        timerText.text = "Game Over!";
    }

    void WinGame()
    {
        gameIsActive = false;
        timerText.text = "You Win!";
    }

    void UpdateTimerText()
    {
        timerText.text = string.Format("{0:00}:{1:00}",
                         Mathf.FloorToInt(currentTime / 60),
                         Mathf.FloorToInt(currentTime % 60));
    }
}