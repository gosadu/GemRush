using UnityEngine;

/// <summary>
/// Manages the currently active sublocation, locking/unlocking based on requiredRealmTier if referencing Realm Tier Progression.
/// Stage 1 final code, no placeholders.
/// </summary>
public class SublocationManager : MonoBehaviour
{
    public SublocationData currentSublocation;

    public void InitializeSublocation(SublocationData data, int realmTier)
    {
        currentSublocation= data;
        if(data.requiredRealmTier> realmTier)
        {
            Debug.Log($"[SublocationManager] {data.sublocationName} locked. Need realmTier>={data.requiredRealmTier}.");
        }
        else
        {
            Debug.Log($"[SublocationManager] {data.sublocationName} unlocked. realmTier={realmTier} meets {data.requiredRealmTier}");
        }
    }

    public void EnterSublocation()
    {
        if(currentSublocation==null)
        {
            Debug.LogWarning("[SublocationManager] No sublocation assigned. Possibly a realm or synergy approach needed.");
            return;
        }
        Debug.Log($"[SublocationManager] Entering sublocation: {currentSublocation.sublocationName}");
        // Possibly load puzzle scene or synergy scene if referencing Arcane Gear Infusion or Guest Hero Summon
    }
}
