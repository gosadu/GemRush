using UnityEngine;
using System;

public class ProgressionSystem : MonoBehaviour
{
    public event Action OnBossDefeated;
    public event Action OnMinionDefeated;

    public void Init()
    {
        // Could load PlayerPrefs data or from cloud
        Debug.Log("[ProgressionSystem] Initialized. Ready to track progress.");
    }

    public void TrackMinionDefeat()
    {
        Debug.Log("[ProgressionSystem] A minion was defeated!");
        OnMinionDefeated?.Invoke();
    }

    public void TrackBossDefeat()
    {
        Debug.Log("[ProgressionSystem] A boss was defeated!");
        OnBossDefeated?.Invoke();
    }
}
