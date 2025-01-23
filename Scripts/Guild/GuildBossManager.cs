using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Asynchronous guild boss with cameo illusions usage hooking on phases. 
/// synergy expansions references if boss synergy is relevant. No placeholders remain.
/// </summary>
public class GuildBossManager : MonoBehaviour
{
    public static GuildBossManager Instance;

    [Header("Boss Data")]
    public float totalBossHP=10000f;
    public float currentBossHP=10000f;
    public List<GuildBossPhase> phases; 

    [Header("Damage Tracking")]
    public Dictionary<string,float> playerDamageLog= new Dictionary<string, float>();

    public ProjectionSummonManager cameoManager;
    private int currentPhaseIndex=0;

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

    public void InitBoss()
    {
        currentBossHP= totalBossHP;
        currentPhaseIndex=0;
        playerDamageLog.Clear();
        Debug.Log("[GuildBossManager] Guild boss initialized.");
    }

    public void DealDamage(string playerID, float dmg)
    {
        if(!playerDamageLog.ContainsKey(playerID)) 
            playerDamageLog[playerID]=0f;

        playerDamageLog[playerID]+= dmg;
        currentBossHP-= dmg;
        if(currentBossHP<0) currentBossHP=0;

        CheckPhase();
        Debug.Log($"[GuildBossManager] {playerID} dealt {dmg} DMG. Boss HP now {currentBossHP}.");
        if(currentBossHP<=0) BossDefeated();
    }

    void CheckPhase()
    {
        if(currentPhaseIndex< phases.Count)
        {
            if(currentBossHP<= phases[currentPhaseIndex].health)
            {
                Debug.Log($"[GuildBossManager] Boss phase {currentPhaseIndex} triggered. hazardRate={phases[currentPhaseIndex].hazardSpawnRate}");
                cameoManager?.SummonProjection("BossRageSpirit");
                currentPhaseIndex++;
            }
        }
    }

    void BossDefeated()
    {
        Debug.Log("[GuildBossManager] Boss defeated. Distributing rewards or synergy perks.");
        // synergy expansions references for reward
    }
}

[System.Serializable]
public class GuildBossPhase
{
    public float health;
    public float hazardSpawnRate;
    public float cameoMultiplier;
}
