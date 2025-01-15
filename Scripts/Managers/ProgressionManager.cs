using UnityEngine;

/// <summary>
/// Minimal logs: only warnings if needed. No big debug spam.
/// </summary>
public class ProgressionManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentScore = 0;

    public void AddScore(int points)
    {
        currentScore += points;
    }

    public void NextLevel()
    {
        currentLevel++;
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
    }
}
