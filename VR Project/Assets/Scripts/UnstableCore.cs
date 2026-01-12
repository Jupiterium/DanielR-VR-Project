using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnstableCore : MonoBehaviour
{
    public float coreLifetime = 15f; // Meltdown timer
    public int pointsOnDetonate = -1; // Lose 1 point
    public int pointsOnSocket = 1;

    private XRGrabInteractable grabInteractable;
    private GameManager gameManager;
    private float meltdownTimer;
    private bool isGrabbed = false;
    private bool isSocketed = false;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (isSocketed) return;
        isGrabbed = true;
        meltdownTimer = coreLifetime;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void Update()
    {
        if (isGrabbed && !isSocketed)
        {
            meltdownTimer -= Time.deltaTime;
            if (meltdownTimer <= 0)
            {
                Detonate();
            }
        }
    }

    void Detonate()
    {
        if (gameManager) gameManager.LosePoint(pointsOnDetonate);
        Destroy(gameObject);
    }

    public void OnSocketed()
    {
        if (isSocketed) return;
        isSocketed = true;
        isGrabbed = false;
        if (gameManager) gameManager.AddPoint(pointsOnSocket);

        // This makes the core "stick" and become non-grabbable
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        // Stop beeping, set material to "stable"
        this.enabled = false;
    }
}