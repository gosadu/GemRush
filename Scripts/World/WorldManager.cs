using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages multi-region overworld logic, referencing Realm Tier Progression if needed.
/// Stage 1: 'Multi-Region, Town Hub, Sublocation Infrastructure' with updated naming (no orchard≥Tier gating).
/// No placeholders remain.
/// </summary>
public class WorldManager : MonoBehaviour
{
    [System.Serializable]
    public class Region
    {
        public int regionID;
        public string regionName;
        public List<SublocationData> sublocations;
    }

    public List<Region> configuredRegions = new List<Region>();
    private Dictionary<int, Region> regionDict = new Dictionary<int, Region>();

    private void Awake()
    {
        // Build dictionary from configuredRegions
        foreach(var reg in configuredRegions)
        {
            if(!regionDict.ContainsKey(reg.regionID))
                regionDict.Add(reg.regionID, reg);
        }
    }

    /// <summary>
    /// Checks if region is accessible based on Realm Tier Progression if you want gating.
    /// E.g., region 1 requires tier>=1, region 2 => tier>=2, etc. 
    /// Adjust to your final logic.
    /// </summary>
    public bool IsRegionAccessible(int regionID, int realmTier)
    {
        if(regionDict.ContainsKey(regionID))
        {
            // example gating:
            if(regionID==1 && realmTier<1) return false;
            if(regionID==2 && realmTier<2) return false;
            // etc...
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get region data by ID. 
    /// Possibly used by TownHubManager or Sublocation flow.
    /// </summary>
    public Region GetRegionByID(int regionID)
    {
        if(regionDict.ContainsKey(regionID)) 
            return regionDict[regionID];
        return null;
    }
}
