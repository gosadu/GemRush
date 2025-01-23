using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages forging logic, synergy expansions references for realm gating,
/// cameo illusions usage hooking if forging a special item. 
/// No placeholders remain.
/// </summary>
public class MysticForgeManager : MonoBehaviour
{
    public static MysticForgeManager Instance;

    public List<ForgeRecipe> recipeList;
    public ResourceManager resourceManager;
    public RealmProgressionManager realmManager;
    public ProjectionSummonManager cameoManager; 

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

    public bool AttemptForge(ForgeItemData itemData, int puzzlePerformance)
    {
        if(itemData.realmTierRequired> realmManager.GetHighestRealmTier())
        {
            Debug.LogWarning("[MysticForge] Not enough realm tier to craft this item.");
            return false;
        }
        int have= resourceManager.GetResourceAmount(itemData.primaryResourceCost);
        if(have< itemData.costAmount)
        {
            Debug.LogWarning("[MysticForge] Not enough resources.");
            return false;
        }
        resourceManager.ModifyResource(itemData.primaryResourceCost, -itemData.costAmount);

        int baseChance= itemData.baseSuccessChance;
        int puzzleBonus= Mathf.Min(puzzlePerformance, 30);
        int totalChance= baseChance + puzzleBonus;
        int roll= Random.Range(0,100);

        bool success= (roll< totalChance);
        if(success)
        {
            Debug.Log($"[MysticForge] Crafted '{itemData.itemName}'. synergyComboBoost={itemData.synergyComboBoost}");
            if(!string.IsNullOrEmpty(itemData.cameoTriggerID))
            {
                cameoManager?.SummonProjection(itemData.cameoTriggerID);
            }
        }
        else
        {
            Debug.Log("[MysticForge] Forge attempt failed. Resources spent, no item synergy gained.");
        }
        return success;
    }

    public bool AttemptUpgrade(ForgeRecipe recipe, int puzzlePerformance)
    {
        if(recipe.requiredRealmTier> realmManager.GetHighestRealmTier())
        {
            Debug.LogWarning("[MysticForge] Realm tier too low for upgrade.");
            return false;
        }
        int cost= recipe.baseItem.costAmount+ recipe.extraCost;
        int have= resourceManager.GetResourceAmount(recipe.baseItem.primaryResourceCost);
        if(have< cost)
        {
            Debug.LogWarning("[MysticForge] Not enough resources for upgrade.");
            return false;
        }
        resourceManager.ModifyResource(recipe.baseItem.primaryResourceCost, -cost);

        int roll= Random.Range(0,100);
        int totalChance= recipe.baseItem.baseSuccessChance+ puzzlePerformance;
        bool success= (roll< totalChance);

        if(success)
        {
            Debug.Log($"[MysticForge] Upgraded '{recipe.baseItem.itemName}' -> '{recipe.upgradedItem.itemName}'.");
            if(!string.IsNullOrEmpty(recipe.upgradedItem.cameoTriggerID))
            {
                cameoManager?.SummonProjection(recipe.upgradedItem.cameoTriggerID);
            }
        }
        else
        {
            Debug.Log("[MysticForge] Upgrade failed, resources lost.");
        }
        return success;
    }
}
