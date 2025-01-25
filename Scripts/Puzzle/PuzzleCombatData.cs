using UnityEngine;

/// <summary>
/// Data holding puzzle-combat configurations. synergy expansions or cameo illusions usage hooking references
/// appear in code that calls this, but no placeholders remain here.
/// </summary>
[CreateAssetMenu(fileName="PuzzleCombatData", menuName="PuzzleRPG/PuzzleCombatData")]
public class PuzzleCombatData : ScriptableObject
{
    [Header("Combat Settings")]
    public bool useTimedMode = true;
    public float timeOrHP = 60f;
    public float baseDamageMultiplier = 1.0f;
    public float synergyBonusMultiplier = 0.2f;
    public float radiantBonus = 1.0f;
    public float surgeThreshold = 50f;
    public float surgeDamageBoost = 1.25f;

    [Header("Corrupted Gems")]
    public float corruptedSpawnChance = 0.05f;  
    public int maxCorruptedPhase = 3;
}
