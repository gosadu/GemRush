using UnityEngine;

/// <summary>
/// Each realm's tier data. synergy expansions references if relevant to forging or cameo illusions usage hooking unlocks.
/// No placeholders remain.
/// </summary>
[System.Serializable]
public class RealmTierData
{
    public string realmName;
    public int currentTier;
    public int maxTier;
    public int requiredResources; // optional cost
}
