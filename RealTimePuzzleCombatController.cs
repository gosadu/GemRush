using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the real-time aspect of puzzle combat:
/// - Ticks down player HP over time
/// - (Later phases) can delay or pause HP drain if combos are triggered
/// </summary>
public class RealTimePuzzleCombatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnhancedBoardManager boardManager;
    [SerializeField] private BossManager bossManager;

    [Header("HP Drain Settings")]
    [Tooltip("Base HP drained every interval")]
    public int hpDrainAmount = 5;

    [Tooltip("Seconds between HP drain ticks")]
    public float hpDrainInterval = 2f;

    [Header("Combo Delay Settings")]
    [Tooltip("If true, combos can delay the next HP drain tick.")]
    public bool combosCanDelay = true;

    [Tooltip("Extra delay (in seconds) added per big combo (4+ matches)")]
    public float comboDelayTime = 1f;

    private float nextDrainTime;
    private float currentDelayBonus;

    private void Start()
    {
        // Schedule the first drain
        nextDrainTime = Time.time + hpDrainInterval;
        currentDelayBonus = 0f;

        // Optional: in Phase 2, we’ll refine how combos are detected.
        if (boardManager != null)
        {
            boardManager.OnComboExecuted += OnComboExecuted;
        }
    }

    private void Update()
    {
        if (Time.time >= nextDrainTime)
        {
            DoHPDrainTick();
        }
    }

    private void DoHPDrainTick()
    {
        if (boardManager != null)
        {
            boardManager.ModifyPlayerHP(-hpDrainAmount);
        }

        // Schedule next tick, incorporating any delay bonus
        float finalInterval = hpDrainInterval + currentDelayBonus;
        nextDrainTime = Time.time + finalInterval;

        // Reset the bonus after applying it
        currentDelayBonus = 0f;
    }

    private void OnComboExecuted(int comboSize)
    {
        if (!combosCanDelay) return;

        // If the player pulls off a 4+ match, delay the next HP drain slightly
        if (comboSize >= 4)
        {
            currentDelayBonus += comboDelayTime;
        }
    }
}
