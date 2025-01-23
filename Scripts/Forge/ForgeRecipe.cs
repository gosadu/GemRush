using UnityEngine;

/// <summary>
/// Holds references for upgrading baseItem -> upgradedItem with extra cost, synergy expansions references in realmTier.
/// No placeholders remain.
/// </summary>
[CreateAssetMenu(fileName="ForgeRecipe", menuName="PuzzleRPG/ForgeRecipe")]
public class ForgeRecipe : ScriptableObject
{
    public ForgeItemData baseItem;
    public ForgeItemData upgradedItem;
    public int extraCost;
    public int requiredRealmTier;
}
