using UnityEngine;

/// <summary>
/// Defines each minion's stats for encounters: HP, attack, drop chance, synergy expansions references if needed.
/// If using advanced animations, minionPrefab references an Animator with spawn/idle/death states.
/// </summary>
[CreateAssetMenu(fileName="MinionDefinition", menuName="PuzzleRPG/MinionDefinition")]
public class MinionDefinition : ScriptableObject
{
    public string minionID;
    public float maxHP;
    public float attackPower;
    public float synergyResist;
    public ResourceType dropResource;
    public int dropAmount;
    [Range(0f,1f)]
    public float dropChance;
    public GameObject minionPrefab; // advanced animation reference
}
