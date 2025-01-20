using UnityEngine;

/// <summary>
/// Basic data structure to represent a single node in a sublocation.
/// Nodes can be combat, gathering, event, mini-puzzle, or boss types.
/// </summary>
[System.Serializable]
public class NodeData
{
    public enum NodeType
    {
        Combat,
        Gathering,
        Event,
        MiniPuzzle,
        Boss
    }

    public NodeType nodeType;
    public bool isCleared;
    public string displayName;

    // Optional fields for item requirements or currency rewards
    public int requiredItemID;
    public int rewardGold;
    public int rewardGems;
}
