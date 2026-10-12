using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public enum DeliveryType { 
        NormalFast, InSwing, OutSwing, Cutter, Bouncer, Yorker, Slower,
        NormalSpin, Googly, Doosra, TopSpin 
    }
    public enum Arm { Right, Left }

    [Header("Current Ball Setup")]
    public DeliveryType currentDelivery;
    public Arm bowlerArm;
    public Transform rightHandBatsmanStumps; 

    [Header("Physics Speeds")]
    public float fastSpeed = 30f;
    public float spinSpeed = 18f;
    public float slowerSpeed = 22f;

    private Rigidbody rb;
    private bool isBallInAir = false;
    private bool hasBounced = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Auto-Find Stumps
        if (rightHandBatsmanStumps == null)
        {
            GameObject target = GameObject.Find("StumpTarget");
            if (target != null) rightHandBatsmanStumps = target.transform;
        }
    }

    public void BowlRealistically(Vector3 releasePosition)
    {
        rb.isKinematic = false;
        isBallInAir = true;
        hasBounced = false;

        // 1. Target Calculate karna (Ab ye release position bhi check karega)
        Vector3 finalTarget = CalculateTarget(releasePosition);

        // 2. Fekne ki disha nikalna
        Vector3 directionToTarget = (finalTarget - releasePosition).normalized;
        float speedToApply = SetBallSpeed();

        rb.AddForce(directionToTarget * speedToApply, ForceMode.Impulse);
    }

    private Vector3 CalculateTarget(Vector3 releasePosition)
{
    if (rightHandBatsmanStumps == null) return new Vector3(0, 0, 0);

    Vector3 targetPos = rightHandBatsmanStumps.position;
    
    // Stumps aur ball ke beech ka theek raasta nikalna (Bina rotation ke)
    Vector3 straightLine = (targetPos - releasePosition).normalized;
    Vector3 rightSide = Vector3.Cross(Vector3.up, straightLine).normalized;

    // Length (Aage-Peeche)
    if (currentDelivery == DeliveryType.Bouncer) 
        targetPos -= straightLine * 7f; 
    else if (currentDelivery == DeliveryType.Yorker) 
        targetPos -= straightLine * 1f; 
    else 
        targetPos -= straightLine * Random.Range(3f, 5f); 

    // Accuracy (Daayein-Baayein)
    int wideChance = Random.Range(0, 100);
    if (wideChance < 10) 
    {
        float wideError = Random.Range(1.5f, 2.5f);
        targetPos += rightSide * ((Random.Range(0, 2) == 0) ? wideError : -wideError);
    }
    else 
    {
        // Accuracy aur tight kar di hai (0.2 meters ke andar)
        targetPos += rightSide * Random.Range(-0.2f, 0.2f); 
    }

    // DEBUG LASER: Scene view mein dekho ki nishana kahan hai
    Debug.DrawLine(releasePosition, targetPos, Color.red, 5f);

    return targetPos;
}
    private float SetBallSpeed()
    {
        if (currentDelivery == DeliveryType.Slower) return slowerSpeed;
        if (currentDelivery == DeliveryType.NormalSpin || currentDelivery == DeliveryType.Googly || currentDelivery == DeliveryType.Doosra || currentDelivery == DeliveryType.TopSpin) return spinSpeed;
        return fastSpeed;
    }

    void FixedUpdate()
    {
        if (isBallInAir && !hasBounced)
        {
            float swingDir = 0f;
            if (currentDelivery == DeliveryType.InSwing) swingDir = (bowlerArm == Arm.Right) ? -1f : 1f;
            if (currentDelivery == DeliveryType.OutSwing) swingDir = (bowlerArm == Arm.Right) ? 1f : -1f;

            if (swingDir != 0) rb.AddForce(transform.right * (6f * swingDir), ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pitch") && !hasBounced)
        {
            isBallInAir = false;
            hasBounced = true;

            float turnDir = 0f;
            if (currentDelivery == DeliveryType.NormalSpin) turnDir = (bowlerArm == Arm.Right) ? -1f : 1f; 
            if (currentDelivery == DeliveryType.Googly) turnDir = (bowlerArm == Arm.Right) ? -1f : 1f; 
            if (currentDelivery == DeliveryType.Doosra) turnDir = (bowlerArm == Arm.Right) ? 1f : -1f; 
            if (currentDelivery == DeliveryType.Cutter) turnDir = (Random.Range(0, 2) == 0) ? -0.5f : 0.5f;

            if (turnDir != 0) rb.AddForce(transform.right * (8f * turnDir), ForceMode.Impulse);
        }
    }
}