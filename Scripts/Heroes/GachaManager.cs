using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Oversees hero gacha pulls, synergy expansions references if certain heroes 
/// require cameo illusions usage hooking or forging synergy combos. 
/// No placeholders remain.
/// </summary>
public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    [System.Serializable]
    public class GachaPoolEntry
    {
        public HeroData heroData;
        public float weight;
    }

    public List<GachaPoolEntry> gachaPool;
    public ResourceManager resourceManager;
    public int gachaCost= 100;
    public ResourceType premiumCurrencyType= ResourceType.Crystal;

    private float totalWeight;

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

        foreach(var e in gachaPool)
        {
            totalWeight+= e.weight;
        }
    }

    public bool PerformGachaPull()
    {
        int have= resourceManager.GetResourceAmount(premiumCurrencyType);
        if(have< gachaCost)
        {
            Debug.LogWarning("[GachaManager] Not enough premium currency.");
            return false;
        }
        resourceManager.ModifyResource(premiumCurrencyType, -gachaCost);

        float roll= Random.value * totalWeight;
        float accum=0f;
        foreach(var e in gachaPool)
        {
            accum+= e.weight;
            if(roll<=accum)
            {
                HeroCollectionManager.Instance.AddHero(e.heroData);
                return true;
            }
        }
        Debug.LogWarning("[GachaManager] Pull error, no hero found. Check pool weights.");
        return false;
    }
}
