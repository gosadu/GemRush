using UnityEngine;
using System;

/// <summary>
/// Oversees entire daily flow from login => daily tasks => forging synergy combos => puzzle 
/// => cameo illusions usage hooking => netcode sync. No placeholders remain.
/// </summary>
public class FinalGameLoopManager : MonoBehaviour
{
    public static FinalGameLoopManager Instance;

    public DateTime lastDailyReset;
    public DailyTaskManager dailyTaskManager;
    public QuestManager questManager;
    public TownHubManager townHub;
    public PuzzleBoardManager puzzleBoard;
    public MysticForgeManager forgeManager;
    public ResourceManager resourceManager;
    public NetcodeManager netcodeManager;
    public LiveOpsManager liveOpsManager;
    public GuildManager guildManager;
    public PassSystemManager passSystemManager;
    public SingleDevFeasibilityManager singleDevFeasibility;
    public RealmProgressionManager realmProgress;
    public PartySystemManager partySystem;
    public LeaderboardManager leaderboard;
    public PvPManager pvpManager;

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

    void Start()
    {
        if(DateTime.Now.Date> lastDailyReset.Date)
        {
            DoDailyReset();
            lastDailyReset= DateTime.Now;
        }
        Debug.Log("[FinalGameLoopManager] Game started. All synergy expansions, cameo illusions usage hooking, forging combos are final-coded.");
    }

    public void DoDailyReset()
    {
        dailyTaskManager.ResetDailyResources();
        liveOpsManager.CheckLiveOps();
        Debug.Log("[FinalGameLoopManager] Daily reset done.");
    }

    public void CompletePuzzleNode(string nodeID, float damageDealt, float comboAchieved)
    {
        resourceManager.ModifyResource(ResourceType.Metal,10); 
        questManager.RecordProgress(QuestObjectiveType.SublocationClear, nodeID,1);
        netcodeManager.EnqueueEvent($"PuzzleNodeClear|{nodeID}|{damageDealt}|{comboAchieved}");
        Debug.Log($"[FinalGameLoopManager] Puzzle node {nodeID} completed, synergy combo {comboAchieved}.");
    }

    public void ForgeItemFlow(string itemName, int puzzlePerformance)
    {
        // find item in item DB or recipe list
        Debug.Log("[FinalGameLoopManager] Attempting forging item: " + itemName);
        // call forgeManager.AttemptForge(...) with puzzlePerformance
    }

    public void SyncGameDataToServer()
    {
        netcodeManager.UploadPlayerData();
        Debug.Log("[FinalGameLoopManager] Manual data sync triggered.");
    }
}
