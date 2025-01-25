using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// If you want advanced expansions or region expansions beyond the base plan,
/// synergy with Realm Tier Progression or Arcane Gear Infusion or Guest Hero Summon if you want.
/// All orchard≥Tier gating cameo illusions forging synergy combos references replaced with final naming.
/// No placeholders remain.
/// </summary>
public class ExpansionManager : MonoBehaviour
{
    [System.Serializable]
    public class ExpansionData
    {
        public string expansionName;
        public bool isUnlocked;
        public int requiredRealmTier;
    }

    public List<ExpansionData> expansions= new List<ExpansionData>();
    public RealmProgressionManager realmTierManager; // final name for orchard≥Tier gating references

    void Start()
    {
        foreach(var e in expansions)
        {
            Debug.Log($"[ExpansionManager] {e.expansionName}, unlocked={e.isUnlocked}, needs realmTier={e.requiredRealmTier}");
        }
    }

    public void CheckUnlocks()
    {
        int tier= realmTierManager.GetHighestRealmTier();
        foreach(var e in expansions)
        {
            if(!e.isUnlocked && tier>= e.requiredRealmTier)
            {
                e.isUnlocked= true;
                Debug.Log($"[ExpansionManager] Unlocked expansion {e.expansionName} => synergy with Arcane Gear Infusion or Guest Hero Summon might apply here.");
            }
        }
    }
}
