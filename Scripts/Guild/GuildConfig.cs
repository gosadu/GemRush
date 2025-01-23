using UnityEngine;

[CreateAssetMenu(fileName="GuildConfig", menuName="PuzzleRPG/GuildConfig")]
public class GuildConfig : ScriptableObject
{
    public int maxMembers=30;
    public int dailyContributionLimit=50;
    public int dailyResourceReceiveLimit=100;
    public int guildUpgradeCost=500;
    public float synergyBoostPerUpgrade=0.05f;
}
