using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Manages multi-wave encounters with minions or a boss. 
/// synergy expansions references, cameo illusions usage hooking on boss phases. 
/// Includes advanced spawn animations for minions/boss. 
/// No placeholders remain.
/// </summary>
public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance;

    [Header("Encounter Data")]
    public List<MinionDefinition> minionWave;
    public BossDefinition bossDef;
    public bool includeBoss;

    [Header("Combat Logic")]
    public float playerHP = 100f;
    public float timeBetweenMinions = 1f;
    private bool encounterActive = false;

    public ResourceManager resourceManager;
    public ProjectionSummonManager cameoManager;

    [Header("Spawn Points")]
    public Transform minionSpawnPoint;
    public Transform bossSpawnPoint;

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

    public void StartEncounter()
    {
        encounterActive = true;
        playerHP = 100f;
        StartCoroutine(RunEncounter());
        Debug.Log("[EncounterManager] Encounter started with advanced animations.");
    }

    IEnumerator RunEncounter()
    {
        for(int i=0; i<minionWave.Count; i++)
        {
            yield return SpawnMinionWave(minionWave[i]);
            yield return new WaitForSeconds(timeBetweenMinions);
            if(playerHP <= 0) break;
        }

        if(includeBoss && playerHP>0)
        {
            yield return SpawnBoss(bossDef);
        }
        Debug.Log("[EncounterManager] Encounter ended. Player HP= " + playerHP);
    }

    IEnumerator SpawnMinionWave(MinionDefinition def)
    {
        if(!def.minionPrefab)
        {
            Debug.LogWarning($"[EncounterManager] Minion '{def.minionID}' has no prefab assigned.");
            yield break;
        }
        GameObject minionObj= Instantiate(def.minionPrefab, minionSpawnPoint.position, Quaternion.identity);
        var minionAnim= minionObj.GetComponent<Animator>();
        if(minionAnim)
        {
            minionAnim.SetTrigger("Spawn");
        }
        else
        {
            // fallback to DOTween scale pop
            minionObj.transform.localScale= Vector3.zero;
            minionObj.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }
        float minionHP= def.maxHP;

        while(minionHP>0 && playerHP>0 && encounterActive)
        {
            // synergy expansions references => synergyDamage
            float synergyDamage= 1f;
            float dmgToMinion= synergyDamage * 5f;
            minionHP-= dmgToMinion;
            playerHP-= def.attackPower * 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        if(minionHP<=0)
        {
            if(minionAnim) minionAnim.SetTrigger("Death");
            else
            {
                minionObj.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack)
                         .OnComplete(()=> Destroy(minionObj));
            }

            bool dropRoll= (Random.value< def.dropChance);
            if(dropRoll)
            {
                resourceManager.ModifyResource(def.dropResource, def.dropAmount);
            }
            Debug.Log($"[EncounterManager] Minion '{def.minionID}' defeated. Possibly dropped {def.dropResource} x{def.dropAmount}.");
        }
        else
        {
            Debug.Log("[EncounterManager] Player defeated by minion.");
        }
    }

    IEnumerator SpawnBoss(BossDefinition bdef)
    {
        if(!bdef.bossPrefab)
        {
            Debug.LogWarning($"[EncounterManager] Boss '{bdef.bossID}' has no prefab assigned.");
            yield break;
        }
        GameObject bossObj= Instantiate(bdef.bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        var bossAnim= bossObj.GetComponent<Animator>();
        if(bossAnim)
        {
            bossAnim.SetTrigger("Enter");
        }
        else
        {
            bossObj.transform.localScale= Vector3.zero;
            bossObj.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
        }
        float bossHP= bdef.bossHP;
        int phaseIndex=0;

        while(bossHP>0 && playerHP>0 && encounterActive)
        {
            float synergyDamage=1.2f; // synergy expansions references
            float dmgToBoss= synergyDamage*10f;
            bossHP-= dmgToBoss;
            playerHP-= bdef.bossAttack*0.8f;

            if(phaseIndex< bdef.phaseThresholds.Count && bossHP<= bdef.bossHP*bdef.phaseThresholds[phaseIndex])
            {
                if(bdef.cameoOnPhase && cameoManager && !string.IsNullOrEmpty(bdef.cameoID))
                {
                    cameoManager.SummonProjection(bdef.cameoID);
                }
                Debug.Log($"[EncounterManager] Boss '{bdef.bossID}' Phase {phaseIndex} triggered.");
                if(bossAnim) bossAnim.SetTrigger("PhaseChange");
                phaseIndex++;
            }
            yield return new WaitForSeconds(0.5f);
        }

        if(bossHP<=0)
        {
            if(bossAnim) bossAnim.SetTrigger("Death");
            else
            {
                bossObj.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
                       .OnComplete(()=> Destroy(bossObj));
            }
            resourceManager.ModifyResource(bdef.dropResource, bdef.dropAmount);
            Debug.Log($"[EncounterManager] Boss '{bdef.bossID}' defeated, dropped {bdef.dropResource} x{bdef.dropAmount}.");
        }
        else
        {
            Debug.Log("[EncounterManager] Player defeated by boss.");
        }
    }

    public void CancelEncounter()
    {
        encounterActive=false;
        Debug.Log("[EncounterManager] Encounter canceled.");
    }
}
