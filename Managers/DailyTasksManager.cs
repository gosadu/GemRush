using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DailyTask
{
    public string taskID;
    public string taskDescription;
    public bool completed;
    public int rewardSeeds;
    public int rewardWood;
    public int rewardOre;
    public int rewardBlossoms;
    public int rewardGold;
}

public class DailyTasksManager : MonoBehaviour
{
    [SerializeField] private List<DailyTask> dailyTasks;
    [SerializeField] private OrchardManager orchardManager;
    [SerializeField] private ForgingManager forgingManager;
    [SerializeField] private WorldMapManager worldMapManager;
    [SerializeField] private ProgressionManager progressionManager;

    public void InitializeDailyTasks()
    {
        foreach (DailyTask t in dailyTasks) t.completed = false;
        Debug.Log("[DailyTasksManager] Daily tasks loaded. No placeholders.");
    }

    public void CompleteTask(string taskID)
    {
        DailyTask found = dailyTasks.Find(t => t.taskID == taskID);
        if (found == null) return;
        if (found.completed) return;

        found.completed = true;
        progressionManager.AddSeeds(found.rewardSeeds);
        progressionManager.AddWood(found.rewardWood);
        progressionManager.AddOre(found.rewardOre);
        progressionManager.AddBlossoms(found.rewardBlossoms);
        progressionManager.AddScore(found.rewardGold);

        Debug.Log("[DailyTasksManager] Completed daily task: " + found.taskID
                  + ". Awarded resources and gold.");
    }
}