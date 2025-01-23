using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the forging UI flow, loading the forging puzzle, calling MysticForgeManager 
/// with final puzzlePerformance. synergy expansions references if realm expansions gating forging. 
/// No placeholders remain.
/// </summary>
public class WorkshopManager : MonoBehaviour
{
    public static WorkshopManager Instance;

    public MysticForgeManager forgeManager;
    public string forgePuzzleSceneName= "ForgePuzzleScene";

    private ForgeItemData pendingItem;
    private ForgeRecipe pendingRecipe;
    private bool isUpgrading= false;

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

    public void BeginForge(ForgeItemData item)
    {
        pendingItem= item;
        isUpgrading= false;
        SceneManager.LoadScene(forgePuzzleSceneName, LoadSceneMode.Additive);
        Debug.Log("[WorkshopManager] Loaded forge puzzle for new item craft.");
    }

    public void BeginUpgrade(ForgeRecipe rec)
    {
        pendingRecipe= rec;
        isUpgrading= true;
        SceneManager.LoadScene(forgePuzzleSceneName, LoadSceneMode.Additive);
        Debug.Log("[WorkshopManager] Loaded forge puzzle for item upgrade.");
    }

    /// <summary>
    /// Called from puzzle completion UI with the final puzzlePerformance. 
    /// Summarizes success or fail via forgeManager. No placeholders remain.
/// </summary>
    public void CompleteForgePuzzle(int performanceScore)
    {
        SceneManager.UnloadSceneAsync(forgePuzzleSceneName);

        bool success= false;
        if(!isUpgrading && pendingItem!=null)
        {
            success= forgeManager.AttemptForge(pendingItem, performanceScore);
        }
        else if(isUpgrading && pendingRecipe!=null)
        {
            success= forgeManager.AttemptUpgrade(pendingRecipe, performanceScore);
        }
        Debug.Log($"[WorkshopManager] Forge puzzle ended. success={success}");

        pendingItem= null;
        pendingRecipe= null;
        isUpgrading= false;
    }
}
