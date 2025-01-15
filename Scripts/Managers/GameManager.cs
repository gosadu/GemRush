using UnityEngine;

/// <summary>
/// A central game manager that references the EnhancedBoardManager, calls InitBoard, etc.
/// Ensures all systems are initialized in the correct order.
/// Minimal logs: only warnings/errors or confirmations if references are missing.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("References")]
    public EnhancedBoardManager boardManager;
    public WorldMapManager worldMapManager;
    public ProgressionManager progressionManager;

    private void Awake()
    {
        // Confirm references in Inspector
        if (!boardManager)
            Debug.LogWarning("[GameManager] boardManager is not assigned!");
        if (!worldMapManager)
            Debug.LogWarning("[GameManager] worldMapManager is not assigned!");
        if (!progressionManager)
            Debug.LogWarning("[GameManager] progressionManager is not assigned!");
    }

    private void Start()
    {
        // Initialize
        InitializeAll();
    }

    private void InitializeAll()
    {
        // 1. World map
        if (worldMapManager)
        {
            worldMapManager.InitMap();
        }

        // 2. Board
        if (boardManager)
        {
            boardManager.InitBoard();
        }

        // 3. Progression
        if (progressionManager)
        {
            progressionManager.LoadProgress();
        }
    }

    public void StartNewGame()
    {
        if (boardManager)
        {
            boardManager.InitBoard();
        }
        else
        {
            Debug.LogWarning("[GameManager] boardManager is null in StartNewGame().");
        }
    }

    private void OnApplicationQuit()
    {
        // Save progress if possible
        if (progressionManager)
        {
            progressionManager.SaveProgress();
        }
    }
}
