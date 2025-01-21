using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text movesText;

    public void UpdateMoves(int moves)
    {
        if (movesText) movesText.text = "Moves: " + moves;
    }
}
