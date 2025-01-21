using UnityEngine;
using System.Collections.Generic;

public class ForgingManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> forgingItems;
    [SerializeField] private ProgressionManager progressionManager;
    [SerializeField] private float forgingBaseTime = 15f;

    public void InitializeForging()
    {
        Debug.Log("[ForgingManager] Forging system initialized with final synergy logic.");
    }

    public bool ForgeItem(string itemName)
    {
        ItemData item = forgingItems.Find(i => i.itemName == itemName);
        if (item == null)
        {
            Debug.Log("[ForgingManager] Item not found in forging list: " + itemName);
            return false;
        }

        // This is a final approach: deduct forging cost from resources, queue forging time...
        bool canAfford = progressionManager.GetSeeds() >= 5; 
        if (!canAfford)
        {
            Debug.Log("[ForgingManager] Not enough seeds to forge " + itemName);
            return false;
        }

        progressionManager.SpendSeeds(5);
        CompleteForge(item);
        return true;
    }

    public void CompleteForge(ItemData item)
    {
        // In a real game, wait forgingBaseTime seconds or use a puzzle mini-game.
        Debug.Log("[ForgingManager] Successfully forged item: " + item.itemName
                  + " with effect type: " + item.effectType
                  + " effect value: " + item.effectValue);
        // Possibly add to inventory:
        progressionManager.AddForgedItem(item);
    }

    public bool TransmuteItem(ItemData sourceItem, int blossomCost)
    {
        bool hasBlossoms = progressionManager.GetBlossoms() >= blossomCost;
        if (!hasBlossoms)
        {
            Debug.Log("[ForgingManager] Not enough Arcane Blossoms for transmutation.");
            return false;
        }
        progressionManager.SpendBlossoms(blossomCost);
        Debug.Log("[ForgingManager] Transmuted item: " + sourceItem.itemName);
        // Upgrade rarity or special effect
        return true;
    }
}