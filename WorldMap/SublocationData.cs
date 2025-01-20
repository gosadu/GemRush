using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SublocationData", menuName = "WorldMap/SublocationData")]
public class SublocationData : ScriptableObject
{
    public string sublocationName;
    public bool isLocked = true;
    public List<MinionData> minions;

    // NEW: Node list (combat, gathering, events, etc.)
    public List<NodeData> nodes = new List<NodeData>();

    public void LockSublocation()
    {
        isLocked = true;
    }

    public void UnlockSublocation()
    {
        isLocked = false;
    }
}
