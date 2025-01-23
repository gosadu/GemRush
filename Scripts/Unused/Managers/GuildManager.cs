using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class GuildMemberData
{
    public string memberID;
    public int dailyResourcesSent;
    public int dailyResourcesReceived;
    public int totalContribution;
}

public class GuildManager : MonoBehaviour
{
    [SerializeField] private int guildBossHP = 5000;
    [SerializeField] private Dictionary<string, GuildMemberData> guildMembers 
        = new Dictionary<string, GuildMemberData>();
    private const int DAILY_SEND_LIMIT = 50;
    private const int DAILY_RECEIVE_LIMIT = 100;
    private const string GUILD_BOSS_HP_KEY = "GUILD_BOSS_HP";

    public void InitializeGuild()
    {
        LoadGuildBossHP();
        Debug.Log("[GuildManager] Guild system initialized with final asynchronous logic.");
    }

    public bool SendResource(string senderID, int amount)
    {
        if (!guildMembers.ContainsKey(senderID)) AddNewMember(senderID);
        GuildMemberData member = guildMembers[senderID];

        if (member.dailyResourcesSent + amount > DAILY_SEND_LIMIT) return false;
        member.dailyResourcesSent += amount;
        member.totalContribution += amount;
        Debug.Log("[GuildManager] " + senderID + " sent " + amount + " resources to guild.");
        return true;
    }

    public bool ReceiveResource(string receiverID, int amount)
    {
        if (!guildMembers.ContainsKey(receiverID)) AddNewMember(receiverID);
        GuildMemberData member = guildMembers[receiverID];

        if (member.dailyResourcesReceived + amount > DAILY_RECEIVE_LIMIT) return false;
        member.dailyResourcesReceived += amount;
        Debug.Log("[GuildManager] " + receiverID + " received " + amount + " resources from guild.");
        return true;
    }

    public void FightGuildBoss(string memberID, int damage)
    {
        if (!guildMembers.ContainsKey(memberID)) AddNewMember(memberID);

        guildBossHP -= damage;
        if (guildBossHP < 0) guildBossHP = 0;
        SaveGuildBossHP();
        Debug.Log("[GuildManager] " + memberID + " inflicted " + damage
                  + " on Guild Boss. Remaining HP=" + guildBossHP);
    }

    private void LoadGuildBossHP()
    {
        guildBossHP = PlayerPrefs.GetInt(GUILD_BOSS_HP_KEY, 5000);
    }

    private void SaveGuildBossHP()
    {
        PlayerPrefs.SetInt(GUILD_BOSS_HP_KEY, guildBossHP);
        PlayerPrefs.Save();
    }

    private void AddNewMember(string id)
    {
        GuildMemberData newMember = new GuildMemberData();
        newMember.memberID = id;
        newMember.dailyResourcesSent = 0;
        newMember.dailyResourcesReceived = 0;
        newMember.totalContribution = 0;
        guildMembers[id] = newMember;
    }

    public int GetGuildBossHP()
    {
        return guildBossHP;
    }
}