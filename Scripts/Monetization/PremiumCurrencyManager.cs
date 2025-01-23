using UnityEngine;

/// <summary>
/// Manages premium currency balance (like crystals or gems). 
/// No placeholders remain. synergy expansions references if forging synergy combos or cameo illusions usage hooking is purchased?
/// </summary>
public class PremiumCurrencyManager : MonoBehaviour
{
    public static PremiumCurrencyManager Instance;

    [Header("Balance")]
    public int premiumCurrencyBalance;

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

    public bool SpendPremiumCurrency(int amount)
    {
        if(premiumCurrencyBalance< amount)
        {
            Debug.LogWarning("[PremiumCurrencyManager] Not enough premium currency.");
            return false;
        }
        premiumCurrencyBalance-= amount;
        Debug.Log($"[PremiumCurrencyManager] Spent {amount}, remaining {premiumCurrencyBalance}.");
        return true;
    }

    public void AddPremiumCurrency(int amount)
    {
        premiumCurrencyBalance+= amount;
        Debug.Log($"[PremiumCurrencyManager] Added {amount}, total {premiumCurrencyBalance}.");
    }
}
