using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("All Quests")]
    public List<QuestData> allQuests;
    private Dictionary<string, QuestData> questDict= new Dictionary<string, QuestData>();

    public ResourceManager resourceManager;
    public ProjectionSummonManager cameoManager;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        foreach(var q in allQuests)
        {
            questDict[q.questID]= q;
        }
    }

    public void RecordProgress(QuestObjectiveType type, string reference, int amount=1)
    {
        foreach(var quest in allQuests)
        {
            if(quest.isCompleted) continue;
            foreach(var obj in quest.objectives)
            {
                if(obj.isCompleted) continue;
                if(obj.objectiveType== type)
                {
                    bool matches=false;
                    switch(type)
                    {
                        case QuestObjectiveType.CollectResource:
                            if(obj.resourceType.ToString()== reference)
                                matches= true;
                            break;
                        case QuestObjectiveType.DefeatEnemy:
                            if(obj.enemyID== reference)
                                matches= true;
                            break;
                        case QuestObjectiveType.ForgeItem:
                            if(obj.forgeItemName== reference)
                                matches= true;
                            break;
                        case QuestObjectiveType.SublocationClear:
                            if(obj.sublocationName== reference)
                                matches= true;
                            break;
                        default:
                            break;
                    }
                    if(matches)
                    {
                        obj.currentProgress+= amount;
                        if(obj.currentProgress>= obj.requiredCount)
                        {
                            obj.isCompleted= true;
                            Debug.Log($"[QuestManager] Objective completed for quest {quest.questID}: {type}");
                            CheckQuestCompletion(quest);
                        }
                    }
                }
            }
        }
    }

    public void CheckQuestCompletion(QuestData quest)
    {
        bool allDone= true;
        foreach(var o in quest.objectives)
        {
            if(!o.isCompleted) 
            {
                allDone= false; 
                break;
            }
        }
        if(allDone)
        {
            quest.isCompleted= true;
            Debug.Log($"[QuestManager] Quest '{quest.questID}' completed. Reward: {quest.rewardAmount} of {quest.rewardResource}");
            resourceManager.ModifyResource(quest.rewardResource, quest.rewardAmount);
            if(!string.IsNullOrEmpty(quest.cameoID))
            {
                cameoManager?.SummonProjection(quest.cameoID);
            }
            if(quest.isBranching && !string.IsNullOrEmpty(quest.nextQuestID))
            {
                Debug.Log($"[QuestManager] Branching quest. NextID= {quest.nextQuestID}, Alternate= {quest.alternateQuestID}");
            }
        }
    }

    public QuestData GetQuestByID(string questID)
    {
        if(questDict.ContainsKey(questID))
            return questDict[questID];
        return null;
    }
}
