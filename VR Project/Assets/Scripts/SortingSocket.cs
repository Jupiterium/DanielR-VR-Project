////using UnityEngine;
////using UnityEngine.XR.Interaction.Toolkit;

////[RequireComponent(typeof(XRSocketInteractor))]
////public class SortingSocket : MonoBehaviour
////{
////    // --- SET THIS IN THE INSPECTOR ---
////    [Tooltip("What kind of core does this socket accept?")]
////    public CoreType expectedCoreType;
////    public enum CoreType { Red, Blue, Green }

////    // --- ASSIGN THESE ---
////    public int pointsForThisCore = 1;
////    private GameManager gameManager;

////    private XRSocketInteractor socket;

////    void Awake()
////    {
////        gameManager = FindObjectOfType<GameManager>();
////        socket = GetComponent<XRSocketInteractor>();

////        // Subscribe to the socket's "selectEntered" event
////        socket.selectEntered.AddListener(OnCoreSocketed);
////    }

////    //private void OnCoreSocketed(SelectEnterEventArgs args)
////    //{
////    //    // 1. Get the object that was just socketed
////    //    GameObject coreObject = args.interactableObject.transform.gameObject;

////    //    // 2. Check if it's the right type
////    //    // (We are checking the Physics Layer, which is the simple way)
////    //    bool isCorrectType = false;
////    //    switch (expectedCoreType)
////    //    {
////    //        case CoreType.Red:
////    //            isCorrectType = (coreObject.layer == LayerMask.NameToLayer("RedCoreLayer"));
////    //            break;
////    //        case CoreType.Blue:
////    //            isCorrectType = (coreObject.layer == LayerMask.NameToLayer("BlueCoreLayer"));
////    //            break;
////    //        case CoreType.Green:
////    //            isCorrectType = (coreObject.layer == LayerMask.NameToLayer("GreenCoreLayer"));
////    //            break;
////    //    }

////    //    // 3. If it's not the right core, reject it!
////    //    if (!isCorrectType)
////    //    {
////    //        // You can add a "REJECT" sound here
////    //        return;
////    //    }

////    //    // 4. It IS the right core. Now, is this a tutorial core or a real core?
////    //    // We'll check the core's own script to see if it's enabled.
////    //    // Tutorial cores have their scripts disabled.



////    //    bool isTutorialCore = false;
////    //    if (coreObject.TryGetComponent<UnstableCore>(out UnstableCore unstable))
////    //    {
////    //        isTutorialCore = !unstable.enabled;
////    //    }
////    //    else if (coreObject.TryGetComponent<FragileCore>(out FragileCore fragile))
////    //    {
////    //        isTutorialCore = !fragile.enabled;
////    //    }

////    //    // 5. Call the right GameManager function
////    //    if (isTutorialCore)
////    //    {
////    //        gameManager.OnTutorialCoreSocketed();
////    //    }
////    //    else
////    //    {
////    //        gameManager.AddPoint(pointsForThisCore);

////    //        // Call the core's "OnSocketed" function (if it exists) to make it stop
////    //        if (unstable) unstable.OnSocketed();
////    //        //if (fragile) fragile.OnSocketed();
////    //        if (coreObject.TryGetComponent<BatteryCore>(out BatteryCore battery))
////    //        {
////    //            battery.OnSocketed();
////    //        }
////    //    }

////    //    // 6. Disable the socket so it can't be used again
////    //    socket.socketActive = false;
////    //    // Optionally, destroy the core
////    //    // Destroy(coreObject, 1f); // Wait 1 sec
////    //}

////    private void OnCoreSocketed(SelectEnterEventArgs args)
////    {
////        // 1. Get the object that was just socketed
////        GameObject coreObject = args.interactableObject.transform.gameObject;

////        // 2. Check if it's the right type
////        bool isCorrectType = false;
////        switch (expectedCoreType)
////        {
////            case CoreType.Red:
////                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("RedCoreLayer"));
////                break;
////            case CoreType.Blue:
////                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("BlueCoreLayer"));
////                break;
////            case CoreType.Green:
////                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("GreenCoreLayer"));
////                break;
////        }

////        if (!isCorrectType)
////        {
////            return; // Reject wrong core
////        }

////        // 4. Get all possible core scripts
////        UnstableCore unstable = coreObject.GetComponent<UnstableCore>();
////        FragileCore fragile = coreObject.GetComponent<FragileCore>();
////        NormalCore normal = coreObject.GetComponent<NormalCore>();

////        // 5. Is this a tutorial core? (Check if its script is disabled)
////        bool isTutorialCore = false;
////        if (unstable != null)
////        {
////            isTutorialCore = !unstable.enabled;
////        }
////        else if (fragile != null)
////        {
////            isTutorialCore = !fragile.enabled;
////        }

////        else if (normal != null)
////        {
////            // We check the NormalCore's script
////            isTutorialCore = !normal.enabled;
////        }

////        // 6. Call the right GameManager function
////        if (isTutorialCore)
////        {
////            gameManager.OnTutorialCoreSocketed();
////        }
////        else
////        {
////            gameManager.AddPoint(pointsForThisCore);

////            // Call the core's "OnSocketed" function (if it exists) to make it stop
////            if (unstable != null) unstable.OnSocketed();
////            if (fragile != null) fragile.OnSocketed();
////            if (normal != null) normal.OnSocketed();
////        }

////        // 7. Disable the socket so it can't be used again
////        socket.socketActive = false;

////        // 8. Destroy the core to clean it up
////        Destroy(coreObject, 1.0f); // Destroys the core after 1 second
////    }
////}





//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//[RequireComponent(typeof(XRSocketInteractor))]
//public class SortingSocket : MonoBehaviour
//{
//    [Tooltip("What kind of core does this socket accept?")]
//    public CoreType expectedCoreType;
//    public enum CoreType { Red, Blue, Green }

//    public int pointsForThisCore = 1;
//    private GameManager gameManager;
//    private XRSocketInteractor socket;

//    void Awake()
//    {
//        gameManager = FindObjectOfType<GameManager>();
//        socket = GetComponent<XRSocketInteractor>();
//        socket.selectEntered.AddListener(OnCoreSocketed);
//    }

//    private void OnCoreSocketed(SelectEnterEventArgs args)
//    {
//        GameObject coreObject = args.interactableObject.transform.gameObject;

//        // 1. Check if it's the right type (This logic is the same)
//        bool isCorrectType = false;
//        switch (expectedCoreType)
//        {
//            case CoreType.Red:
//                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("RedCore"));
//                break;
//            case CoreType.Blue:
//                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("BlueCore"));
//                break;
//            case CoreType.Green:
//                isCorrectType = (coreObject.layer == LayerMask.NameToLayer("GreenCore"));
//                break;
//        }

//        if (!isCorrectType)
//        {
//            return; // Reject wrong core
//        }

//        // -----------------------------------------------------------------
//        //  ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼ FIX ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
//        // -----------------------------------------------------------------
//        //  We now check the object's TAG to see if it's a tutorial core.
//        //  This is much more reliable than checking if a script is enabled.
//        // -----------------------------------------------------------------
//        bool isTutorialCore = coreObject.CompareTag("Tutorial Core");

//        // 5. Call the right GameManager function
//        if (isTutorialCore)
//        {
//            gameManager.OnTutorialCoreSocketed();
//        }
//        else
//        {
//            gameManager.AddPoint(pointsForThisCore);

//            // Call the core's "OnSocketed" function (if it exists) to make it stop
//            if (coreObject.TryGetComponent<UnstableCore>(out UnstableCore unstable))
//            {
//                unstable.OnSocketed();
//            }
//            else if (coreObject.TryGetComponent<FragileCore>(out FragileCore fragile))
//            {
//                fragile.OnSocketed();
//            }
//            else if (coreObject.TryGetComponent<NormalCore>(out NormalCore normal))
//            {
//                normal.OnSocketed();
//            }
//        }

//        // 6. Disable the socket so it can't be used again
//        socket.socketActive = false;

//        // 7. (OPTIONAL) Destroy the core to clean it up
//        Destroy(coreObject, 1.0f); // Destroys the core after 1 second
//    }
//}


using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SortingSocket : MonoBehaviour
{
    // The kinds of Core's the current socket accepts
    public CoreType expectedCoreType;
    public enum CoreType { Red, Blue, Green }

    // Interaction layer masks
    public InteractionLayerMask redCoreMask;
    public InteractionLayerMask blueCoreMask;
    public InteractionLayerMask greenCoreMask;

    private GameManager gameManager;
    private XRSocketInteractor socket;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnCoreSocketed);
    }

    private void OnCoreSocketed(SelectEnterEventArgs args)
    {
        // We get the interactable (the core)
        IXRSelectInteractable coreInteractable = args.interactableObject;
        GameObject coreObject = coreInteractable.transform.gameObject;

        // Check the Core's Interaction Layer Mask
        bool isCorrectType = false;
        InteractionLayerMask coreMask = coreInteractable.interactionLayers;

        switch (expectedCoreType)
        {
            case CoreType.Red:
                // Check if the core's mask overlaps with the red mask we defined
                isCorrectType = (coreMask & redCoreMask) != 0;
                break;
            case CoreType.Blue:
                isCorrectType = (coreMask & blueCoreMask) != 0;
                break;
            case CoreType.Green:
                isCorrectType = (coreMask & greenCoreMask) != 0;
                break;
        }

        if (!isCorrectType)
        {
            Debug.LogError("Socket Rejected: Core does not have the correct Interaction Layer.", this);
            return; // Reject wrong core
        }

        // Call the core's "OnSocketed" function (if it exists)
        if (coreObject.TryGetComponent<UnstableCore>(out UnstableCore unstable))
        {
            unstable.OnSocketed();
        }
        else if (coreObject.TryGetComponent<FragileCore>(out FragileCore fragile))
        {
            fragile.OnSocketed();
        }
        else if (coreObject.TryGetComponent<NormalCore>(out NormalCore normal))
        {
            normal.OnSocketed();
        }

        // Disable the socket and destroy the core
        socket.socketActive = false;
        // Destroy(coreObject, 1.5f); // Destroys the core after 1.5 seconds
    }
}