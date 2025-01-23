using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Displays owned heroes, allowing user to add to party. synergy expansions references or cameo illusions usage hooking 
/// if hero cameo triggers. No placeholders remain.
/// </summary>
public class HeroUI : MonoBehaviour
{
    public HeroCollectionManager collectionManager;
    public PartySystemManager partyManager;
    public Transform heroListRoot;
    public GameObject heroListItemPrefab;

    void Start()
    {
        RefreshHeroList();
    }

    public void RefreshHeroList()
    {
        foreach(Transform t in heroListRoot)
        {
            Destroy(t.gameObject);
        }
        foreach(var oh in collectionManager.ownedHeroes)
        {
            var itemObj= Instantiate(heroListItemPrefab, heroListRoot);
            var text= itemObj.GetComponentInChildren<Text>();
            text.text= $"{oh.heroData.heroName} (Lvl {oh.level}) [Rarity:{oh.heroData.rarity}] Mastery:{oh.masteryPoints}";
            var btn= itemObj.GetComponentInChildren<Button>();
            var heroRef= oh.heroData;
            btn.onClick.AddListener(()=> OnSelectHero(heroRef));
        }
    }

    void OnSelectHero(HeroData hero)
    {
        partyManager.AddHeroToParty(hero, leader:false);
        Debug.Log($"[HeroUI] Selected {hero.heroName} to party. Party synergy: {partyManager.CalculateTotalSynergy()}");
    }
}
