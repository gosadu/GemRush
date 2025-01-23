using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple MonoBehaviour to display a tile (Sprite, position).
/// In a production game, you'd add animations, lock icons, etc.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class TileView : MonoBehaviour
{
    public TileData tile;         // reference to the logical tile
    public Image tileImage;       // the sprite renderer (UI Image) for the tile

    private EnhancedBoardManager boardManager;

    /// <summary>
    /// Initialize the tile's visuals with a sprite, also store references.
    /// </summary>
    public void Init(TileData data, Sprite sprite, EnhancedBoardManager bm)
    {
        tile = data;
        boardManager = bm;

        if (tileImage && sprite)
        {
            tileImage.sprite = sprite;
        }
    }

    /// <summary>
    /// Example pointer-click or drag handle. You can wire up your input
    /// to call boardManager.TrySwap(...) with neighbors, etc.
    /// </summary>
    public void OnTileClicked()
    {
        // This is just a stub for demonstration.
        // E.g., store first clicked tile globally, then swap with second.
    }
}
