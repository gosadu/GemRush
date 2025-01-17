using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LocationData", menuName = "WorldMap/LocationData")]
public class LocationData : ScriptableObject
{
    public string locationName;
    public bool isLocked = true;
    public List<SublocationData> sublocations;
    public BossData bossData; // The final boss for this location

    public void LockLocation()
    {
        isLocked = true;
    }

    public void UnlockLocation()
    {
        isLocked = false;
    }
}
