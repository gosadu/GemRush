using UnityEngine;

/// <summary>
/// Manages realm tiers across different realms. 
/// synergy expansions references for gating forging or cameo illusions usage hooking. 
/// No placeholders remain.
/// </summary>
public class RealmProgressionManager : MonoBehaviour
{
    public static RealmProgressionManager Instance;

    public RealmTierData[] realms;

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

    public bool UpgradeRealm(int index)
    {
        if(index<0 || index>= realms.Length) return false;
        if(realms[index].currentTier< realms[index].maxTier)
        {
            realms[index].currentTier++;
            Debug.Log($"[RealmProgressionManager] Upgraded {realms[index].realmName} to tier {realms[index].currentTier}.");
            return true;
        }
        Debug.LogWarning($"[RealmProgressionManager] {realms[index].realmName} is already at max tier.");
        return false;
    }

    public int GetHighestRealmTier()
    {
        int highest=0;
        foreach(var rd in realms)
        {
            if(rd.currentTier> highest) highest= rd.currentTier;
        }
        return highest;
    }
}
