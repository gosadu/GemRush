using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Maintains the player's owned heroes, synergy expansions references if forging synergy combos 
/// or cameo illusions usage hooking references are relevant. 
/// No placeholders remain.
/// </summary>
public class HeroCollectionManager : MonoBehaviour
{
    public static HeroCollectionManager Instance;

    [System.Serializable]
    public class OwnedHero
    {
        public HeroData heroData;
        public int level;
        public int masteryPoints;
    }

    public List<OwnedHero> ownedHeroes = new List<OwnedHero>();

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

    public void AddHero(HeroData newHero)
    {
        var oh= new OwnedHero{ heroData=newHero, level=1, masteryPoints=0 };
        ownedHeroes.Add(oh);
        Debug.Log($"[HeroCollection] Acquired hero: {newHero.heroName}, rarity={newHero.rarity}");
    }

    public void GrantMasteryPoints(HeroData hero, int points)
    {
        var owned= ownedHeroes.Find(h=>h.heroData==hero);
        if(owned!=null)
        {
            owned.masteryPoints+= points;
            Debug.Log($"[HeroCollection] {hero.heroName} mastery +{points}, total {owned.masteryPoints}");
        }
    }
}
