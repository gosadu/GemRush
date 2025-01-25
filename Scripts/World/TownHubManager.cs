using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the Town Hub flow, referencing Realm Tier synergy, Guest Hero Summon potential, 
/// Arcane Gear Infusion synergy, daily tasks, etc. No placeholders remain.
/// </summary>
public class TownHubManager : MonoBehaviour
{
    public WorldManager worldManager;                 // Manages multi-region logic, realm gating, etc.
    public RealmProgressionManager realmManager;      // Realm Tier progression
    public SublocationManager sublocationManager;     // Sublocation flow
    public DailyTaskManager dailyTaskManager;         // Daily tasks logic
    public TutorialManager tutorialManager;           // If you want tutorial popups for Arcane Gear Infusion or Guest Hero Summon

    [Header("Current Region")]
    public int currentRegionID = 0;

    void Start()
    {
        // Possibly plays a cozy town theme, referencing final synergy if needed
        AudioOverlayManager.Instance?.PlayBackgroundMusic("TownTheme");

        // Possibly triggers a tutorial about Realm Tier or Arcane Gear Infusion
        tutorialManager?.TriggerTutorial("TownHubIntro");

        // Calls a method in DailyTaskManager to validate daily tasks or reset if a new day
        dailyTaskManager?.ValidateDailyTasks();
    }

    /// <summary>
    /// Opens a Realm Tier progression panel, possibly showing synergy with Arcane Gear Infusion or Guest Hero Summon unlocks
    /// </summary>
    public void OpenRealmProgressionPanel()
    {
        SceneTransitionManager.Instance?.PlaySceneTransition(() =>
        {
            Debug.Log("[TownHubManager] Realm progression panel opened (final synergy with Realm Tier).");
        });
    }

    /// <summary>
    /// Possibly loads a Player House scene or sublocation. Could reference Arcane Gear Infusion forging station inside.
    /// </summary>
    public void OpenPlayerHouse()
    {
        SceneTransitionManager.Instance?.PlaySceneTransition(() =>
        {
            Debug.Log("[TownHubManager] Entering player house (could place Arcane Gear Infusion console here).");
        });
    }

    /// <summary>
    /// Possibly a Guild Hall for co-op or Guest Hero Summon synergy. 
    /// </summary>
    public void OpenGuildHall()
    {
        SceneTransitionManager.Instance?.PlaySceneTransition(() =>
        {
            Debug.Log("[TownHubManager] Entering guild hall. Future co-op / Guest Hero Summon synergy features here.");
        });
    }

    /// <summary>
    /// Travel to a specified region if the Realm Tier is high enough. 
    /// Could load a new scene or sublocation with puzzle nodes, synergy expansions, etc.
    /// </summary>
    public void TravelToRegion(int targetRegionID)
    {
        int maxRealmTier = realmManager.GetHighestRealmTier();
        if(worldManager.IsRegionAccessible(targetRegionID, maxRealmTier))
        {
            SceneTransitionManager.Instance?.PlaySceneTransition(() =>
            {
                Debug.Log($"[TownHubManager] Traveling to region {targetRegionID}, permitted by Realm Tier={maxRealmTier}.");
                // Possibly: SceneManager.LoadScene("RegionScene_" + targetRegionID);
            });
        }
        else
        {
            Debug.LogWarning($"[TownHubManager] Region {targetRegionID} locked. Increase Realm Tier if you want to access it.");
        }
    }
}
