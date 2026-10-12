// using UnityEngine;
// using System.Collections.Generic;

// public enum BowlerType
// {
//     LeftArmFast,
//     RightArmFast,
//     LeftArmMediumFast,
//     RightArmMediumFast,
//     LeftArmOrthodoxSpin,
//     LeftArmWristSpin,
//     RightArmOffSpin,
//     RightArmLegSpin
// }

// [System.Serializable]
// public class BallVariation
// {
//     public string name;
//     public Transform targetPoint;
//     public float timeToTarget = 0.6f;
//     public float swingMeters = 0f;
//     public Vector3 spinTorque = Vector3.zero;
// }

// public class BowlingManager : MonoBehaviour
// {
//     [Header("References")]
//     public Animator animator;
//     public Transform rightHandSpawn;
//     public Transform leftHandSpawn;
//     public GameObject ballPrefab;

//     [Header("Bowler Type Selection")]
//     public BowlerType currentBowler;

//     [Header("Ball Variations List")]
//     public List<BallVariation> variations = new List<BallVariation>();

//     Transform activeSpawn;
//     int lastIndex = -1;

//     public static bool isLeftArm = false;
//     public static bool isSpinBowler = false;

//     void Start()
//     {
//         ApplyBowlerType();
//     }

//     // CALLED WHEN USER SELECTS BOWLER TYPE
//     public void SetBowlerType(int typeIndex)
//     {
//         currentBowler = (BowlerType)typeIndex;
//         ApplyBowlerType();
//     }

//     void ApplyBowlerType()
//     {
//         // 1. Check if left arm bowler
//         isLeftArm =
//             currentBowler == BowlerType.LeftArmFast ||
//             currentBowler == BowlerType.LeftArmMediumFast ||
//             currentBowler == BowlerType.LeftArmOrthodoxSpin ||
//             currentBowler == BowlerType.LeftArmWristSpin;

//         // 2. Check if spin bowler
//         isSpinBowler =
//             currentBowler == BowlerType.LeftArmOrthodoxSpin ||
//             currentBowler == BowlerType.LeftArmWristSpin ||
//             currentBowler == BowlerType.RightArmOffSpin ||
//             currentBowler == BowlerType.RightArmLegSpin;

//         // 3. Hand spawn set
//         activeSpawn = isLeftArm ? leftHandSpawn : rightHandSpawn;

//         // 4. Play Animation Trigger (make sure triggers exist in Animator)
//         if (animator != null)
//             animator.SetTrigger(currentBowler.ToString());
//     }

//     BallVariation PickVariation()
//     {
//         if (variations.Count == 0) return null;

//         int idx = Random.Range(0, variations.Count);

//         if (idx == lastIndex && variations.Count > 1)
//             idx = (idx + 1) % variations.Count;

//         lastIndex = idx;

//         return variations[idx];
//     }

//     // CALLED FROM ANIMATION EVENT FRAME
//     public void OnBallRelease()
//     {
//         BallVariation v = PickVariation();
//         if (v == null) return;

//         float finalSwing = v.swingMeters;
//         if (isLeftArm) finalSwing = -finalSwing;

//         GameObject ball = Instantiate(ballPrefab, activeSpawn.position, activeSpawn.rotation);

//         ball.GetComponent<BallThrower>().Launch(
//             activeSpawn.position,
//             v.targetPoint.position,
//             v.timeToTarget,
//             finalSwing,
//             v.spinTorque
//         );
//     }
// }

// ---------------------------------------------------------------------------------------------------------------------------------------------
  
//   check 2.0 

// ---------------------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public enum BowlerType
{
    LeftArmFast,
    RightArmFast,
    LeftArmMediumFast,
    RightArmMediumFast,
    LeftArmOrthodoxSpin,
    LeftArmWristSpin,
    RightArmOffSpin,
    RightArmLegSpin
}

[System.Serializable]
public class BallVariation
{
    public string name;
    public Transform targetPoint;
    public float timeToTarget = 0.6f;
    public float swingMeters = 0f;
    public Vector3 spinTorque = Vector3.zero;
}

public class BowlingManager : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform rightHandSpawn;
    public Transform leftHandSpawn;
    public GameObject ballPrefab;

    [Header("UI")]
    public BowlingStyleUI bowlingStyleUI;

    [Header("Over Settings")]
    public int ballsPerOver = 6;

    [Header("Bowler Rotation Order")]
    public BowlerType[] bowlerOrder =
    {
        BowlerType.RightArmFast,
        BowlerType.LeftArmFast,
        BowlerType.RightArmOffSpin,
        BowlerType.LeftArmOrthodoxSpin
    };

    [Header("Ball Variations")]
    public List<BallVariation> variations = new List<BallVariation>();

    public BowlerType currentBowler;

    Transform activeSpawn;
    int lastIndex = -1;

    public static bool isLeftArm;
    public static bool isSpinBowler;

    int ballCount = 0;
    int bowlerIndex = 0;

    void Start()
    {
        if (bowlerOrder == null || bowlerOrder.Length == 0)
        {
            Debug.LogError("BowlingManager: bowlerOrder is empty.");
            return;
        }

        currentBowler = bowlerOrder[bowlerIndex];
        ApplyBowlerType();
        UpdateBowlingStyleUI();
    }

    void ApplyBowlerType()
    {
        // LEFT / RIGHT ARM
        isLeftArm =
            currentBowler == BowlerType.LeftArmFast ||
            currentBowler == BowlerType.LeftArmMediumFast ||
            currentBowler == BowlerType.LeftArmOrthodoxSpin ||
            currentBowler == BowlerType.LeftArmWristSpin;

        // SPIN / FAST
        isSpinBowler =
            currentBowler == BowlerType.LeftArmOrthodoxSpin ||
            currentBowler == BowlerType.LeftArmWristSpin ||
            currentBowler == BowlerType.RightArmOffSpin ||
            currentBowler == BowlerType.RightArmLegSpin;

        // HAND SPAWN
        activeSpawn = isLeftArm ? leftHandSpawn : rightHandSpawn;

        // 🔥 ONLY ONE ANIMATOR TRIGGER
        if (animator != null)
        {
            animator.ResetTrigger("Bowl");
            animator.SetTrigger("Bowl");
        }
    }

    BallVariation PickVariation()
    {
        if (variations.Count == 0) return null;

        int idx = Random.Range(0, variations.Count);
        if (idx == lastIndex && variations.Count > 1)
            idx = (idx + 1) % variations.Count;

        lastIndex = idx;
        return variations[idx];
    }

    // 🔥 ANIMATION EVENT
    public void OnBallRelease()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("BowlingManager: ballPrefab is not assigned.");
            return;
        }

        if (activeSpawn == null)
        {
            Debug.LogError("BowlingManager: activeSpawn is not set.");
            return;
        }

        BallVariation v = PickVariation();
        if (v == null)
        {
            Debug.LogWarning("BowlingManager: no ball variations available.");
            return;
        }

        float finalSwing = isLeftArm ? -v.swingMeters : v.swingMeters;

        GameObject ball = Instantiate(
            ballPrefab,
            activeSpawn.position,
            activeSpawn.rotation
        );

        BallThrower ballThrower = ball.GetComponent<BallThrower>();
        if (ballThrower == null)
        {
            Debug.LogError("BowlingManager: BallThrower component is missing on the ball prefab.");
            return;
        }

        ballThrower.Launch(
            activeSpawn.position,
            v.targetPoint.position,
            v.timeToTarget,
            finalSwing,
            v.spinTorque
        );

        OnBallDelivered();
    }

    void OnBallDelivered()
    {
        ballCount++;

        if (ballCount >= ballsPerOver)
        {
            ballCount = 0;
            bowlerIndex = (bowlerIndex + 1) % bowlerOrder.Length;
            currentBowler = bowlerOrder[bowlerIndex];
            UpdateBowlingStyleUI();
        }

        // Re-fire the Bowl trigger after EVERY delivery so the next ball
        // is bowled. Bowler rotation still happens only on over completion.
        ApplyBowlerType();
    }

    void UpdateBowlingStyleUI()
    {
        if (bowlingStyleUI == null) return;

        string text = GetBowlerDisplayName(currentBowler);
        Color color = isSpinBowler ? Color.cyan : Color.yellow;

        bowlingStyleUI.Show(text, color);
    }

    string GetBowlerDisplayName(BowlerType type)
    {
        switch (type)
        {
            case BowlerType.RightArmFast: return "RIGHT ARM FAST BOWLER";
            case BowlerType.LeftArmFast: return "LEFT ARM FAST BOWLER";
            case BowlerType.RightArmMediumFast: return "RIGHT ARM MEDIUM FAST";
            case BowlerType.LeftArmMediumFast: return "LEFT ARM MEDIUM FAST";
            case BowlerType.RightArmOffSpin: return "RIGHT ARM OFF SPIN";
            case BowlerType.RightArmLegSpin: return "RIGHT ARM LEG SPIN";
            case BowlerType.LeftArmOrthodoxSpin: return "LEFT ARM ORTHODOX SPIN";
            case BowlerType.LeftArmWristSpin: return "LEFT ARM WRIST SPIN";
        }
        return "";
    }
}


// ---------------------------------------------------------------------------------------------------------------------------------------------
  
//   check 3.0 

// ---------------------------------------------------------------------------------------------------------------------------------------------

// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public enum BowlerType
// {
//     LeftArmFast,
//     RightArmFast,
//     LeftArmMediumFast,
//     RightArmMediumFast,
//     LeftArmOrthodoxSpin,
//     LeftArmWristSpin,
//     RightArmOffSpin,
//     RightArmLegSpin
// }

// [System.Serializable]
// public class BallVariation
// {
//     public string name;
//     public Transform targetPoint;
//     public float timeToTarget = 0.6f;
//     public float swingMeters = 0f;
//     public Vector3 spinTorque = Vector3.zero;
// }

// public class BowlingManager : MonoBehaviour
// {
//     [Header("References")]
//     public Animator animator;
//     public Transform rightHandSpawn;
//     public Transform leftHandSpawn;
//     public GameObject ballPrefab;

//     [Header("UI")]
//     public BowlingStyleUI bowlingStyleUI;

//     [Header("Over Settings")]
//     public int ballsPerOver = 6;
//     public float nextBallDelay = 2.5f;   // 🔥 AUTO NEXT BALL

//     [Header("Bowler Rotation Order")]
//     public BowlerType[] bowlerOrder =
//     {
//         BowlerType.RightArmFast,
//         BowlerType.LeftArmFast,
//         BowlerType.RightArmOffSpin,
//         BowlerType.LeftArmOrthodoxSpin
//     };

//     [Header("Ball Variations")]
//     public List<BallVariation> variations = new List<BallVariation>();

//     Transform activeSpawn;
//     int lastIndex = -1;

//     int ballCount = 0;
//     int bowlerIndex = 0;

//     bool isLeftArm;
//     bool isSpinBowler;

//     void Start()
//     {
//         StartCoroutine(BowlingLoop()); // 🔥 AUTO LOOP
//     }

//     IEnumerator BowlingLoop()
//     {
//         while (true)
//         {
//             ApplyBowlerType();

//             animator.ResetTrigger("Bowl");
//             animator.SetTrigger("Bowl");

//             yield return new WaitForSeconds(nextBallDelay);
//         }
//     }

//     void ApplyBowlerType()
//     {
//         BowlerType currentBowler = bowlerOrder[bowlerIndex];

//         isLeftArm =
//             currentBowler == BowlerType.LeftArmFast ||
//             currentBowler == BowlerType.LeftArmMediumFast ||
//             currentBowler == BowlerType.LeftArmOrthodoxSpin ||
//             currentBowler == BowlerType.LeftArmWristSpin;

//         isSpinBowler =
//             currentBowler == BowlerType.LeftArmOrthodoxSpin ||
//             currentBowler == BowlerType.LeftArmWristSpin ||
//             currentBowler == BowlerType.RightArmOffSpin ||
//             currentBowler == BowlerType.RightArmLegSpin;

//         activeSpawn = isLeftArm ? leftHandSpawn : rightHandSpawn;

//         if (bowlingStyleUI != null)
//         {
//             bowlingStyleUI.Show(
//                 GetBowlerDisplayName(currentBowler),
//                 isSpinBowler ? Color.cyan : Color.yellow
//             );
//         }
//     }

//     BallVariation PickVariation()
//     {
//         if (variations.Count == 0) return null;

//         int idx = Random.Range(0, variations.Count);
//         if (idx == lastIndex && variations.Count > 1)
//             idx = (idx + 1) % variations.Count;

//         lastIndex = idx;
//         return variations[idx];
//     }

//     // 🔥 ANIMATION EVENT
//     public void OnBallRelease()
//     {
//         BallVariation v = PickVariation();
//         if (v == null) return;

//         GameObject ball = Instantiate(
//             ballPrefab,
//             activeSpawn.position,
//             activeSpawn.rotation
//         );

//         ball.GetComponent<BallThrower>().Launch(
//             activeSpawn.position,
//             v.targetPoint.position,
//             v.timeToTarget,
//             isLeftArm ? -v.swingMeters : v.swingMeters,
//             v.spinTorque
//         );

//         ballCount++;

//         if (ballCount >= ballsPerOver)
//         {
//             ballCount = 0;
//             bowlerIndex = (bowlerIndex + 1) % bowlerOrder.Length;
//         }
//     }

//     string GetBowlerDisplayName(BowlerType type)
//     {
//         switch (type)
//         {
//             case BowlerType.RightArmFast: return "RIGHT ARM FAST";
//             case BowlerType.LeftArmFast: return "LEFT ARM FAST";
//             case BowlerType.RightArmMediumFast: return "RIGHT ARM MEDIUM";
//             case BowlerType.LeftArmMediumFast: return "LEFT ARM MEDIUM";
//             case BowlerType.RightArmOffSpin: return "RIGHT ARM OFF SPIN";
//             case BowlerType.RightArmLegSpin: return "RIGHT ARM LEG SPIN";
//             case BowlerType.LeftArmOrthodoxSpin: return "LEFT ARM ORTHODOX";
//             case BowlerType.LeftArmWristSpin: return "LEFT ARM WRIST SPIN";
//         }
//         return "";
//     }
// }


// check 4.0
// In this the bowler continuously throws the ball and the text on the pitch changes after each over 

// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public enum BowlerType
// {
//     LeftArmFast = 1,
//     LeftArmMediumFast = 2,
//     LeftArmOrthodoxSpin = 3,
//     LeftArmWristSpin = 4,
//     RightArmFast = 5,
//     RightArmMediumFast = 6,
//     RightArmLegSpin = 7,
//     RightArmOffSpin = 8
// }

// [System.Serializable]
// public class BallVariation
// {
//     public string name;
//     public Transform targetPoint;
//     public float timeToTarget = 0.6f;
//     public float swingMeters = 0f;
//     public Vector3 spinTorque = Vector3.zero;
// }

// public class BowlingManager : MonoBehaviour
// {
//     [Header("References")]
//     public Animator animator;
//     public Transform rightHandSpawn;
//     public Transform leftHandSpawn;
//     public GameObject ballPrefab;

//     [Header("UI")]
//     public BowlingStyleUI bowlingStyleUI;

//     [Header("Over Settings")]
//     public int ballsPerOver = 6;
//     public float nextBallDelay = 2.5f;

//     [Header("Bowler Rotation Order")]
//     public BowlerType[] bowlerOrder =
//     {
//         BowlerType.LeftArmFast,
//         BowlerType.LeftArmMediumFast,
//         BowlerType.LeftArmOrthodoxSpin,
//         BowlerType.RightArmFast,
//         BowlerType.RightArmOffSpin
//     };

//     [Header("Ball Variations")]
//     public List<BallVariation> variations = new List<BallVariation>();

//     int ballCount = 0;
//     int bowlerIndex = 0;
//     int lastIndex = -1;

//     Transform activeSpawn;
//     bool isLeftArm;
//     bool isSpinBowler;

//     void Start()
//     {
//         StartCoroutine(BowlingLoop());
//     }

//     IEnumerator BowlingLoop()
//     {
//         while (true)
//         {
//             ApplyBowlerType();

//             yield return new WaitForSeconds(nextBallDelay);
//         }
//     }

//     void ApplyBowlerType()
//     {
//         BowlerType currentBowler = bowlerOrder[bowlerIndex];

//         // ARM TYPE
//         isLeftArm =
//             currentBowler == BowlerType.LeftArmFast ||
//             currentBowler == BowlerType.LeftArmMediumFast ||
//             currentBowler == BowlerType.LeftArmOrthodoxSpin ||
//             currentBowler == BowlerType.LeftArmWristSpin;

//         // SPIN TYPE
//         isSpinBowler =
//             currentBowler == BowlerType.LeftArmOrthodoxSpin ||
//             currentBowler == BowlerType.LeftArmWristSpin ||
//             currentBowler == BowlerType.RightArmOffSpin ||
//             currentBowler == BowlerType.RightArmLegSpin;

//         activeSpawn = isLeftArm ? leftHandSpawn : rightHandSpawn;

//         // 🔥 MAIN FIX — ANIMATOR CONTROL
//         animator.SetInteger("BowlingTy", (int)currentBowler);
//         animator.ResetTrigger("Bowl");
//         animator.SetTrigger("Bowl");

//         // UI
//         if (bowlingStyleUI != null)
//         {
//             bowlingStyleUI.Show(
//                 GetBowlerDisplayName(currentBowler),
//                 isSpinBowler ? Color.cyan : Color.yellow
//             );
//         }
//     }

//     BallVariation PickVariation()
//     {
//         if (variations.Count == 0) return null;

//         int idx = Random.Range(0, variations.Count);
//         if (idx == lastIndex && variations.Count > 1)
//             idx = (idx + 1) % variations.Count;

//         lastIndex = idx;
//         return variations[idx];
//     }

//     // 🎯 ANIMATION EVENT
//     public void OnBallRelease()
//     {
//         BallVariation v = PickVariation();
//         if (v == null) return;

//         GameObject ball = Instantiate(
//             ballPrefab,
//             activeSpawn.position,
//             activeSpawn.rotation
//         );

//         ball.GetComponent<BallThrower>().Launch(
//             activeSpawn.position,
//             v.targetPoint.position,
//             v.timeToTarget,
//             isLeftArm ? -v.swingMeters : v.swingMeters,
//             v.spinTorque
//         );

//         ballCount++;

//         if (ballCount >= ballsPerOver)
//         {
//             ballCount = 0;
//             bowlerIndex = (bowlerIndex + 1) % bowlerOrder.Length;
//         }
//     }

//     string GetBowlerDisplayName(BowlerType type)
//     {
//         return type.ToString().ToUpper();
//     }
// }
