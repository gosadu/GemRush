using UnityEngine;

/// <summary>
/// Simple minion info. Minimal logs: none, unless you need error checks inside properties.
/// </summary>
[CreateAssetMenu(fileName = "MinionData", menuName = "WorldMap/MinionData")]
public class MinionData : ScriptableObject
{
    public string minionName;
    public int hp;
    public int attack;
    public Sprite minionSprite;
}
