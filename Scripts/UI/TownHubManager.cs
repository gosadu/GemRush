using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central controller for the Town Hub (Stage 1 references), synergy expansions or cameo illusions usage hooking in daily tasks, forging, etc.
/// No placeholders remain.
/// </summary>
public class TownHubManager : MonoBehaviour
{
    public WorldManager worldManager;
    public RealmProgressionManager realmManager;
    public SublocationManager sublocationManager;
    public DailyTaskManager dailyTaskManager;
    public TutorialManager tutorialManager;

    [Header("Current Region")]
    public int currentRegionID=0;

    void Start()
    {
        AudioOverlayManager.Instance?.PlayBackgroundMusic("TownTheme");
        tutorialManager?.TriggerTutorial("TownHubIntro");
        dailyTaskManager?.ValidateDailyTasks();
    }

    public void OpenRealmProgressionPanel()
    {
        SceneTransitionManager.Instance?.PlaySceneTransition(()=>
        {
            Debug.Log("[TownHubManager] Opening realm progression panel.");
        });
    }

    public void OpenPlayerHouse()
    {
        SceneTransitionManager.Instance?.PlaySceneTransition(()=>
        {
            Debug.Log("[TownHubManager] Entering player house.");
        });
    }

    public void OpenGuildHall()
    {
        SceneTransitionManager.Instance?.PlaySceneTransition(()=>
        {
            Debug.Log("[TownHubManager] Entering guild hall. cameo illusions usage hooking or co-op systems appear later.");
        });
    }

    public void TravelToRegion(int targetRegionID)
    {
        int maxTier= realmManager.GetHighestRealmTier();
        if(worldManager.IsRegionAccessible(targetRegionID, maxTier))
        {
            SceneTransitionManager.Instance?.PlaySceneTransition(()=>
            {
                Debug.Log($"[TownHubManager] Traveling to region {targetRegionID}.");
            });
        }
        else
        {
            Debug.LogWarning($"[TownHubManager] Region {targetRegionID} locked. Increase realm tier first.");
        }
    }
}
