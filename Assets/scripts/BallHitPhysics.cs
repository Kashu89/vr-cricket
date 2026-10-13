using UnityEngine;

/// <summary>
/// Lives on every bowled ball (added at runtime). Handles the two real-cricket
/// moments the user asked for:
///   1. Ball hits the stumps -> bails fly (BOWLED!), ball deflects off physically.
///   2. Bat hits the ball   -> real impulse from the actual swing speed, so the
///      ball can clear the boundary (SIX!) or roll to it (FOUR!).
/// A struck ball is re-spawned as a pure-physics ball so nothing scripted
/// interferes with its flight.
/// </summary>
public class BallHitPhysics : MonoBehaviour
{
    public enum BallState { Flying, Hit, Dead }

    public BallState State { get; private set; } = BallState.Flying;

    // Real-world tuning.
    const float BallMass = 0.156f;          // kg, real cricket ball
    const float HitTransfer = 1.35f;        // bat speed -> ball speed multiplier
    const float MaxExitSpeed = 42f;         // m/s (~151 km/h, elite six)
    const float MinExitSpeed = 8f;

    Rigidbody rb;
    int bounces;
    bool boundaryResolved;
    float lifeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (State != BallState.Flying || rb == null) return;

        // 1) Wicket zone -> bowled.
        var zone = other.GetComponent<WicketZone>();
        if (zone == null) zone = other.GetComponentInParent<WicketZone>();
        if (zone != null)
        {
            OnBowled();
            return;
        }

        // 2) Bat -> hit (only if the bat is genuinely swinging).
        var tracker = other.GetComponent<BatSwingTracker>();
        if (tracker == null) tracker = other.GetComponentInParent<BatSwingTracker>();
        if (tracker != null)
        {
            if (tracker.IsRealSwing()) OnBatHit(tracker);
            else if (tracker.IsBlock()) OnBlock();
            // else: bat is still, ball passes (leave / miss)
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Direct solid hit on a stump also counts as bowled.
        if (State == BallState.Flying)
        {
            var stumps = collision.gameObject.GetComponentInParent<StumpsPhysics>();
            if (stumps != null)
            {
                OnBowled();
                return;
            }
        }

        // Count ground bounces for FOUR vs SIX.
        if (State == BallState.Hit && !boundaryResolved)
        {
            foreach (var cp in collision.contacts)
            {
                if (cp.normal.y > 0.5f) { bounces++; break; }
            }
        }
    }

    // ---------------- BOWLED ----------------

    void OnBowled()
    {
        State = BallState.Dead;
        Vector3 incoming = rb.velocity;

        var stumps = FindFirstObjectByType<StumpsPhysics>();
        if (stumps != null) stumps.KnockDown(incoming);

        // Deflect back off the stumps with a pop-up, like a real bowled ball.
        Vector3 back = -incoming;
        back.y = 0f;
        if (back.sqrMagnitude < 0.01f) back = Vector3.back;
        back.Normalize();
        rb.velocity = back * incoming.magnitude * 0.35f + Vector3.up * 3f;
        rb.angularVelocity = Random.insideUnitSphere * 10f;

        var ui = CricketMatchManager.Instance;
        if (ui != null) ui.ShowMessage("BOWLED HIM!", Color.red);

        lifeTimer = 4f;
        Debug.Log($"BallHitPhysics: bowled! incoming {incoming.magnitude:F1} m/s");
    }

    // ---------------- BAT HIT ----------------

    void OnBatHit(BatSwingTracker tracker)
    {
        float batSpeed = tracker.GetSwingSpeed(); // m/s, real swing
        Vector3 swingDir = tracker.GetSwingDirection();

        float exitSpeed = Mathf.Clamp(batSpeed * HitTransfer, MinExitSpeed, MaxExitSpeed);
        Vector3 dir = swingDir * 0.9f + Vector3.up * 0.45f;
        if (dir.sqrMagnitude < 0.01f) dir = Vector3.up;
        dir.Normalize();

        Debug.Log($"BallHitPhysics: HIT! bat {batSpeed:F1} m/s -> ball {exitSpeed:F1} m/s ({exitSpeed * 3.6f:F0} km/h)");
        SpawnHitBall(transform.position, dir * exitSpeed);
        Destroy(gameObject); // scripted ball replaced by a pure-physics ball
    }

    void OnBlock()
    {
        // Defensive block: ball drops dead near the bat.
        rb.velocity = rb.velocity * 0.12f + Vector3.up * 1.2f;
        rb.angularVelocity *= 0.2f;
        Debug.Log("BallHitPhysics: blocked.");
    }

    // A fresh ball with no scripted flight — pure rigid-body physics from here.
    void SpawnHitBall(Vector3 pos, Vector3 velocity)
    {
        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();
        var sc = GetComponent<SphereCollider>();

        var go = new GameObject("HitBall");
        go.transform.position = pos;

        if (mf != null)
        {
            var n = go.AddComponent<MeshFilter>();
            n.sharedMesh = mf.sharedMesh;
        }
        if (mr != null)
        {
            var n = go.AddComponent<MeshRenderer>();
            n.sharedMaterials = mr.sharedMaterials;
        }
        var col = go.AddComponent<SphereCollider>();
        col.radius = sc != null ? sc.radius : 0.036f;
        if (sc != null) col.material = sc.material;

        var nrb = go.AddComponent<Rigidbody>();
        nrb.mass = BallMass;
        nrb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        var hp = go.AddComponent<BallHitPhysics>();
        hp.BeginHit(velocity);

        var ui = CricketMatchManager.Instance;
        if (ui != null) ui.ShowSpeed(velocity.magnitude);
    }

    public void BeginHit(Vector3 velocity)
    {
        rb = GetComponent<Rigidbody>();
        State = BallState.Hit;
        rb.velocity = velocity;
        // A touch of backspin, like a real lofted shot.
        rb.angularVelocity = Vector3.Cross(velocity.normalized, Vector3.up) * velocity.magnitude * 0.4f;
        lifeTimer = 14f;
    }

    // ---------------- Boundary + cleanup ----------------

    void Update()
    {
        if (State == BallState.Hit && !boundaryResolved)
        {
            var m = CricketMatchManager.Instance;
            if (m != null)
            {
                Vector3 d = transform.position - m.PitchCentre;
                d.y = 0f;
                if (d.magnitude > m.boundaryRadius)
                {
                    boundaryResolved = true;
                    bool six = bounces == 0;
                    m.ShowMessage(six ? "SIX!" : "FOUR!", Color.yellow, 3f);
                    Debug.Log($"BallHitPhysics: {(six ? "SIX" : "FOUR")} (bounces={bounces})");
                }
            }
        }

        if (State != BallState.Flying)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f) Destroy(gameObject);
        }
    }
}
