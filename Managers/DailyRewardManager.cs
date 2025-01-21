using UnityEngine;
using System;

/// <summary>
/// Minimal logs: warnings if not set up, or a quick message if daily not ready.
/// </summary>
public class DailyRewardManager : MonoBehaviour
{
    public ProgressionManager progressionManager;
    private const string LAST_REWARD_KEY = "LastDailyRewardTime";

    private void Start()
    {
        CheckDailyRewardAvailability();
    }

    public void CheckDailyRewardAvailability()
    {
        string lastClaimString = PlayerPrefs.GetString(LAST_REWARD_KEY, "");
        if (string.IsNullOrEmpty(lastClaimString))
        {
            // not claimed yet
        }
        else
        {
            DateTime lastClaim = DateTime.Parse(lastClaimString);
            DateTime now = DateTime.Now;

            if ((now - lastClaim).TotalHours < 24)
            {
                // not ready
            }
        }
    }

    public void ClaimDailyReward()
    {
        if (progressionManager)
        {
            progressionManager.AddScore(1000);
        }
        PlayerPrefs.SetString(LAST_REWARD_KEY, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }
}
