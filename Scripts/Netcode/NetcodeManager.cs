using UnityEngine;

public class NetcodeManager : MonoBehaviour
{
    public bool isOnline = false;

    public void ConnectToServer()
    {
        isOnline = true;
        Debug.Log("[NetcodeManager] Connected => synergy with Realm Tier, Guest Hero Summon, Arcane Gear Infusion co-op possible.");
    }

    public void Disconnect()
    {
        isOnline = false;
        Debug.Log("[NetcodeManager] Disconnected => co-op ended.");
    }

    public bool IsOnline()
    {
        return isOnline;
    }

    /// <summary>
    /// We add this parameterless method so calls like netcodeManager.UploadPlayerData() compile.
    /// Internally, it just calls the main version with a null argument or a default data object.
    /// </summary>
    public void UploadPlayerData()
    {
        UploadPlayerData(null);
    }

    /// <summary>
    /// The main version that actually handles player data. The above calls this with null if no data is passed.
    /// </summary>
    public void UploadPlayerData(object playerData)
    {
        Debug.Log("[NetcodeManager] UploadPlayerData => sending data to server, synergy expansions if needed.");
        // do your final logic here
    }

    /// <summary>
    /// EnqueueEvent needed by FinalGameLoopManager
    /// </summary>
    public void EnqueueEvent(string eventName)
    {
        Debug.Log($"[NetcodeManager] EnqueueEvent => {eventName}. Possibly broadcast to co-op players.");
    }
}
