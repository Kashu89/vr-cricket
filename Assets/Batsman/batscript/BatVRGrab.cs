using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Makes the cricket bat grabbable with VR controllers.
/// Added at runtime by SnapSceneBatToHand when enableVRGrab is true.
/// Requirements in the scene: an XRInteractionManager (present) and an
/// interactor on each hand controller (add XRDirectInteractor to your
/// controller GameObjects, or use XRI's default XR Origin prefab).
/// When no VR hardware/interactor is present this component is inert.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BatVRGrab : MonoBehaviour
{
    void Start()
    {
        // Kinematic: never falls, never gets pushed. Queries stay ON so
        // XR hover/ray can detect the bat (SnapSceneBatToHand turns them off).
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.detectCollisions = true;

        var grab = GetComponent<XRGrabInteractable>();
        if (grab == null)
            grab = gameObject.AddComponent<XRGrabInteractable>();

        grab.trackPosition = true;
        grab.trackRotation = true;
        grab.movementType = XRBaseInteractable.MovementType.Kinematic;
        grab.throwOnDetach = true;
        grab.attachEaseInTime = 0.15f;

        Debug.Log("BatVRGrab: bat is now grabbable in VR.");
    }
}
