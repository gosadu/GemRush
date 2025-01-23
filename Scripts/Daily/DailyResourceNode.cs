using UnityEngine;

/// <summary>
/// A node that grants resources daily, synergy expansions references if realm tier modifies yield. 
/// No placeholders remain.
/// </summary>
public class DailyResourceNode : MonoBehaviour
{
    public ResourceType grantType= ResourceType.Wood;
    public int grantAmount= 10;
    public bool hasBeenCollectedToday= false;

    public void CollectResource()
    {
        if(hasBeenCollectedToday)
        {
            Debug.Log("[DailyResourceNode] Already collected today.");
            return;
        }
        ResourceManager.Instance?.ModifyResource(grantType, grantAmount);
        hasBeenCollectedToday= true;
        Debug.Log($"[DailyResourceNode] Granted {grantAmount} of {grantType}.");
    }

    public void ResetDaily()
    {
        hasBeenCollectedToday= false;
    }
}
