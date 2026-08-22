using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ModuleSelectorUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject moduleButtonPrefab;
    public Transform buttonContainer;
    public TextMeshProUGUI playerNameDisplay;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI totalSessionsDisplay;
    public GameObject moduleInfoPanel;
    public TextMeshProUGUI moduleInfoText;
    public Button startModuleButton;
    
    [Header("Module Data")]
    private string[] moduleNames = {
        "⚡ Reaction Time & Anticipation",
        "👁️ Gaze Training / Visual Focus",
        "🤝 Hand-Eye Coordination",
        "👀 Peripheral Awareness",
        "🎯 Smooth Pursuit",
        "🔄 Saccadic Movement",
        "📏 Vergence",
        "🔍 Visual Acuity",
        "🌓 Contrast Sensitivity",
        "📐 Depth Perception",
        "🧭 Spatial Awareness",
        "⏱️ Predictive Timing",
        "🧠 Visual Memory & Decision Making"
    };
    
    private string[] moduleDescriptions = {
        "Predict ball delivery before halfway point",
        "Maintain fixation during distractions",
        "Intercept moving targets with precision",
        "Detect off-axis movements",
        "Follow moving objects smoothly",
        "Rapid focus shifts between targets",
        "Track depth as ball approaches",
        "Clarity under motion",
        "Detect ball in varying light",
        "Judge distance and bounce",
        "Maintain position awareness",
        "Time interception perfectly",
        "Recall plays and decide quickly"
    };
    
    private Color[] moduleColors = {
        new Color(1f, 0.2f, 0.2f),
        new Color(0.2f, 0.4f, 1f),
        new Color(0.2f, 0.8f, 0.2f),
        new Color(1f, 0.8f, 0f),
        new Color(0f, 0.8f, 0.8f),
        new Color(0.8f, 0f, 0.8f),
        new Color(0.5f, 0.5f, 0.5f),
        new Color(0.2f, 0.6f, 0.8f),
        new Color(0.8f, 0.4f, 0f),
        new Color(0f, 0.6f, 0.4f),
        new Color(0.6f, 0.2f, 0.6f),
        new Color(0.8f, 0.8f, 0.2f),
        new Color(0.2f, 0.8f, 0.6f)
    };
    
    private int selectedIndex = -1;
    
    void Start()
    {
        UpdatePlayerInfo();
        CreateModuleButtons();
        
        if (moduleInfoPanel != null)
            moduleInfoPanel.SetActive(false);
        
        if (startModuleButton != null)
            startModuleButton.onClick.AddListener(StartSelectedModule);
        
        // Subscribe to GameManager events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnModuleEnded += OnModuleEnded;
        }
    }
    
    void UpdatePlayerInfo()
    {
        if (GameManager.Instance != null)
        {
            playerNameDisplay.text = $"🏏 {GameManager.Instance.PlayerName}";
            levelDisplay.text = $"Level: {GameManager.Instance.PlayerLevel}";
            totalSessionsDisplay.text = $"Sessions: {GameManager.Instance.TotalSessions}";
        }
    }
    
    void CreateModuleButtons()
    {
        // Clear existing
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);
        
        for (int i = 0; i < moduleNames.Length; i++)
        {
            int index = i;
            GameObject buttonObj = Instantiate(moduleButtonPrefab, buttonContainer);
            
            Button btn = buttonObj.GetComponent<Button>();
            TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
            Image bgImage = buttonObj.GetComponent<Image>();
            
            if (texts.Length >= 2)
            {
                texts[0].text = moduleNames[i];
                texts[1].text = moduleDescriptions[i];
            }
            
            if (bgImage != null)
                bgImage.color = moduleColors[i % moduleColors.Length];
            
            // Show stats on button
            if (GameManager.Instance != null)
            {
                string key = GetModuleKey(i);
                float avgScore = GameManager.Instance.GetAverageScore(key);
                if (avgScore > 0)
                {
                    // Add score badge (optional)
                }
            }
            
            btn.onClick.AddListener(() => SelectModule(index));
            
            // Hover effect (optional)
            // Add hover script
        }
    }
    
    string GetModuleKey(int index)
    {
        string[] keys = {
            "ReactionTime", "GazeTraining", "HandEyeCoordination",
            "PeripheralAwareness", "SmoothPursuit", "SaccadicMovement",
            "Vergence", "VisualAcuity", "ContrastSensitivity",
            "DepthPerception", "SpatialAwareness", "PredictiveTiming",
            "VisualMemory"
        };
        return keys[index];
    }
    
    void SelectModule(int index)
    {
        selectedIndex = index;
        
        if (moduleInfoPanel != null)
        {
            moduleInfoPanel.SetActive(true);
            string key = GetModuleKey(index);
            float avgScore = GameManager.Instance != null ? GameManager.Instance.GetAverageScore(key) : 0f;
            float avgRT = GameManager.Instance != null ? GameManager.Instance.GetAverageReactionTime(key) : 0f;
            
            moduleInfoText.text = $"<size=36>{moduleNames[index]}</size>\n\n" +
                                  $"{moduleDescriptions[index]}\n\n" +
                                  $"📊 Best Score: {(avgScore > 0 ? avgScore.ToString("F0") : "Not played yet")}\n" +
                                  $"⏱️ Avg Reaction: {(avgRT > 0 ? avgRT.ToString("F3") + "s" : "N/A")}";
        }
        
        Debug.Log($"Selected: {moduleNames[index]}");
    }
    
    void StartSelectedModule()
    {
        if (selectedIndex < 0)
        {
            Debug.LogWarning("Please select a module first!");
            return;
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartModule(selectedIndex);
        }
        
        // Load module scene
        // SceneManager.LoadScene($"Module_{selectedIndex + 1}");
        // Or activate module GameObject
        
        Debug.Log($"🚀 Starting Module {selectedIndex + 1}");
    }
    
    void OnModuleEnded(string moduleName)
    {
        UpdatePlayerInfo();
        // Refresh stats on buttons
        CreateModuleButtons();
    }
    
    public void ShowAllStats()
    {
        if (AnalyticsEngine.Instance == null) return;
        
        OverallStats stats = AnalyticsEngine.Instance.GetOverallStats();
        string statsText = "<size=36>📊 Overall Stats</size>\n\n";
        statsText += $"Total Sessions: {stats.totalSessions}\n";
        statsText += $"Average Score: {stats.averageOverallScore:F0}\n";
        statsText += $"Player Level: {stats.playerLevel}\n\n";
        
        foreach (var module in stats.moduleStats)
        {
            statsText += $"{module.moduleName}: {module.averageScore:F0}";
            if (module.improvement > 0)
                statsText += $" ▲ +{module.improvement:F0}";
            statsText += "\n";
        }
        
        // Show in a popup or panel
        Debug.Log(statsText);
    }
    
    public void ResetData()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetPlayerData();
            UpdatePlayerInfo();
            CreateModuleButtons();
            Debug.Log("Data reset!");
        }
    }
}