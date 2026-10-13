using UnityEngine;

/// <summary>
/// Measures the bat's real swing velocity in world metres/second.
/// Works for mouse swings and VR controller swings alike, because it
/// tracks the bat transform itself. Added to the bat at runtime.
/// </summary>
public class BatSwingTracker : MonoBehaviour
{
    // Below this bat speed a "touch" is ignored (ball just passes the still bat).
    const float HitThreshold = 3.5f;
    // Between these speeds the ball is softly blocked instead of smashed.
    const float BlockThreshold = 1.0f;

    Vector3 prevPos;
    Vector3 smoothVel;
    float maxRecentSpeed;
    float recentTimer;
    const float RecentWindow = 0.25f;

    void Start()
    {
        prevPos = transform.position;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (dt > 0.0001f)
        {
            Vector3 v = (transform.position - prevPos) / dt;
            // Heavier smoothing on tiny speeds so a still bat reads ~0.
            smoothVel = Vector3.Lerp(smoothVel, v, 0.6f);
            float s = smoothVel.magnitude;
            if (s > maxRecentSpeed) maxRecentSpeed = s;
        }
        prevPos = transform.position;

        recentTimer += Time.deltaTime;
        if (recentTimer >= RecentWindow)
        {
            recentTimer = 0f;
            maxRecentSpeed = smoothVel.magnitude;
        }
    }

    /// <summary>Peak swing speed over the last quarter second (m/s).</summary>
    public float GetSwingSpeed()
    {
        return Mathf.Max(smoothVel.magnitude, maxRecentSpeed);
    }

    /// <summary>Swing direction in world space (full 3D).</summary>
    public Vector3 GetSwingDirection()
    {
        if (smoothVel.sqrMagnitude > 0.01f)
            return smoothVel.normalized;
        return transform.forward;
    }

    public bool IsRealSwing() { return GetSwingSpeed() >= HitThreshold; }
    public bool IsBlock() { float s = GetSwingSpeed(); return s >= BlockThreshold && s < HitThreshold; }
}
