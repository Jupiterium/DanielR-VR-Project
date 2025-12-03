using UnityEngine;
// We removed UnityEngine.Video since we don't need it anymore

public class SimpleGameManager : MonoBehaviour
{
    [Header("Win Condition")]
    [Tooltip("The UI Object to turn on when you win (e.g., a Canvas or Panel)")]
    public GameObject winUIObject;

    private int occupiedSockets = 0;
    private const int TARGET_SOCKETS = 3;
    private bool gameWon = false;

    void Start()
    {
        // Ensure the Win UI is hidden when the game starts
        if (winUIObject != null)
        {
            winUIObject.SetActive(false);
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
        Debug.Log("ALL SOCKETS FILLED. SHOWING WIN UI.");

        // Simply turn on the UI object
        if (winUIObject != null)
        {
            winUIObject.SetActive(true);
        }
    }
}