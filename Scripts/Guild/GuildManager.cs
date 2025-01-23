using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles guild membership, daily donations, cameo illusions usage hooking if thresholds reached, synergy expansions references if needed.
/// No placeholders remain.
/// </summary>
public class GuildManager : MonoBehaviour
{
    public static GuildManager Instance;

    [Header("Guild Info")]
    public string guildName="DefaultGuild";
    public GuildConfig config;
    public List<GuildMemberData> members= new List<GuildMemberData>();
    public int guildLevel=1;
    public int totalResourcesContributed=0;

    public ResourceManager resourceManager;
    public ProjectionSummonManager cameoManager;

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

    public bool AddMember(string playerID)
    {
        if(members.Count>= config.maxMembers)
        {
            Debug.LogWarning("[GuildManager] Guild is full.");
            return false;
        }
        var mData= new GuildMemberData{ playerID=playerID, contributionPoints=0 };
        members.Add(mData);
        Debug.Log($"[GuildManager] {playerID} joined guild '{guildName}'.");
        return true;
    }

    public bool DonateResources(string playerID, ResourceType type, int amount)
    {
        var mem= members.Find(m=>m.playerID==playerID);
        if(mem==null)
        {
            Debug.LogWarning("[GuildManager] Player not in guild.");
            return false;
        }
        if(amount> config.dailyContributionLimit)
        {
            Debug.LogWarning("[GuildManager] Exceeds daily contribution limit.");
            return false;
        }
        int have= resourceManager.GetResourceAmount(type);
        if(have< amount)
        {
            Debug.LogWarning("[GuildManager] Not enough resources to donate.");
            return false;
        }
        resourceManager.ModifyResource(type, -amount);
        mem.contributionPoints+= amount;
        totalResourcesContributed+= amount;
        Debug.Log($"[GuildManager] {playerID} donated {amount} of {type} to guild '{guildName}'.");

        // cameo illusions usage hooking if thresholds
        if(totalResourcesContributed>=1000 && cameoManager!=null)
        {
            cameoManager.SummonProjection("GuildSpirit");
        }
        return true;
    }

    public bool UpgradeGuildLevel()
    {
        if(totalResourcesContributed< config.guildUpgradeCost)
        {
            Debug.LogWarning("[GuildManager] Not enough total contributions for guild upgrade.");
            return false;
        }
        guildLevel++;
        totalResourcesContributed-= config.guildUpgradeCost;
        Debug.Log($"[GuildManager] Guild '{guildName}' upgraded to level {guildLevel}.");
        return true;
    }
}
