using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BatteryCore : MonoBehaviour
{
    public int pointsOnSocket = 1;

    private GameManager gameManager;
    private bool isSocketed = false;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // This function is called by the SortingSocket script
    public void OnSocketed()
    {
        // Check if it's already been socketed to prevent scoring twice
        if (isSocketed) return;

        isSocketed = true;

        if (gameManager != null)
        {
            gameManager.AddPoint(pointsOnSocket);
        }

        // Disable this script so it can't be used again
        this.enabled = false;
    }
}