/*************************************************************
 * TopPanelController.cs
 * Attach this script to the TopPanel GameObject.
 * The design is strictly fantasy: imagine ornate metallic frames,
 * swirling arcane runes, or aged parchment as the background.
 *************************************************************/
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // If you're using TextMeshPro

public class TopPanelController : MonoBehaviour
{
    [Header("Fantasy UI References")]
    [SerializeField] private Image enemyBackgroundImage;
    
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider shieldBar;
    
    [SerializeField] private TextMeshProUGUI enemyNameText; 
    // Or use plain UnityEngine.UI.Text if preferred
    
    [Header("Damage Text Parent")]
    [SerializeField] private RectTransform damageTextSpawner;
    
    [Header("Optional Turn/Timer")]
    [SerializeField] private TextMeshProUGUI turnOrTimerText;

    // Example: For demonstration only—your BossManager might call these methods:
    private float currentHP = 1f;     // normalized 0..1 for demonstration
    private float currentShield = 0.3f; // normalized 0..1 for demonstration

    void Start()
    {
        // If references not assigned, log a warning
        if (!enemyBackgroundImage) Debug.LogWarning("[TopPanel] No enemyBackgroundImage assigned.");
        if (!hpBar) Debug.LogWarning("[TopPanel] No HP Bar assigned.");
        if (!shieldBar) Debug.LogWarning("[TopPanel] No Shield Bar assigned.");
        if (!enemyNameText) Debug.LogWarning("[TopPanel] No Enemy Name Text assigned.");
        
        // Initialize default states
        if (hpBar) hpBar.value = currentHP;
        if (shieldBar) shieldBar.value = currentShield;
        if (enemyNameText) enemyNameText.text = "Ancient Wyrm";

        if (turnOrTimerText)
        {
            turnOrTimerText.text = "Turn 1";
        }
    }

    /// <summary>
    /// Updates the HP bar value (0..1).
    /// Could be called from BossManager or EnhancedBoardManager.
    /// </summary>
    public void SetHP(float ratio)
    {
        if (hpBar) hpBar.value = Mathf.Clamp01(ratio);
    }

    /// <summary>
    /// Updates the shield bar value (0..1).
    /// </summary>
    public void SetShield(float ratio)
    {
        if (shieldBar) shieldBar.value = Mathf.Clamp01(ratio);
    }

    /// <summary>
    /// Sets the enemy name text (fantasy style).
    /// </summary>
    public void SetEnemyName(string enemyName)
    {
        if (enemyNameText) enemyNameText.text = enemyName;
    }

    /// <summary>
    /// Spawns a floating damage text near the HP/Shield bars using damageTextSpawner.
    /// You’d instantiate some prefab, set its text, etc.
    /// </summary>
    public void SpawnDamageText(int damageAmount)
    {
        if (!damageTextSpawner)
        {
            Debug.LogWarning("[TopPanel] No DamageTextSpawner assigned!");
            return;
        }
        
        // Example usage:
        // var dmgPopup = Instantiate(damageTextPrefab, damageTextSpawner);
        // dmgPopup.GetComponent<TextMeshProUGUI>().text = "-" + damageAmount.ToString();
        // Then animate it or fade it out as desired.

        Debug.Log("[TopPanel] (Fantasy) Spawning damage text: " + damageAmount);
    }

    /// <summary>
    /// Updates a turn or timer readout (optional).
    /// E.g.: "Turn 5", or "Time Left: 12s"
    /// </summary>
    public void UpdateTurnOrTimer(int turnNumber, float timeLeft)
    {
        if (turnOrTimerText)
        {
            turnOrTimerText.text = 
                $"Turn {turnNumber} - Time: {Mathf.RoundToInt(timeLeft)}s";
        }
    }
}
