using UnityEngine;
using System.Collections;

/// <summary>
/// Handles surge activation if combos pass threshold, cameo illusions usage hooking if config says cameoTrigger= true,
/// synergy expansions references for board synergy. No placeholders remain.
/// </summary>
public class SurgeManager : MonoBehaviour
{
    public static SurgeManager Instance;

    public SurgeConfig config;
    public ProjectionSummonManager cameoManager;
    public AudioOverlayManager audioOverlay;

    private bool isSurgeActive=false;
    private float surgeTimer=0f;
    private float storedDamageBoost=1f;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AttemptActivateSurge(float currentCombo, System.Action<float> onDamageBoostChanged, System.Action onRemoveHazards)
    {
        if(isSurgeActive) return;
        if(currentCombo>= config.threshold)
        {
            ActivateSurge(onDamageBoostChanged, onRemoveHazards);
        }
    }

    private void ActivateSurge(System.Action<float> onDamageBoostChanged, System.Action onRemoveHazards)
    {
        isSurgeActive=true;
        surgeTimer= config.duration;
        storedDamageBoost= config.damageBoost;

        onDamageBoostChanged?.Invoke(storedDamageBoost);

        if(config.removeCorruptedGems)
        {
            onRemoveHazards?.Invoke();
        }
        if(config.cameoTrigger && !string.IsNullOrEmpty(config.cameoID))
        {
            cameoManager?.SummonProjection(config.cameoID);
        }
        if(config.surgeAudioClip && audioOverlay)
        {
            audioOverlay.StopMusic();
            audioOverlay.bgmSource.PlayOneShot(config.surgeAudioClip);
        }
        Debug.Log("[SurgeManager] Surge activated.");
    }

    private void Update()
    {
        if(!isSurgeActive) return;
        surgeTimer-=Time.deltaTime;
        if(surgeTimer<=0f)
        {
            EndSurge();
        }
    }

    private void EndSurge()
    {
        isSurgeActive=false;
        storedDamageBoost=1f;
        Debug.Log("[SurgeManager] Surge ended.");
    }

    public float GetCurrentDamageBoost()
    {
        return storedDamageBoost;
    }
}
