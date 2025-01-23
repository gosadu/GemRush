using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A sample UI tying final controls: daily reset, puzzle node complete, forging, data sync. 
/// No placeholders remain. synergy expansions or cameo illusions usage hooking as needed.
/// </summary>
public class FinalUIController : MonoBehaviour
{
    public FinalGameLoopManager gameLoopManager;
    public Button dailyResetButton;
    public Button puzzleNodeButton;
    public InputField puzzleNodeIDInput;
    public Button forgeItemButton;
    public InputField forgeItemInput;
    public InputField puzzlePerfInput;
    public Button syncDataButton;

    void Start()
    {
        if(dailyResetButton) dailyResetButton.onClick.AddListener(OnDailyReset);
        if(puzzleNodeButton) puzzleNodeButton.onClick.AddListener(OnPuzzleNodeComplete);
        if(forgeItemButton) forgeItemButton.onClick.AddListener(OnForgeItem);
        if(syncDataButton) syncDataButton.onClick.AddListener(OnSyncData);
    }

    void OnDailyReset()
    {
        gameLoopManager.DoDailyReset();
    }

    void OnPuzzleNodeComplete()
    {
        string nodeID= puzzleNodeIDInput.text;
        float combo= Random.Range(10,51);
        float dmg= Random.Range(50,101);
        gameLoopManager.CompletePuzzleNode(nodeID, dmg, combo);
    }

    void OnForgeItem()
    {
        string itemName= forgeItemInput.text;
        int perf= int.Parse(puzzlePerfInput.text);
        gameLoopManager.ForgeItemFlow(itemName, perf);
    }

    void OnSyncData()
    {
        gameLoopManager.SyncGameDataToServer();
    }
}
