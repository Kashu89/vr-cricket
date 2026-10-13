using UnityEngine;

/// <summary>
/// Auto-starts the cricket match systems when the cricket scene loads.
/// No scene edits and no existing-file edits required.
/// </summary>
public static class CricketMatchBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        // Only in the cricket scene (it has a Bowler).
        if (GameObject.Find("Bowler") == null) return;
        if (Object.FindAnyObjectByType<CricketMatchManager>() != null) return;

        var go = new GameObject("CricketMatchManager");
        go.AddComponent<CricketMatchManager>();
        Debug.Log("CricketMatchBootstrap: match systems started.");
    }
}
