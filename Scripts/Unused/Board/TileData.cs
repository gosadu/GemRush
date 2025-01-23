using UnityEngine;

/// <summary>
/// Represents all data for a tile: row, col, tileType, locked, sticky, etc.
/// This is purely logical (no visuals).
/// </summary>
[System.Serializable]
public class TileData
{
    public int row;
    public int col;
    public int tileType;   // e.g. color or gem type
    public bool isLocked;  // can't be moved/swapped
    public bool isSticky;  // won't fall during cascades

    public TileData(int row, int col, int tileType, bool locked = false, bool sticky = false)
    {
        this.row = row;
        this.col = col;
        this.tileType = tileType;
        this.isLocked = locked;
        this.isSticky = sticky;
    }
}
