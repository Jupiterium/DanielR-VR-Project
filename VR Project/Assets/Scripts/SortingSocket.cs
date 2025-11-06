using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SortingSocket : MonoBehaviour
{
    // --- SET THIS IN THE INSPECTOR ---
    [Tooltip("What kind of core does this socket accept?")]
    public CoreType expectedCoreType;
    public enum CoreType { Red, Blue, Green }

    // --- ASSIGN THESE ---
    public int pointsForThisCore = 1;
    private GameManager gameManager;

    private XRSocketInteractor socket;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        socket = GetComponent<XRSocketInteractor>();

        // Subscribe to the socket's "selectEntered" event
        socket.selectEntered.AddListener(OnCoreSocketed);
    }

    private void OnCoreSocketed(SelectEnterEventArgs args)
    {
        // 1. Get the object that was just socketed
        GameObject coreObject = args.interactableObject.transform.gameObject;

        // 2. Check if it's the right type
        // (We are checking the Physics Layer, which is the simple way)
        bool isCorrectType = false;
        switch (expectedCoreType)
        {
            case CoreType.Red:
                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("RedCoreLayer"));
                break;
            case CoreType.Blue:
                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("BlueCoreLayer"));
                break;
            case CoreType.Green:
                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("GreenCoreLayer"));
                break;
        }

        // 3. If it's not the right core, reject it!
        if (!isCorrectType)
        {
            // You can add a "REJECT" sound here
            return;
        }

        // 4. It IS the right core. Now, is this a tutorial core or a real core?
        // We'll check the core's own script to see if it's enabled.
        // Tutorial cores have their scripts disabled.



        bool isTutorialCore = false;
        if (coreObject.TryGetComponent<UnstableCore>(out UnstableCore unstable))
        {
            isTutorialCore = !unstable.enabled;
        }
        else if (coreObject.TryGetComponent<FragileCore>(out FragileCore fragile))
        {
            isTutorialCore = !fragile.enabled;
        }

        // 5. Call the right GameManager function
        if (isTutorialCore)
        {
            gameManager.OnTutorialCoreSocketed();
        }
        else
        {
            gameManager.AddPoint(pointsForThisCore);

            // Call the core's "OnSocketed" function (if it exists) to make it stop
            if (unstable) unstable.OnSocketed();
            //if (fragile) fragile.OnSocketed();
            if (coreObject.TryGetComponent<BatteryCore>(out BatteryCore battery))
            {
                battery.OnSocketed();
            }
        }

        // 6. Disable the socket so it can't be used again
        socket.socketActive = false;
        // Optionally, destroy the core
        // Destroy(coreObject, 1f); // Wait 1 sec
    }
}