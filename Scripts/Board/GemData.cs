using UnityEngine;

[System.Serializable]
public class GemData
{
    public int row;
    public int col;
    public int colorIndex;
    public bool isSpecial;

    public GemData(int r, int c, int colorIndex, bool isSpecial)
    {
        row = r;
        col = c;
        this.colorIndex = colorIndex;
        this.isSpecial = isSpecial;
    }
}
