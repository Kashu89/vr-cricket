using UnityEngine;
using System.Collections; // Coroutine (Timer) ke liye zaroori hai

public class OverManager : MonoBehaviour
{
    [Header("Bowler Setup")]
    public GameObject[] bowlers; 
    public int ballCount = 0;
    
    [Header("Timers")]
    public float overBreakTime = 15f; // Over ke baad 15 second ka gap

    void Start()
    {
        // Game start hote hi ek bowler le aao
        SwitchToRandomBowler();
    }

    public void RecordBall()
    {
        ballCount++;
        Debug.Log("Ball number: " + ballCount);

        if (ballCount >= 6)
        {
            ballCount = 0; // Agle over ke liye reset
            
            // Instantly change karne ki jagah timer start karo
            StartCoroutine(OverBreakRoutine());
        }
    }

    IEnumerator OverBreakRoutine()
    {
        // 1. Ball fenkne ke baad 3 second wait karo (taaki bowler apna action aur follow-through poora kar sake)
        yield return new WaitForSeconds(1f);

        // 2. 3 second baad current bowler ko screen se hata do (taaki wo agli ball na daalne lage)
        for (int i = 0; i < bowlers.Length; i++)
        {
            bowlers[i].SetActive(false);
        }

        // 3. Ab bache hue 12 second ka wait karo (Total 15 seconds ka gap)
        yield return new WaitForSeconds(overBreakTime - 1f);

        // 4. Break khatam, ab naya bowler ground par aayega
        SwitchToRandomBowler();
    }

    void SwitchToRandomBowler()
    {
        for (int i = 0; i < bowlers.Length; i++)
        {
            bowlers[i].SetActive(false);
        }

        int randomIndex = Random.Range(0, bowlers.Length);
        bowlers[randomIndex].SetActive(true);
        Debug.Log("Naya Bowler Aaya: " + bowlers[randomIndex].name);
    }
}