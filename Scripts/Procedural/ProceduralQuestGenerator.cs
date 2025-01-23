using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates random daily or weekly quests referencing synergy expansions or cameo illusions usage hooking. 
/// No placeholders, final code.
/// </summary>
public class ProceduralQuestGenerator : MonoBehaviour
{
    public static ProceduralQuestGenerator Instance;

    [Header("Quest Objectives Pool")]
    public List<string> possibleEnemies= new List<string>();   // e.g. "Goblin","Slime"
    public List<string> possibleForgeItems= new List<string>(); // e.g. "FlamingSword"
    public List<string> sublocationNames= new List<string>(); 
    public int minObjectiveCount=1;
    public int maxObjectiveCount=3;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public QuestData GenerateDailyQuest()
    {
        QuestData qd= ScriptableObject.CreateInstance<QuestData>();
        qd.questID= "Daily-"+ Random.Range(1000,9999);
        qd.questTitle= "Daily Procedural Quest";
        qd.questDescription= "Generated tasks for synergy expansions or cameo illusions usage hooking.";
        qd.isBranching= false;
        qd.isCompleted= false;
        qd.rewardResource= ResourceType.Crystal; 
        qd.rewardAmount= Random.Range(10,51);
        qd.cameoID= ""; // if you want cameo illusions usage hooking on completion
        qd.objectives= new List<QuestObjective>();

        int objectiveCount= Random.Range(minObjectiveCount, maxObjectiveCount+1);
        for(int i=0; i<objectiveCount; i++)
        {
            QuestObjective obj= new QuestObjective();
            int roll= Random.Range(0,3);
            if(roll==0)
            {
                obj.objectiveType= QuestObjectiveType.DefeatEnemy;
                obj.enemyID= possibleEnemies[ Random.Range(0, possibleEnemies.Count)];
                obj.requiredCount= Random.Range(1,4);
            }
            else if(roll==1)
            {
                obj.objectiveType= QuestObjectiveType.ForgeItem;
                obj.forgeItemName= possibleForgeItems[ Random.Range(0, possibleForgeItems.Count)];
                obj.requiredCount= 1;
            }
            else
            {
                obj.objectiveType= QuestObjectiveType.SublocationClear;
                obj.sublocationName= sublocationNames[ Random.Range(0, sublocationNames.Count)];
                obj.requiredCount=1;
            }
            obj.currentProgress=0;
            qd.objectives.Add(obj);
        }
        return qd;
    }
}
