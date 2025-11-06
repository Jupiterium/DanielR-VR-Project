using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Swing : MonoBehaviour
{
    // --- Locomotion Providers ---
    public ContinuousMoveProviderBase moveProvider;
    //public SnapTurnProviderBase turnProvider;
    public ContinuousTurnProviderBase turnProvider;

    // --- Jump ---
    public InputActionProperty jumpAction;
    public float jumpForce = 5f;
    public float jumpCooldown = 5f;
    private float lastJumpTime = -10f;

    // --- Swing Settings ---
    public Transform startSwingHand;
    public float maxDistance = 35;
    public LayerMask swingableLayer;
    public Transform predictionPoint;
    private Vector3 swingPoint;

    // --- Input Actions ---
    public InputActionProperty swingAction;
    public InputActionProperty pullAction;

    // --- Physics ---
    public float pullingStrength = 500f;
    public Rigidbody playerRigidbody;

    // Using a ConfigurableJoint, which can act as a rope.
    private ConfigurableJoint joint;

    public LineRenderer lineRenderer;

    private bool hasHit;

    void Awake()
    {
        // Find swing & pull actions
        swingAction.action.Enable();
        pullAction.action.Enable();

        // Enable the jump action
        if (jumpAction.action != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPressed;
        }
    }

    void OnDestroy()
    {
        if (jumpAction.action != null)
        {
            jumpAction.action.performed -= OnJumpPressed;
        }
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        if (joint == null && Time.time > lastJumpTime + jumpCooldown)
        {
            Jump();
        }
    }

    void Update()
    {
        GetSwingPoint();

        if (swingAction.action.WasPressedThisFrame())
        {
            StartSwing();
        }
        else if (swingAction.action.WasReleasedThisFrame())
        {
            StopSwing();
        }

        PullRope();
        DrawRope();
    }

    public void StartSwing()
    {
        if (hasHit)
        {
            // Disable locomotion providers so they don't fight the joint
            //if (moveProvider) moveProvider.enabled = false;
            //if (turnProvider) turnProvider.enabled = false;

            joint = playerRigidbody.gameObject.AddComponent<ConfigurableJoint>();
            joint.anchor = playerRigidbody.transform.InverseTransformPoint(startSwingHand.position);
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint; // The point on the wall

            // Set the rope's length
            float distance = Vector3.Distance(startSwingHand.position, swingPoint);
            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = distance;
            joint.linearLimit = limit;

            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
        }
    }

    public void StopSwing()
    {
        Destroy(joint);

        // Re-enable locomotion providers
        //if (moveProvider) moveProvider.enabled = true;
        //if (turnProvider) turnProvider.enabled = true;
    }

    public void GetSwingPoint()
    {
        if (joint)
        {
            predictionPoint.gameObject.SetActive(false);
            return;
        }

        RaycastHit raycastHit;
        hasHit = Physics.Raycast(startSwingHand.position, startSwingHand.forward, out raycastHit, maxDistance, swingableLayer);
        
        if (hasHit)
        {
            swingPoint = raycastHit.point;
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = swingPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }
    }

    public void DrawRope()
    {
        if(!joint)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startSwingHand.position);
            lineRenderer.SetPosition(1, swingPoint);
        }

    }

    public void PullRope()
    {
        if (!joint) return;

        if (pullAction.action.IsPressed())
        {
            // Add force to pull us toward the anchor
            Vector3 direction = (swingPoint - startSwingHand.position).normalized;
            playerRigidbody.AddForce(direction * pullingStrength * Time.deltaTime);

            // Shorten the rope's limit to our new distance
            float distance = Vector3.Distance(startSwingHand.position, swingPoint);
            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = distance;
            joint.linearLimit = limit;
        }
    }

    private void Jump()
    {
        // Apply an instant upward force to the player's Rigidbody
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Record the time of this jump
        lastJumpTime = Time.time;
    }
}
