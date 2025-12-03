using UnityEngine;
using UnityEngine.Video;

public class GameManager2 : MonoBehaviour
{
    [Header("Win Condition")]
    [Tooltip("The GameObject holding the Video Player")]
    public GameObject videoScreenObject;

    private int occupiedSockets = 0;
    private const int TARGET_SOCKETS = 3;
    private bool gameWon = false;

    private VideoPlayer videoPlayer;

    void Start()
    {
        if (videoScreenObject != null)
        {
            videoPlayer = videoScreenObject.GetComponent<VideoPlayer>();
            videoScreenObject.SetActive(false); // Hide screen initially
        }
    }

    // --- CONNECT THIS TO SOCKET EVENTS ---
    public void SocketOccupied()
    {
        if (gameWon) return;

        occupiedSockets++;
        Debug.Log($"Sockets Filled: {occupiedSockets} / {TARGET_SOCKETS}");

        if (occupiedSockets >= TARGET_SOCKETS)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameWon = true;
        Debug.Log("ALL SOCKETS FILLED. PLAYING VIDEO.");

        if (videoScreenObject != null)
        {
            videoScreenObject.SetActive(true);

            if (videoPlayer != null)
            {
                videoPlayer.Play();
            }
        }
    }
}