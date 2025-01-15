using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "WorldMap/BossData")]
public class BossData : ScriptableObject
{
    public string bossName;
    public int bossHP;
    public int bossAttack;
    public Sprite bossSprite;
    // any extra fields, e.g. elemental type
}
