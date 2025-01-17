// FILE: ItemManager.cs
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public void UseHealingPotion(int amount)
    {
        EnhancedBoardManager board = 
            Object.FindAnyObjectByType<EnhancedBoardManager>();
        if (board)
        {
            board.HealPlayer(amount);
        }
    }

    public void UseAggregatorItem(int points)
    {
        EnhancedBoardManager board = 
            Object.FindAnyObjectByType<EnhancedBoardManager>();
        if (board)
        {
            board.AddAggregatorPoints(points);
        }
    }
}
