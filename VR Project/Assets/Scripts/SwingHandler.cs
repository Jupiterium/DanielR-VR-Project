using UnityEngine;
using UnityEngine.InputSystem; // You MUST add this
using UnityEngine.XR.Interaction.Toolkit;

public class SwingHandler : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("The Input Action for the swing button (e.g., Select/Trigger)")]
    public InputActionProperty swingAction; // DRAG YOUR BUTTON ACTION HERE

    [Header("Swing Settings")]
    [Tooltip("The layer that your swing anchors are on.")]
    public LayerMask swingableLayer; // SET THIS TO YOUR ANCHOR'S LAYER

    [Tooltip("Maximum distance you can shoot a web from.")]
    public float maxSwingDistance = 30f;

    // Private variables
    private ConfigurableJoint joint;
    private Rigidbody handRigidbody;

    void Awake()
    {
        // Get the Rigidbody on this hand
        handRigidbody = GetComponent<Rigidbody>();

        // Subscribe to the button press and release events
        swingAction.action.Enable();
        swingAction.action.performed += OnSwingStart; // Button pressed
        swingAction.action.canceled += OnSwingEnd;   // Button released
    }

    private void OnSwingStart(InputAction.CallbackContext context)
    {
        // 1. Fire our own raycast
        RaycastHit hit;

        // We use QueryTriggerInteraction.Collide to FORCE it to hit triggers
        bool didHit = Physics.Raycast(transform.position, transform.forward,
                                      out hit, maxSwingDistance,
                                      swingableLayer, QueryTriggerInteraction.Collide);

        // 2. Did we hit a swingable anchor?
        if (didHit && hit.collider.CompareTag("SwingAnchor"))
        {
            // --- We are now 100% sure we hit a valid anchor ---

            // 3. Add the joint TO THIS HAND
            joint = gameObject.AddComponent<ConfigurableJoint>();

            // 4. Configure the joint
            joint.autoConfigureConnectedAnchor = false;

            // Connect to the exact point we hit, NOT the anchor's center
            joint.connectedAnchor = hit.point;

            // 5. Set the "rope length"
            float ropeLength = Vector3.Distance(transform.position, hit.point);
            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = ropeLength;
            joint.linearLimit = limit;

            // 6. Set joint to be strong
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
        }
    }

    private void OnSwingEnd(InputAction.CallbackContext context)
    {
        // 1. If we are swinging (joint exists) and let go...
        if (joint != null)
        {
            // 2. Destroy the joint
            Destroy(joint);
            joint = null;
        }
    }

    void OnDestroy()
    {
        // Clean up
        swingAction.action.performed -= OnSwingStart;
        swingAction.action.canceled -= OnSwingEnd;
    }
}