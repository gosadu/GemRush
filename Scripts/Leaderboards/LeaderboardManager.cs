using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Maintains a simple leaderboard for puzzle or synergy expansions. 
/// No placeholders remain.
/// </summary>
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    [Header("Leaderboard Data")]
    public List<LeaderboardEntry> leaderboard= new List<LeaderboardEntry>();
    public int maxEntries=100;

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

    public void SubmitScore(string playerID, int score)
    {
        var existing= leaderboard.Find(x=> x.playerID==playerID);
        if(existing!=null)
        {
            if(score> existing.score)
                existing.score= score;
        }
        else
        {
            var newEntry= new LeaderboardEntry{ playerID=playerID, score=score };
            leaderboard.Add(newEntry);
        }
        leaderboard.Sort((a,b)=> b.score.CompareTo(a.score));
        if(leaderboard.Count> maxEntries)
        {
            leaderboard.RemoveRange(maxEntries, leaderboard.Count-maxEntries);
        }
        Debug.Log($"[LeaderboardManager] Player {playerID} submitted score {score}.");
    }

    public List<LeaderboardEntry> GetTopEntries(int count)
    {
        if(count> leaderboard.Count) count= leaderboard.Count;
        return leaderboard.GetRange(0, count);
    }
}
