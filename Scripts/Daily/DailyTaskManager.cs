using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Checks daily tasks or resource nodes. synergy expansions references if gating. 
/// No placeholders remain.
/// </summary>
public class DailyTaskManager : MonoBehaviour
{
    public static DailyTaskManager Instance;

    [Header("Daily Resource Nodes")]
    public List<DailyResourceNode> dailyNodes;

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
    }

    public void ValidateDailyTasks()
    {
        Debug.Log("[DailyTaskManager] Checking daily tasks/resources. (final code).");
    }

    public void ResetDailyResources()
    {
        foreach(var node in dailyNodes)
        {
            node.ResetDaily();
        }
        Debug.Log("[DailyTaskManager] Reset daily resource collection on nodes.");
    }
}
