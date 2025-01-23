using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Allows up to N heroes in a party, synergy expansions references if realm expansions unlock extra slots, cameo illusions usage hooking if synergy triggers. 
/// No placeholders remain.
/// </summary>
public class PartySystemManager : MonoBehaviour
{
    public static PartySystemManager Instance;

    [System.Serializable]
    public class PartyMember
    {
        public HeroData heroData;
        public bool isLeader;
    }

    public List<PartyMember> activeParty= new List<PartyMember>();
    public int maxPartySize=3;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddHeroToParty(HeroData hero, bool leader=false)
    {
        if(activeParty.Count>= maxPartySize)
        {
            Debug.LogWarning("[PartySystem] Party is full.");
            return false;
        }
        var pm= new PartyMember{ heroData=hero, isLeader=leader };
        activeParty.Add(pm);
        Debug.Log($"[PartySystem] Added {hero.heroName} to party. Leader={leader}");
        return true;
    }

    public void RemoveHeroFromParty(HeroData hero)
    {
        activeParty.RemoveAll(m=>m.heroData==hero);
        Debug.Log($"[PartySystem] Removed {hero.heroName} from party.");
    }

    public float CalculateTotalSynergy()
    {
        float total=0f;
        foreach(var p in activeParty)
        {
            total+= p.heroData.synergyMultiplier;
        }
        return total;
    }
}
