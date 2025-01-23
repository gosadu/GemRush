using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class OrchardDistrictTierData
{
    public int tier;
    public int costSeeds;
    public int costWood;
    public int costOre;
    public int costBlossoms;
    public float synergyBonusPercent; 
    public float forgingDiscountPercent;
    public float puzzleSpawnBoostPercent; 
    public float forgingSuccessBoostPercent; 
}

[CreateAssetMenu(fileName = "OrchardDistrictData", menuName = "GameData/OrchardDistrictData")]
public class OrchardDistrictData : ScriptableObject
{
    public string districtName;
    public List<OrchardDistrictTierData> tierData;
    public int currentTier;
    public float dailyResourceAmount; // Base resource earned each day
}

public class OrchardManager : MonoBehaviour
{
    [SerializeField] private List<OrchardDistrictData> orchardDistricts;
    [SerializeField] private ProgressionManager progressionManager;

    private DateTime lastOfflineCheck;

    public void InitializeOrchard()
    {
        lastOfflineCheck = DateTime.Now;
        Debug.Log("[OrchardManager] Orchard initialized with final synergy logic.");
    }

    public bool UpgradeDistrict(string districtName)
    {
        OrchardDistrictData district = orchardDistricts.Find(d => d.districtName == districtName);
        if (district == null) return false;

        int currentTier = district.currentTier;
        if (currentTier >= district.tierData.Count)
        {
            Debug.Log("[OrchardManager] Already at max tier for " + districtName);
            return false;
        }

        OrchardDistrictTierData nextTier = district.tierData[currentTier]; 
        bool hasResources = CheckPlayerHasResources(nextTier.costSeeds, nextTier.costWood, nextTier.costOre, nextTier.costBlossoms);
        if (!hasResources)
        {
            Debug.Log("[OrchardManager] Not enough resources to upgrade " + districtName);
            return false;
        }

        SpendResources(nextTier.costSeeds, nextTier.costWood, nextTier.costOre, nextTier.costBlossoms);
        district.currentTier++;

        ApplySynergyBonuses(district);
        return true;
    }

    private bool CheckPlayerHasResources(int seeds, int wood, int ore, int blossoms)
    {
        // Assume the player’s resource totals are tracked in ProgressionManager or a separate ResourceManager.
        // Checking here:
        return (progressionManager.GetSeeds() >= seeds
            && progressionManager.GetWood() >= wood
            && progressionManager.GetOre() >= ore
            && progressionManager.GetBlossoms() >= blossoms);
    }

    private void SpendResources(int seeds, int wood, int ore, int blossoms)
    {
        progressionManager.SpendSeeds(seeds);
        progressionManager.SpendWood(wood);
        progressionManager.SpendOre(ore);
        progressionManager.SpendBlossoms(blossoms);
    }

    private void ApplySynergyBonuses(OrchardDistrictData district)
    {
        OrchardDistrictTierData tierInfo = district.tierData[district.currentTier - 1];

        // Example synergy effect application
        // The single dev can expand synergy to puzzle spawn rates, forging discount, etc.
        float synergy = tierInfo.synergyBonusPercent;
        float forgingDisc = tierInfo.forgingDiscountPercent;
        float puzzleBoost = tierInfo.puzzleSpawnBoostPercent;
        float successBoost = tierInfo.forgingSuccessBoostPercent;

        Debug.Log("[OrchardManager] Synergy bonus: " + synergy
                  + ", forging discount: " + forgingDisc
                  + ", puzzle spawn boost: " + puzzleBoost
                  + ", forging success boost: " + successBoost
                  + " for district: " + district.districtName);
    }

    public void CollectDailyResources()
    {
        // Collect daily from each district
        foreach (OrchardDistrictData d in orchardDistricts)
        {
            float totalGain = d.dailyResourceAmount + (d.currentTier * 2f); 
            progressionManager.AddSeeds(Mathf.RoundToInt(totalGain)); 
            Debug.Log("[OrchardManager] Collected " + totalGain + " Seeds from " + d.districtName);
        }
    }

    public void CheckOfflineGains()
    {
        DateTime now = DateTime.Now;
        double hoursAway = (now - lastOfflineCheck).TotalHours;
        if (hoursAway > 0)
        {
            foreach (OrchardDistrictData d in orchardDistricts)
            {
                float offlineAmount = (float)hoursAway * (d.dailyResourceAmount / 24f);
                offlineAmount += d.currentTier; 
                int finalAmount = Mathf.RoundToInt(offlineAmount);
                progressionManager.AddSeeds(finalAmount);
                Debug.Log("[OrchardManager] Offline orchard gains: " + finalAmount
                          + " seeds for " + d.districtName);
            }
        }
        lastOfflineCheck = now;
    }
}