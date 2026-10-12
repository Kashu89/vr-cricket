using UnityEngine;
using System.Collections; 

public class RunUpController : MonoBehaviour
{
    [Header("Run-up Settings")]
    public Transform creaseTarget;   
    public float runSpeed = 2.5f;    
    public bool isRunning = true;
    public float resetDelay = 5f;
    public OverManager manager; 

    private Vector3 startPosition; 
    private Quaternion startRotation; // NAYA: Bowler ka angle yaad rakhne ke liye
    private Animator animator;

    void Start()
    {
        // Game start hote hi jagah aur angle dono save kar lo
        startPosition = transform.position;
        startRotation = transform.rotation; 
        
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isRunning && creaseTarget != null)
        {
            Vector3 fixedTarget = new Vector3(creaseTarget.position.x, transform.position.y, creaseTarget.position.z);
            float distance = Vector3.Distance(transform.position, fixedTarget);

            if (distance > 0.1f)
            {
                // Bowler ko hamesha crease ki taraf dekhne par majboor karo
                transform.LookAt(fixedTarget); 
                
                transform.position = Vector3.MoveTowards(transform.position, fixedTarget, runSpeed * Time.deltaTime);
            }
            else
            {
                isRunning = false;
                if (manager != null) manager.RecordBall();
                StartCoroutine(ResetBowlerRoutine());
            }
        }
    }

    IEnumerator ResetBowlerRoutine()
    {
        yield return new WaitForSeconds(resetDelay);

        // 1. Animator ko ek pal ke liye band karo taaki wo teleport ko block na kare
        animator.enabled = false;

        // 2. Position aur Rotation wapas exactly start point (boundary) par set karo
        transform.position = startPosition;
        transform.rotation = startRotation; 

        // 3. Animator wapas chalu karo aur animation ko shuru (0) se play karo
        animator.enabled = true;
        animator.Play("Bowling", 0, 0f); 

        isRunning = true;
    }
}