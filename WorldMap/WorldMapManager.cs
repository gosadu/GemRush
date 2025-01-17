using UnityEngine;

/// <summary>
/// Minimal logs: only warnings if minionDatabase is missing or invalid.
/// Will not spam logs unless there's a real error with minions.
/// </summary>
public class WorldMapManager : MonoBehaviour
{
    public MinionDatabase minionDatabase;

    private void Start()
    {
        // no large logs
    }

    public void InitMap()
    {
        if (!minionDatabase)
        {
            Debug.LogError("[WorldMapManager] No minionDatabase assigned!");
            return;
        }

        // example logic
        if (minionDatabase.allMinions.Count > 0)
        {
            MinionData first = minionDatabase.GetMinion(0);
            if (first == null)
            {
                // The error is already logged in GetMinion if it's null
                return;
            }
            // No additional logs, to keep noise low
        }
        else
        {
            Debug.LogWarning("[WorldMapManager] minionDatabase has 0 minions in allMinions!");
        }
    }
}
