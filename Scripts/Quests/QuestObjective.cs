using UnityEngine;

/// <summary>
/// Distinct objective types: collecting resources, defeating enemies, 
/// forging items, clearing sublocations, or reaching realm tiers. 
/// No placeholders remain.
/// </summary>
public enum QuestObjectiveType
{
    CollectResource,
    DefeatEnemy,
    ReachRealmTier,
    ForgeItem,
    SublocationClear
}

[System.Serializable]
public class QuestObjective
{
    public QuestObjectiveType objectiveType;
    public ResourceType resourceType;   
    public int resourceAmount;          
    public int realmIndex;              
    public int targetTier;              
    public string enemyID;             
    public string sublocationName;     
    public string forgeItemName;       
    public int currentProgress;
    public int requiredCount;
    public bool isCompleted;
}
