using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Holds a single quest's data, synergy expansions references if cameo illusions usage hooking triggers upon completion. 
/// No placeholders remain.
/// </summary>
[CreateAssetMenu(fileName="QuestData", menuName="PuzzleRPG/QuestData")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questTitle;
    public string questDescription;
    public bool isBranching;
    public List<QuestObjective> objectives;  
    public string nextQuestID;         
    public string alternateQuestID;    
    public bool isCompleted;
    public ResourceType rewardResource;
    public int rewardAmount;
    public string cameoID; // cameo illusions usage hooking if user completes quest
}
