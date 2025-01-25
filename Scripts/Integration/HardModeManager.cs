using UnityEngine;

/// <summary>
/// Manages a "Hard Mode" or "Remixed Regions" approach from the final plan:
/// Possibly increases puzzle difficulty, synergy with Realm Tier or Guest Hero Summon if you want advanced logic.
/// No old orchard references remain.
/// </summary>
public class HardModeManager : MonoBehaviour
{
    public bool isHardModeActive = false;
    public float difficultyMultiplier = 1.5f;

    /// <summary>
    /// Enable Hard Mode. Could scale puzzle HP or damage, synergy with Arcane Gear Infusion or Realm Tier gating.
    /// </summary>
    public void EnableHardMode()
    {
        isHardModeActive = true;
        Debug.Log($"[HardModeManager] Hard mode enabled => puzzle difficulty x{difficultyMultiplier}. Potential synergy with Realm Tier or Guest Hero Summon triggers.");
    }

    public void DisableHardMode()
    {
        isHardModeActive = false;
        Debug.Log("[HardModeManager] Hard mode disabled => puzzle difficulty returns to normal.");
    }

    public float GetCurrentMultiplier()
    {
        return (isHardModeActive)? difficultyMultiplier : 1.0f;
    }
}
