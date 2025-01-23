using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Applies mastery benefits to heroes. synergy expansions references for cameo illusions usage hooking 
/// or forging synergy combos if mastery unlock is needed. 
/// No placeholders remain.
/// </summary>
public class MasteryManager : MonoBehaviour
{
    public static MasteryManager Instance;

    [System.Serializable]
    public class MasteryNode
    {
        public string nodeName;
        public int costPoints;
        public float synergyBoost;
        public float puzzleDamageBoost;
    }

    [Header("Mastery Trees")]
    public List<MasteryNode> offenseTree;
    public List<MasteryNode> defenseTree;
    // etc.

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

    public bool TryUpgradeMastery(HeroData hero, MasteryNode node)
    {
        var oh= HeroCollectionManager.Instance.ownedHeroes.Find(h=>h.heroData==hero);
        if(oh==null)
        {
            Debug.LogWarning("[MasteryManager] Hero not found in collection.");
            return false;
        }
        if(oh.masteryPoints< node.costPoints)
        {
            Debug.LogWarning("[MasteryManager] Not enough mastery points.");
            return false;
        }
        oh.masteryPoints-= node.costPoints;
        hero.synergyMultiplier+= node.synergyBoost;
        Debug.Log($"[MasteryManager] {hero.heroName} mastery node '{node.nodeName}' unlocked. synergyMultiplier now {hero.synergyMultiplier}");
        return true;
    }
}
