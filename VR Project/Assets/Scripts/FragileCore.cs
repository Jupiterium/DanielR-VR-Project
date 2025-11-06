using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FragileCore : MonoBehaviour
{
    public float breakThreshold = 2.5f; // Impact speed that will cause the shatter
    public int pointsOnBreak = -1;
    public int pointsOnSocket = 1;

    // Might glass/crystal material and shatter particle effect here

    private GameManager gameManager;
    private Rigidbody rb;
    private bool isSocketed = false;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // If we hit something and are not yet socketed
        if (!isSocketed)
        {
            // Check the speed of the impact
            if (collision.relativeVelocity.magnitude > breakThreshold)
            {
                BreakCore();
            }
        }
    }

    void BreakCore()
    {
        if (gameManager) gameManager.LosePoint(pointsOnBreak);
        Destroy(gameObject);
    }

    public void OnSocketed()
    {
        if (isSocketed) return;
        isSocketed = true;
        if (gameManager) gameManager.AddPoint(pointsOnSocket);
        // Disable collision checks
        rb.isKinematic = true; // Stop it from moving or breaking
        this.enabled = false;
    }
}