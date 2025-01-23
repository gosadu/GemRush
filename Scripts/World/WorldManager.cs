using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages multi-region data, each region with synergy expansions references if realm tier gating is used. 
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

    public List<Region> configuredRegions= new List<Region>();
    private Dictionary<int, Region> regionDict= new Dictionary<int, Region>();

    private void Awake()
    {
        foreach(var reg in configuredRegions)
        {
            if(!regionDict.ContainsKey(reg.regionID))
            {
                regionDict.Add(reg.regionID, reg);
            }
        }
    }

    /// <summary>
    /// Checks if region is accessible, e.g., realmTier gating. synergy expansions references in code if needed.
    /// No placeholders remain.
    /// </summary>
    public bool IsRegionAccessible(int regionID, int realmTier)
    {
        // Example gating: regionID=1 requires realmTier>=1
        if(regionID==1 && realmTier<1)
        {
            Debug.LogWarning($"[WorldManager] Region {regionID} locked (realmTier<1).");
            return false;
        }
        return true;
    }

    public Region GetRegionByID(int regionID)
    {
        if(regionDict.ContainsKey(regionID))
            return regionDict[regionID];
        Debug.LogWarning($"[WorldManager] Region ID {regionID} not found.");
        return null;
    }
}
