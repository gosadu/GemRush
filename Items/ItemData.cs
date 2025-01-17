using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public EffectType effectType;
    public int effectValue;

    public enum EffectType
    {
        None,
        Heal,
        BoostAggregator,
        StarFragment
    }
}
