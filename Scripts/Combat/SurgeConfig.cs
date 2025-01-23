using UnityEngine;

/// <summary>
/// Configurable data for high-combo surge: cameo illusions usage hooking triggers, removing hazards, etc.
/// No placeholders remain.
/// </summary>
[CreateAssetMenu(fileName="SurgeConfig", menuName="PuzzleRPG/SurgeConfig")]
public class SurgeConfig : ScriptableObject
{
    public float threshold=50f;
    public float duration=8f;
    public float damageBoost=1.2f;
    public bool removeCorruptedGems;
    public bool cameoTrigger;
    public string cameoID;
    public AudioClip surgeAudioClip;
}
