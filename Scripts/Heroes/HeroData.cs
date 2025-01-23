using UnityEngine;

/// <summary>
/// Basic hero data for gacha or hero collection. synergy expansions references 
/// if certain heroes require a realm tier. cameo illusions usage hooking if cameoTriggerEnabled, etc.
/// </summary>
[CreateAssetMenu(fileName="HeroData", menuName="PuzzleRPG/HeroData")]
public class HeroData : ScriptableObject
{
    public string heroName;
    public int baseHP;
    public int baseAttack;
    public int rarity; // e.g. 1..4
    public bool cameoTriggerEnabled; // if cameo illusions usage hooking is relevant
    public float synergyMultiplier;  // puzzle synergy factor
}
