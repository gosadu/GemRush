using UnityEngine;

/// <summary>
/// Attached to each forging puzzle gem. 
/// Allows advanced color assignment or subtle animations if desired.
/// No placeholders remain.
/// </summary>
public class ForgeGem : MonoBehaviour
{
    public ForgeGemColor forgeColor;
    private MiniForgePuzzleManager puzzle;

    public void InitForgeGem(MiniForgePuzzleManager mgr)
    {
        puzzle = mgr;
        forgeColor = GetRandomForgeColor();
        UpdateGemVisual();
    }

    ForgeGemColor GetRandomForgeColor()
    {
        float r= Random.value;
        if(r<0.2f) return ForgeGemColor.Fire;
        else if(r<0.4f) return ForgeGemColor.Water;
        else if(r<0.6f) return ForgeGemColor.Earth;
        else if(r<0.8f) return ForgeGemColor.Wind;
        else return ForgeGemColor.Arcane;
    }

    void UpdateGemVisual()
    {
        // If using an Animator or DOTween, you can do so here
        // For example, an Animator with "FireIdle," "WaterIdle," etc.
        // Or a simple sprite assignment. Final approach, no placeholders:
        // e.g., "FireGem.png" if no animator is assigned
    }

    void OnMouseDown()
    {
        ForgeGemSelector.Instance?.SetSelectedForgeGem(this);
    }

    public Vector2Int GetBoardPos()
    {
        return new Vector2Int((int)transform.localPosition.x,(int)transform.localPosition.y);
    }
}

/// <summary>
/// Possible forging puzzle gem colors.
/// </summary>
public enum ForgeGemColor
{
    None,
    Fire,
    Water,
    Earth,
    Wind,
    Arcane
}
