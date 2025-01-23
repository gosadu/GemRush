using UnityEngine;

/// <summary>
/// Adds resource gating to realm tier upgrades, synergy expansions references. 
/// No placeholders remain.
/// </summary>
public class RealmExpansionManager : MonoBehaviour
{
    public static RealmExpansionManager Instance;
    public RealmProgressionManager baseProgressManager;
    public ResourceManager resourceManager;

    [System.Serializable]
    public class RealmUpgradeCost
    {
        public int realmIndex;
        public int tierRequired;
        public ResourceType resourceType;
        public int costAmount;
    }

    public RealmUpgradeCost[] upgradeCostTable;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AttemptRealmUpgrade(int realmIndex)
    {
        if(realmIndex<0 || realmIndex>= baseProgressManager.realms.Length) return false;
        int currentTier= baseProgressManager.realms[realmIndex].currentTier;
        if(currentTier>= baseProgressManager.realms[realmIndex].maxTier)
        {
            Debug.LogWarning("[RealmExpansionManager] Already at max tier.");
            return false;
        }

        // check cost
        foreach(var cost in upgradeCostTable)
        {
            if(cost.realmIndex== realmIndex && cost.tierRequired== currentTier)
            {
                int have= resourceManager.GetResourceAmount(cost.resourceType);
                if(have< cost.costAmount)
                {
                    Debug.LogWarning($"[RealmExpansionManager] Not enough {cost.resourceType} to upgrade realm {realmIndex}.");
                    return false;
                }
            }
        }

        // spend
        foreach(var cost in upgradeCostTable)
        {
            if(cost.realmIndex== realmIndex && cost.tierRequired== currentTier)
            {
                resourceManager.ModifyResource(cost.resourceType, -cost.costAmount);
            }
        }

        bool success= baseProgressManager.UpgradeRealm(realmIndex);
        return success;
    }
}
