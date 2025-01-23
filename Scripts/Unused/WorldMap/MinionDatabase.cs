using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Main scriptable object for storing a list of minions.
/// Minimal logs, but error if we can't return a valid minion.
/// </summary>
[CreateAssetMenu(fileName = "MinionDatabase", menuName = "GameData/MinionDatabase")]
public class MinionDatabase : ScriptableObject
{
    public List<MinionData> allMinions;

    public MinionData GetMinion(int index)
    {
        if (index < 0 || index >= allMinions.Count)
        {
            Debug.LogError("[MinionDatabase] Invalid index " + index + ". allMinions.Count = " + allMinions.Count);
            return null;
        }

        MinionData data = allMinions[index];
        if (!data)
        {
            Debug.LogError("[MinionDatabase] allMinions[" + index + "] is null, check your references in the inspector!");
            return null;
        }

        return data;
    }
}
