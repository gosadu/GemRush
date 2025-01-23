using UnityEngine;

[CreateAssetMenu(fileName="ShopItemData", menuName="PuzzleRPG/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public int costPremium;
    public ResourceType grantedResource;
    public int grantedAmount;
    public bool isSkipToken;
    public int skipTokenCount;
    public bool isBattlePass;
    public float passDurationDays;
}
