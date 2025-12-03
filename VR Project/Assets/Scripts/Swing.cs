using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Swing : MonoBehaviour
{
    // --- Locomotion Providers ---
    public ContinuousMoveProviderBase moveProvider;
    //public SnapTurnProviderBase turnProvider;
    public ContinuousTurnProviderBase turnProvider;

    public Color lowTensionColor = Color.white;
    public Color highTensionColor = Color.red;

    public float cushionForce = 300f;
    public float cushionDistance = 2.0f; // Push away if closer than 2 meters

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

    public AudioSource webShootSound;

    public LineRenderer lineRenderer;

    private bool hasHit;

    // Hit object data
    private Rigidbody hitObjectBody;
    private Transform hitObjectTransform;
    private Vector3 hitLocalAnchor; // anchor in hit object's local space
    private bool followMovingAnchor = false;

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

    void FixedUpdate()
    {
        // 2. NEW: The "Cushion" Safety Push
        // This works even if you aren't swinging, keeping you off walls
        RaycastHit hit;
        // Cast a ray in the direction we are moving
        if (Physics.Raycast(playerRigidbody.position, playerRigidbody.velocity.normalized, out hit, cushionDistance, swingableLayer))
        {
            // If we are about to hit a wall, push BACK smoothly
            playerRigidbody.AddForce(hit.normal * cushionForce * Time.fixedDeltaTime);
        }

        // If we attached to a moving object that is kinematic or moved by Transform,
        // update the joint's connectedAnchor so the joint follows the object's motion.
        if (joint != null && followMovingAnchor && hitObjectTransform != null)
        {
            // If connectedBody is null, connectedAnchor is in world space and must be updated
            if (joint.connectedBody == null)
            {
                joint.connectedAnchor = hitObjectTransform.TransformPoint(hitLocalAnchor);
            }
            else
            {
                // connectedBody was set (non-kinematic Rigidbody); keep local anchor
                joint.connectedAnchor = hitLocalAnchor;
            }
        }
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

            if (webShootSound != null)
            {
                webShootSound.Play();
            }

            // Attach to the moving object if appropriate
            if (hitObjectTransform != null)
            {
                // If we have a Rigidbody and it is not kinematic, attach to it directly
                if (hitObjectBody != null && !hitObjectBody.isKinematic)
                {
                    joint.connectedBody = hitObjectBody;
                    joint.connectedAnchor = hitLocalAnchor; // local to the connected body
                    followMovingAnchor = false; // Rigidbody will be handled by physics
                }
                else
                {
                    // Either no Rigidbody or kinematic Rigidbody: keep the anchor in world space
                    // and update it each FixedUpdate to follow the transform.
                    joint.connectedBody = null;
                    joint.connectedAnchor = hitObjectTransform.TransformPoint(hitLocalAnchor);
                    followMovingAnchor = true;
                }
            }
            else
            {
                joint.connectedBody = null;
                joint.connectedAnchor = swingPoint;
                followMovingAnchor = false;
            }
        }
    }

    public void StopSwing()
    {
        if (webShootSound != null)
        {
            webShootSound.Stop();
        }

        Destroy(joint);

        // Clear moving anchor state
        followMovingAnchor = false;
        hitObjectBody = null;
        hitObjectTransform = null;

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

            // Save hit transform and Rigidbody (if any). Use attachedRigidbody to find Rigidbodies on parents.
            hitObjectTransform = raycastHit.collider != null ? raycastHit.collider.transform : null;
            hitObjectBody = raycastHit.collider != null ? raycastHit.collider.attachedRigidbody : null;
            if (hitObjectBody == null && hitObjectTransform != null)
                hitObjectBody = hitObjectTransform.GetComponentInParent<Rigidbody>();

            // Record local anchor on the hit transform so we can follow it if needed
            if (hitObjectTransform != null)
                hitLocalAnchor = hitObjectTransform.InverseTransformPoint(swingPoint);
            else
                hitLocalAnchor = Vector3.zero;

            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = swingPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
            hitObjectBody = null;
            hitObjectTransform = null;
        }
    }

    //public void DrawRope()
    //{
    //    if(!joint)
    //    {
    //        lineRenderer.enabled = false;
    //    }
    //    else
    //    {
    //        lineRenderer.enabled = true;
    //        lineRenderer.positionCount = 2;
    //        lineRenderer.SetPosition(0, startSwingHand.position);
    //        lineRenderer.SetPosition(1, swingPoint);
    //    }
    //}

    public void DrawRope()
    {
        if (!joint)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startSwingHand.position);
            lineRenderer.SetPosition(1, swingPoint);

            // --- NEW: COLOR CHANGE ---
            if (pullAction.action.IsPressed())
            {
                // We are pulling, so show High Tension (Red)
                lineRenderer.startColor = highTensionColor;
                lineRenderer.endColor = highTensionColor;
            }
            else
            {
                // Relaxed (White)
                lineRenderer.startColor = lowTensionColor;
                lineRenderer.endColor = lowTensionColor;
            }
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
