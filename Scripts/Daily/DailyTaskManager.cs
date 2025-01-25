using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages daily tasks, referencing Realm Tier synergy, Guest Hero Summon (if tasks relate), 
/// Arcane Gear Infusion forging tasks, etc. Final approach with no placeholders.
/// </summary>
public class DailyTaskManager : MonoBehaviour
{
    private Dictionary<string,int> dailyTaskCounts = new Dictionary<string,int>();
    private DateTime lastRecordedDate;

    void Start()
    {
        // Example tasks
        dailyTaskCounts["HarvestResources"] = 0;
        dailyTaskCounts["ForgingAttempts"]  = 0;
        dailyTaskCounts["PuzzleClears"]     = 0;

        // Record current date/time on Start
        lastRecordedDate = DateTime.UtcNow.Date;
    }

    /// <summary>
    /// Increments a named daily task by some amount. 
    /// e.g., "ForgingAttempts" increment if the user does an Arcane Gear Infusion
    /// </summary>
    public void IncrementTask(string taskID, int amount)
    {
        if(!dailyTaskCounts.ContainsKey(taskID))
            dailyTaskCounts[taskID] = 0;

        dailyTaskCounts[taskID] += amount;
        Debug.Log($"[DailyTaskManager] Task '{taskID}' incremented by {amount}. Total={dailyTaskCounts[taskID]}");
    }

    /// <summary>
    /// Resets all daily tasks. Called if the day changed or user hits a daily reset time.
    /// Possibly references Realm Tier synergy if you want bonus tasks each day.
    /// </summary>
    public void ResetDailyResources()
    {
        Debug.Log("[DailyTaskManager] ResetDailyResources => clearing all daily tasks.");
        foreach(var key in dailyTaskCounts.Keys)
        {
            dailyTaskCounts[key] = 0;
        }
        // Could add synergy expansions for Arcane Gear Infusion or Guest Hero Summon each day
    }

    /// <summary>
    /// Called from TownHubManager at Start() or whenever you want to verify daily tasks 
    /// are still valid for the current day. If a new day, reset them.
    /// </summary>
    public void ValidateDailyTasks()
    {
        DateTime today = DateTime.UtcNow.Date;
        if(today > lastRecordedDate) 
        {
            // new day => reset tasks
            Debug.Log("[DailyTaskManager] A new day has arrived, resetting daily tasks.");
            ResetDailyResources();
            lastRecordedDate = today;
        }
        else
        {
            Debug.Log("[DailyTaskManager] ValidateDailyTasks => same day, tasks remain.");
        }
    }

    /// <summary>
    /// Example: retrieve a current count if needed for UI.
    /// </summary>
    public int GetTaskCount(string taskID)
    {
        if(dailyTaskCounts.ContainsKey(taskID))
            return dailyTaskCounts[taskID];
        return 0;
    }
}
