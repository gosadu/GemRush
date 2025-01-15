// FILE: ItemManager.cs
using UnityEngine;

/// <summary>
/// Demonstrates using EnhancedBoardManager's HealPlayer or AddAggregatorPoints
/// without obsolete calls. 
/// </summary>
public class ItemManager : MonoBehaviour
{
    public void UseHealingPotion(int amount)
    {
        // Instead of FindObjectOfType, we do:
        EnhancedBoardManager boardMgr = Object.FindAnyObjectByType<EnhancedBoardManager>();
        if (boardMgr)
        {
            boardMgr.HealPlayer(amount);
        }
    }

    public void UseAggregatorItem(int points)
    {
        // Instead of FindObjectOfType, we do:
        EnhancedBoardManager boardMgr = Object.FindFirstObjectByType<EnhancedBoardManager>();
        if (boardMgr)
        {
            boardMgr.AddAggregatorPoints(points);
        }
    }
}
