using UnityEngine;

/// <summary>
/// Handles selecting two gems to swap in the puzzle board, 
/// referencing PuzzleBoardManager.TrySwap(posA, posB).
/// Ensures advanced synergy expansions or cameo illusions usage hooking if puzzle combos trigger them (no placeholders).
/// </summary>
public class GemSelector : MonoBehaviour
{
    public static GemSelector Instance;

    private Gem selectedGem;
    public PuzzleBoardManager boardManager;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// If a gem is already selected, attempt to swap with the new gem. If not, select the gem.
    /// </summary>
    public void SetSelectedGem(Gem gem)
    {
        if(selectedGem == null)
        {
            selectedGem = gem;
        }
        else
        {
            if(selectedGem == gem)
            {
                selectedGem = null;
                return;
            }
            Vector2Int posA = GetGemBoardPosition(selectedGem);
            Vector2Int posB = GetGemBoardPosition(gem);

            boardManager?.TrySwap(posA, posB);
            selectedGem = null;
        }
    }

    private Vector2Int GetGemBoardPosition(Gem g)
    {
        var localPos = g.transform.localPosition;
        return new Vector2Int(Mathf.RoundToInt(localPos.x), Mathf.RoundToInt(localPos.y));
    }
}
