//using UnityEngine;
//using TMPro;

//public class GameManager : MonoBehaviour
//{
//    [Header("Game State")]
//    public float totalTime = 120f; // Countdown timer for game loop
//    public int score = 0;

//    [Header("UI References")]
//    public TextMeshProUGUI timerText;
//    public TextMeshProUGUI scoreText;

//    private float currentTime;
//    private bool gameIsActive = false;

//    void Start()
//    {
//        // Initialize UI
//        scoreText.text = "Score: 0";
//        currentTime = totalTime;
//        gameIsActive = true;
//    }

//    void Update()
//    {
//        if (gameIsActive)
//        {
//            // --- Timer Countdown ---
//            currentTime -= Time.deltaTime;
//            if (currentTime <= 0)
//            {
//                currentTime = 0;
//                gameIsActive = false;
//                GameOver();
//            }

//            // --- Update UI ---
//            timerText.text = string.Format("{0:00}:{1:00}",
//                             Mathf.FloorToInt(currentTime / 60),
//                             Mathf.FloorToInt(currentTime % 60));
//        }
//    }

//    // Function to be called by the sockets when the cores get placed in
//    public void AddPoint(int pointsToAdd)
//    {
//        if (!gameIsActive) return;

//        score += pointsToAdd;
//        scoreText.text = "Score: " + score;

//        // Maybe add SFX
//    }

//    // This can be called by your UnstableCore
//    public void LosePoint(int pointsToLose)
//    {
//        if (!gameIsActive) return;

//        score -= pointsToLose;
//        scoreText.text = "Score: " + score;

//        // Maybe add SFX
//    }

//    void GameOver()
//    {
//        timerText.text = "GAME OVER";

//        // TODO: show final score
//    }
//}


using UnityEngine;
using TMPro;
using System.Collections.Generic; 

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public float totalTime = 120f; // 2 minutes
    public int score = 0;
    private int tutorialCoresSocketed = 0;
    private int totalCoresToWin = 3;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    [Header("Spawning")]
    [Tooltip("The 3 REAL core prefabs to spawn")]
    public List<GameObject> corePrefabs;
    [Tooltip("The 3 spawn points on the rooftops")]
    public List<Transform> spawnPoints;

    private float currentTime;
    private bool gameIsActive = false;

    void Start()
    {
        // Do not start the timer yet.
        scoreText.text = "Score: 0";
        timerText.text = "POWER UP THE GRID"; // Tutorial prompt
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

    // This is called by the Sockets
    public void OnTutorialCoreSocketed()
    {
        if (gameIsActive) return; // Ignore if game already started

        tutorialCoresSocketed++;

        if (tutorialCoresSocketed >= 3)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameIsActive = true;
        currentTime = totalTime;
        score = 0;
        scoreText.text = "Score: 0";

        // Spawn cores for the game loop
        for (int i = 0; i < corePrefabs.Count; i++)
        {
            if (i < spawnPoints.Count)
            {
                // Spawn the core at the designated spawn point
                Instantiate(corePrefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    }

    // To be called by the sockets
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

    // To be called by the cores
    public void LosePoint(int pointsToLose)
    {
        if (!gameIsActive) return;

        score -= pointsToLose;
        if (score < 0) score = 0; 

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