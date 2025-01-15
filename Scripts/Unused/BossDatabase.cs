using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BossDatabase", menuName = "GameData/BossDatabase")]
public class BossDatabase : ScriptableObject
{
    [Header("All Bosses: 0..49 (or 1..50)")]
    public List<BossData> allBosses = new List<BossData>();

    /// <summary>
    /// Safe accessor for boss data. index is 0-based or 1-based if you like.
    /// </summary>
    public BossData GetBoss(int index)
    {
        // If user calls with 0-based, no shift needed. If they call 1-based, do index-1.
        // For example, let's do index as 0-based to keep it consistent:
        if (index < 0 || index >= allBosses.Count)
        {
            Debug.LogWarning("[BossDatabase] Invalid boss index " + index);
            return null;
        }
        return allBosses[index];
    }
}
