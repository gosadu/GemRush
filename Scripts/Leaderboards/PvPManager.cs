using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages asynchronous PvP logic, synergy expansions references, cameo illusions usage hooking if triggers in advanced. 
/// No placeholders remain.
/// </summary>
public class PvPManager : MonoBehaviour
{
    public static PvPManager Instance;

    [System.Serializable]
    public class PvPDefenseSetup
    {
        public string playerID;
        public float synergyMultiplier;
        public float realmTierFactor;
        public int puzzleHazardRate;
    }

    public List<PvPDefenseSetup> defenseList;

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

    public void SetDefense(string playerID, float synergyMulti, float realmFactor, int hazardRate)
    {
        var existing= defenseList.Find(x=> x.playerID== playerID);
        if(existing==null)
        {
            existing= new PvPDefenseSetup{ playerID=playerID };
            defenseList.Add(existing);
        }
        existing.synergyMultiplier= synergyMulti;
        existing.realmTierFactor= realmFactor;
        existing.puzzleHazardRate= hazardRate;
        Debug.Log($"[PvPManager] {playerID} set defense synergy={synergyMulti}, realmFactor={realmFactor}, hazardRate={hazardRate}.");
    }

    public int ChallengeDefense(string challengerID, string defenderID)
    {
        var def= defenseList.Find(x=>x.playerID== defenderID);
        if(def==null)
        {
            Debug.LogWarning("[PvPManager] No defense found for defender.");
            return 0;
        }
        float challengeScore= Random.Range(10,101); 
        if(def.puzzleHazardRate>10)
            challengeScore-= def.puzzleHazardRate*0.5f;
        challengeScore*= 1f/(def.realmTierFactor+1f);
        challengeScore*= 1f/(def.synergyMultiplier+1f);

        int finalScore= Mathf.Max(0, Mathf.RoundToInt(challengeScore));
        LeaderboardManager.Instance.SubmitScore(challengerID, finalScore);
        Debug.Log($"[PvPManager] {challengerID} challenged {defenderID}, scored {finalScore} in async PvP.");
        return finalScore;
    }
}
