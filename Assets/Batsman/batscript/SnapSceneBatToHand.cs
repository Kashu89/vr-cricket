using UnityEngine;

/// <summary>
/// Parents the scene bat under the hand's BatHolder at runtime and fixes
/// its placement: auto-scales the bat to a real-world size and auto-aligns
/// its longest axis with the holder, so the bat is never oversized/crooked
/// regardless of the source FBX's native scale or orientation.
/// Optionally makes the bat grabbable in VR (BatVRGrab).
/// </summary>
public class SnapSceneBatToHand : MonoBehaviour
{
    [Tooltip("Drag the bat GameObject (scene instance) here")]
    public GameObject batInstance;

    [Tooltip("Drag the BatHolder (empty under the hand) here")]
    public Transform batHolder;

    [Tooltip("Fine-tune: local position relative to holder (applied always)")]
    public Vector3 localPosOffset = Vector3.zero;

    [Tooltip("Fine-tune: local Euler rotation relative to holder (applied AFTER auto-align)")]
    public Vector3 localRotOffset = Vector3.zero;

    [Header("Auto-fit (fixes oversize / crooked bat)")]
    [Tooltip("Measure the bat mesh and scale it so its longest dimension equals realBatLength")]
    public bool autoFitBatSize = true;

    [Tooltip("Real cricket bat length in meters")]
    public float realBatLength = 0.85f;

    [Tooltip("Rotate the bat so its longest axis points along the holder's +Y (stance pose)")]
    public bool autoAlignBatAxis = true;

    [Tooltip("If the bat ends up upside-down (handle pointing down), enable this")]
    public bool flipBat180 = false;

    [Tooltip("Manual size multiplier applied after auto-fit (1,1,1 = no change)")]
    public Vector3 localScaleOffset = Vector3.one;

    [Header("VR")]
    [Tooltip("Make the bat grabbable with VR controllers (needs XR Origin + an interactor in the scene)")]
    public bool enableVRGrab = true;

    void Start()
    {
        // Self-healing: the scene's serialized references can go stale
        // (broken fileIDs). Find by name before giving up.
        ResolveReferences();

        if (batInstance == null || batHolder == null)
        {
            Debug.LogError("SnapSceneBatToHand: batInstance or batHolder could not be found (Inspector refs broken and auto-find failed).");
            return;
        }

        // Parent to holder WITHOUT inheriting parent's scale/rotation issues
        batInstance.transform.SetParent(batHolder, false);

        // Base local transform
        batInstance.transform.localPosition = localPosOffset;
        batInstance.transform.localRotation = Quaternion.Euler(localRotOffset);
        batInstance.transform.localScale = Vector3.one;

        // Auto-fit: measure the mesh, fix size + axis (no-op if mesh already correct)
        ApplyAutoFit();

        // Disable physics so bat doesn't fall or get pushed
        // (BatVRGrab re-enables collision *queries* afterwards so XR hover can find the bat)
        var rb = batInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.detectCollisions = false;
        }

        // VR: let the player grab the bat with controllers
        if (enableVRGrab && batInstance.GetComponent<BatVRGrab>() == null)
            batInstance.AddComponent<BatVRGrab>();
    }

    // The scene's serialized batInstance/batHolder references can go stale
    // (broken fileIDs after reimports). Recover by name instead of failing.
    void ResolveReferences()
    {
        if (batHolder == null)
        {
            var bh = GameObject.Find("BatHolder");
            if (bh != null)
            {
                batHolder = bh.transform;
                Debug.Log("SnapSceneBatToHand: batHolder recovered by name.");
            }
        }

        if (batInstance == null)
        {
            batInstance = FindBatMeshObject();
            if (batInstance != null)
                Debug.Log($"SnapSceneBatToHand: batInstance recovered by name ({batInstance.name}).");
        }
    }

    // Find the FBX bat instance: a GameObject with "bat" in the name that
    // actually carries a mesh. Walks up to the bat hierarchy root.
    static GameObject FindBatMeshObject()
    {
        foreach (var mf in FindObjectsByType<MeshFilter>(FindObjectsInactive.Exclude))
        {
            var go = mf.gameObject;
            // Skip the holder itself and anything without a real mesh.
            if (mf.sharedMesh == null) continue;
            if (!go.name.ToLower().Contains("bat")) continue;
            if (go.name == "BatHolder" || go.name == "Batmanager") continue;

            while (go.transform.parent != null &&
                   go.transform.parent.name.ToLower().Contains("bat") &&
                   go.transform.parent.name != "BatHolder" &&
                   go.transform.parent.name != "Batmanager")
                go = go.transform.parent.gameObject;
            return go;
        }
        return null;
    }

    void ApplyAutoFit()
    {
        var mf = batInstance.GetComponentInChildren<MeshFilter>();
        if (mf == null || mf.sharedMesh == null)
        {
            Debug.LogWarning("SnapSceneBatToHand: no mesh found under batInstance, skipping auto-fit.");
            return;
        }

        // Longest dimension of the mesh, in the MESH's own local units (unit-agnostic:
        // works whether the FBX was authored in meters, centimeters or arbitrary units).
        Vector3 bs = mf.sharedMesh.bounds.size;
        Vector3 axisMeshLocal = Vector3.up;
        float longest = bs.y;
        if (bs.x > longest) { longest = bs.x; axisMeshLocal = Vector3.right; }
        if (bs.z > longest) { longest = bs.z; axisMeshLocal = Vector3.forward; }
        if (longest <= 0.0001f) return;

        // Mesh-local axis -> bat-root local space (robust to rotated intermediate FBX nodes).
        Vector3 axisWorld = mf.transform.TransformDirection(axisMeshLocal);
        Vector3 axisBatLocal = batInstance.transform.InverseTransformDirection(axisWorld).normalized;

        // 1) Rotation: longest axis -> holder +Y, optional 180 flip, then user fine-tune offset.
        Quaternion q = Quaternion.identity;
        if (autoAlignBatAxis)
            q = Quaternion.FromToRotation(axisBatLocal, Vector3.up);
        if (flipBat180)
            q = Quaternion.Euler(180f, 0f, 0f) * q;
        q = Quaternion.Euler(localRotOffset) * q;
        batInstance.transform.localRotation = q;

        // 2) Scale: longest dimension -> realBatLength in WORLD units, then manual multiplier.
        // (Collider sizes inherit transform scale automatically, so hit detection stays proportional.)
        float holderScaleY = Mathf.Abs(batHolder.lossyScale.y);
        if (holderScaleY < 0.0001f) holderScaleY = 1f;
        float s = autoFitBatSize ? (realBatLength / (longest * holderScaleY)) : 1f;
        batInstance.transform.localScale = new Vector3(
            s * localScaleOffset.x,
            s * localScaleOffset.y,
            s * localScaleOffset.z);

        Debug.Log($"SnapSceneBatToHand: bat auto-fit done (mesh longest={longest:F3} units, scale x{s:F3}, axis aligned={autoAlignBatAxis}).");
    }
}
