using UnityEngine;

/// <summary>
/// Similar to GemSelector, but for the forging puzzle. 
/// Swaps two ForgeGem positions, then triggers checks. No placeholders.
/// </summary>
public class ForgeGemSelector : MonoBehaviour
{
    public static ForgeGemSelector Instance;

    public MiniForgePuzzleManager puzzleManager;
    private ForgeGem selectedGem;

    void Awake()
    {
        if(Instance==null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetSelectedForgeGem(ForgeGem g)
    {
        if(selectedGem==null)
        {
            selectedGem = g;
        }
        else
        {
            if(selectedGem == g)
            {
                selectedGem = null;
                return;
            }
            Vector2Int posA= selectedGem.GetBoardPos();
            Vector2Int posB= g.GetBoardPos();
            puzzleManager.TrySwapForgeGems(posA, posB);
            selectedGem= null;
        }
    }
}
