using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NormalCore : MonoBehaviour
{
    public int pointsOnSocket = 1;

    private GameManager gameManager;
    private bool isSocketed = false;

    void Awake()
    {
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    // Called by the SortingSocket 
    public void OnSocketed()
    {
        if (isSocketed) return;
        isSocketed = true;

        if (gameManager != null)
        {
            gameManager.AddPoint(pointsOnSocket);
        }

        // This makes the core "stick" and become non-grabbable
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        // Disable this script so it can't be used again
        this.enabled = false;
    }
}
