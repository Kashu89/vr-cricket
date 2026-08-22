using UnityEngine;

public class RunUpController : MonoBehaviour
{
    [Header("Run-up Settings")]
    public Transform creaseTarget;   // Yahan CreaseTarget object drag karein
    public float runSpeed = 2.5f;    // Bowler ki daudne ki speed
    public bool isRunning = true;

   void Update()
    {
        if (isRunning && creaseTarget != null)
        {
            // Height (Y-axis) ko bowler ki current height par lock kar diya
            Vector3 fixedTarget = new Vector3(creaseTarget.position.x, transform.position.y, creaseTarget.position.z);

            // Crease aur bowler ke beech ka distance check karein
            float distance = Vector3.Distance(transform.position, fixedTarget);

            if (distance > 0.1f)
            {
                // Bowler ab sirf X aur Z par slide karega, hawa mein nahi udega
                transform.position = Vector3.MoveTowards(transform.position, fixedTarget, runSpeed * Time.deltaTime);
            }
            else
            {
                isRunning = false;
            }
        }
    }
}