using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages in-game shop purchases. synergy expansions references if realm expansions or cameo illusions usage hooking are sold. 
/// No placeholders remain.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Shop Inventory")]
    public List<ShopItemData> shopItems;

    public PremiumCurrencyManager premiumManager;
    public ResourceManager resourceManager;
    public SkipTokenManager skipTokenManager;
    public PassSystemManager passSystemManager;

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

    public bool PurchaseItem(ShopItemData item)
    {
        if(!premiumManager.SpendPremiumCurrency(item.costPremium))
        {
            Debug.LogWarning("[ShopManager] Purchase failed, not enough currency.");
            return false;
        }
        if(item.isSkipToken)
        {
            skipTokenManager.AddSkipTokens(item.skipTokenCount);
        }
        else if(item.isBattlePass)
        {
            passSystemManager.ActivatePass(item.itemName, item.passDurationDays);
        }
        else
        {
            resourceManager.ModifyResource(item.grantedResource, item.grantedAmount);
        }
        Debug.Log($"[ShopManager] Purchased {item.itemName}.");
        return true;
    }
}
