using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines a boss with HP, attack, multiple phases, cameo illusions usage hooking triggers, synergy expansions references if needed.
/// </summary>
[CreateAssetMenu(fileName="BossDefinition", menuName="PuzzleRPG/BossDefinition")]
public class BossDefinition : ScriptableObject
{
    public string bossID;
    public float bossHP;
    public float bossAttack;
    public List<float> phaseThresholds; // e.g. [0.75, 0.5, 0.25]
    public bool cameoOnPhase;         // cameo illusions usage hooking on phase triggers
    public string cameoID;            // cameo illusions usage hooking ally ID
    public ResourceType dropResource;
    public int dropAmount;
    public float synergyWeakness;     // e.g. 0.2 => +20% synergy damage
    public GameObject bossPrefab;     // references an Animator for advanced spawn/phase/death
}
