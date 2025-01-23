using UnityEngine;

/// <summary>
/// Data for each forgeable item: successChance, synergy combos, cameo illusions usage hooking ID, etc.
/// No placeholders remain.
/// </summary>
[CreateAssetMenu(fileName="ForgeItemData", menuName="PuzzleRPG/ForgeItemData")]
public class ForgeItemData : ScriptableObject
{
    public string itemName;
    public int baseSuccessChance;
    public ResourceType primaryResourceCost;
    public int costAmount;
    public float synergyComboBoost;
    public float radiantBonus;
    public bool removeCorruptedGems;
    public int realmTierRequired;
    public string cameoTriggerID;
    public bool isLegendary;
}
