using UnityEngine;

public class SnapSceneBatToHand : MonoBehaviour
{
    [Tooltip("Drag the bat GameObject (scene instance) here")]
    public GameObject batInstance;

    [Tooltip("Drag the BatHolder (empty under the hand) here")]
    public Transform batHolder;

    [Tooltip("Fine-tune: local position relative to holder")]
    public Vector3 localPosOffset = Vector3.zero;

    [Tooltip("Fine-tune: local Euler rotation relative to holder")]
    public Vector3 localRotOffset = Vector3.zero;

    void Start()
    {
        if (batInstance == null || batHolder == null)
        {
            Debug.LogWarning("Assign batInstance and batHolder in the Inspector.");
            return;
        }

        // Make sure bat root has neutral scale before parenting
        batInstance.transform.localScale = Vector3.one;

        // Parent to holder WITHOUT inheriting parent's scale/rotation issues
        batInstance.transform.SetParent(batHolder, false);

        // Force desired local transform
        batInstance.transform.localPosition = localPosOffset;
        batInstance.transform.localRotation = Quaternion.Euler(localRotOffset);
        batInstance.transform.localScale = Vector3.one;

        // Disable physics so bat doesn't fall or get pushed
        var rb = batInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // Optional: disable colliders if they cause problems
        // foreach(var c in batInstance.GetComponentsInChildren<Collider>()) c.enabled = false;
    }
}
