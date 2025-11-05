using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// We removed the [RequireComponent] line that caused the error
public class WebShooter : MonoBehaviour
{
    // We changed this from XRDirectInteractor to XRRayInteractor
    private XRRayInteractor interactor;
    private ConfigurableJoint joint;

    void Awake()
    {
        // We get the Ray Interactor that is already on the hand
        interactor = GetComponent<XRRayInteractor>();
    }

    void OnEnable()
    {
        interactor.selectEntered.AddListener(OnSwingStart);
        interactor.selectExited.AddListener(OnSwingEnd);
    }

    void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnSwingStart);
        interactor.selectExited.RemoveListener(OnSwingEnd);
    }

    private void OnSwingStart(SelectEnterEventArgs args)
    {
        // Check if the thing we "selected" (shot with the ray) is a Swing Anchor
        if (args.interactableObject.transform.CompareTag("SwingAnchor"))
        {
            // --- The rest of the logic is identical ---

            // 1. Get the anchor point
            Transform anchor = args.interactableObject.transform;

            // 2. Add the joint TO THIS HAND
            joint = gameObject.AddComponent<ConfigurableJoint>();

            // 3. Configure the joint
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = anchor.position;

            // 4. Set the "rope length"
            float ropeLength = Vector3.Distance(transform.position, anchor.position);
            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = ropeLength;
            joint.linearLimit = limit;

            // 5. Set joint to be strong
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
        }
    }

    private void OnSwingEnd(SelectExitEventArgs args)
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
}