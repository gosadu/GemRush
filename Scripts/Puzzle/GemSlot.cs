using UnityEngine;

/// <summary>
/// Each cell in the puzzle grid holds a Gem. 
/// Position references (x,y) for indexing in PuzzleBoardManager.
/// </summary>
public class GemSlot
{
    public Vector2Int position;
    public Gem gem;   // reference to the actual Gem script
}
