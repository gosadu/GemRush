using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SublocationData", menuName = "WorldMap/SublocationData")]
public class SublocationData : ScriptableObject
{
    public string sublocationName;
    public bool isLocked = true;
    public List<MinionData> minions; // up to 2-5 minions, for example

    public void LockSublocation()
    {
        isLocked = true;
    }

    public void UnlockSublocation()
    {
        isLocked = false;
    }
}
