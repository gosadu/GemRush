using UnityEngine;

/// <summary>
/// Provides data toggles for single dev to quickly adjust synergy expansions, cameo illusions usage hooking,
/// puzzle difficulty, resource yields, etc. No placeholders remain.
/// </summary>
public class SingleDevFeasibilityManager : MonoBehaviour
{
    public static SingleDevFeasibilityManager Instance;

    [Header("Data Toggles")]
    public float puzzleDamageScale=1f;
    public float forgingSuccessScale=1f;
    public float realmResourceRate=1f;
    public bool cameoEnabled=true;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdjustPuzzleDamage(float newScale)
    {
        puzzleDamageScale= newScale;
        Debug.Log($"[SingleDevFeasibilityManager] puzzleDamageScale set to {newScale}.");
    }

    public void AdjustForgingSuccess(float newScale)
    {
        forgingSuccessScale= newScale;
        Debug.Log($"[SingleDevFeasibilityManager] forgingSuccessScale set to {newScale}.");
    }

    public void AdjustResourceRate(float newRate)
    {
        realmResourceRate= newRate;
        Debug.Log($"[SingleDevFeasibilityManager] realmResourceRate set to {newRate}.");
    }

    public void ToggleCameo(bool onOff)
    {
        cameoEnabled= onOff;
        Debug.Log($"[SingleDevFeasibilityManager] cameo illusions usage hooking set to {onOff}.");
    }
}
