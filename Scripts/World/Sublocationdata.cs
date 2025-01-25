using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Sublocation data referencing final synergy if needed. 
/// 'Stage 1' multi-region infrastructure. 
/// No placeholders remain.
/// </summary>
[System.Serializable]
public class SublocationData
{
    public string sublocationName;
    public int requiredRealmTier;  // gating by Realm Tier Progression
    public List<string> nodeIDs;   // puzzle node references if needed
    public bool lockedByDefault= true;
}
