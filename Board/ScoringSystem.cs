using UnityEngine;

/// <summary>
/// Basic scoring logic to reward matches, combos, etc.
/// </summary>
[System.Serializable]
public class ScoringSystem
{
    public int currentScore = 0;
    public int comboChain = 0; // how many cascades in a row
    public int movesCount = 0;

    private int comboMultiplier = 1;

    /// <summary>
    /// Called whenever the player makes a valid move.
    /// </summary>
    public void RecordMove()
    {
        movesCount++;
    }

    /// <summary>
    /// Increase score based on the number of matched tiles.
    /// If there's an active combo chain, we add a multiplier.
    /// </summary>
    public void AddMatchScore(int matchedCount)
    {
        int basePoints = matchedCount * 10;
        int total = basePoints * comboMultiplier;
        currentScore += total;
        Debug.Log($"[ScoringSystem] +{total} points (combo x{comboMultiplier}), total score: {currentScore}");
    }

    public void IncrementComboChain()
    {
        comboChain++;
        comboMultiplier = 1 + comboChain; // e.g., first chain=2x, second=3x
        Debug.Log("[ScoringSystem] Combo chain is now: " + comboChain + " multiplier: x" + comboMultiplier);
    }

    public void ResetComboChain()
    {
        comboChain = 0;
        comboMultiplier = 1;
    }
}
