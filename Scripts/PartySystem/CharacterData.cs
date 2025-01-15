using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "PartySystem/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public string characterName;
    public Sprite portrait;
    [Range(1,5)] public int starRating = 1;

    [Header("Stats/Synergy")]
    public float synergyBonus = 0.0f;
    public int baseAttack = 10;
    public int baseDefense = 5;

    [Header("XP & Leveling")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    [Header("Skills/Spells")]
    public string[] skills;

    public void AddXP(int xpAmount)
    {
        currentXP += xpAmount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);

        synergyBonus += 0.05f;
        baseAttack += 2;
        baseDefense += 1;

        Debug.Log(characterName + " leveled up to " + currentLevel + "! synergy: " + synergyBonus.ToString("F2"));
    }
}
