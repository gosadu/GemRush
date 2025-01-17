using UnityEngine;

/// <summary>
/// Simple GameManager that references the EnhancedBoardManager and calls Init.
/// This matches the logs shown (GameManager:InitializeAll).
/// </summary>
public class GameManager : MonoBehaviour
{
    public EnhancedBoardManager boardManager;

    void Start()
    {
        // Typically we initialize all systems here
        InitializeAll();
    }

    public void InitializeAll()
    {
        // Example: call boardManager.InitBoard
        if (boardManager)
        {
            boardManager.InitBoard();
        }
    }
}
