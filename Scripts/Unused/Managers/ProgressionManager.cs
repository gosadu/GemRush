using UnityEngine;              // for MonoBehaviour
using System.Collections.Generic; // for List<>

public class ProgressionManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentScore = 0;

    private int seeds;
    private int wood;
    private int ore;
    private int blossoms;
    private int refinedPlanks;

    private List<ItemData> playerForgedItems = new List<ItemData>();

    private void Awake()
    {
        LoadProgress();
    }

    public void AddScore(int points) { currentScore += points; }
    public void NextLevel() { currentLevel++; }

    public int GetSeeds() { return seeds; }
    public int GetWood() { return wood; }
    public int GetOre() { return ore; }
    public int GetBlossoms() { return blossoms; }

    public void AddSeeds(int amount) { seeds += amount; }
    public void AddWood(int amount) { wood += amount; }
    public void AddOre(int amount) { ore += amount; }
    public void AddBlossoms(int amount) { blossoms += amount; }

    public void SpendSeeds(int amount) { seeds = Mathf.Max(seeds - amount, 0); }
    public void SpendWood(int amount) { wood = Mathf.Max(wood - amount, 0); }
    public void SpendOre(int amount) { ore = Mathf.Max(ore - amount, 0); }
    public void SpendBlossoms(int amount) { blossoms = Mathf.Max(blossoms - amount, 0); }

    public void AddRefinedPlanks(int amount) { refinedPlanks += amount; }
    public int GetRefinedPlanks() { return refinedPlanks; }

    public void AddForgedItem(ItemData item) { playerForgedItems.Add(item); }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.SetInt("Seeds", seeds);
        PlayerPrefs.SetInt("Wood", wood);
        PlayerPrefs.SetInt("Ore", ore);
        PlayerPrefs.SetInt("Blossoms", blossoms);
        PlayerPrefs.SetInt("RefinedPlanks", refinedPlanks);
        PlayerPrefs.SetInt("ForgedItemCount", playerForgedItems.Count);
        for(int i = 0; i < playerForgedItems.Count; i++)
        {
            PlayerPrefs.SetString("ForgedItem_" + i, playerForgedItems[i].itemName);
        }
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
        seeds = PlayerPrefs.GetInt("Seeds", 0);
        wood = PlayerPrefs.GetInt("Wood", 0);
        ore = PlayerPrefs.GetInt("Ore", 0);
        blossoms = PlayerPrefs.GetInt("Blossoms", 0);
        refinedPlanks = PlayerPrefs.GetInt("RefinedPlanks", 0);

        int itemCount = PlayerPrefs.GetInt("ForgedItemCount", 0);
        playerForgedItems.Clear();
        for(int i = 0; i < itemCount; i++)
        {
            string itemName = PlayerPrefs.GetString("ForgedItem_" + i, "");
            if(!string.IsNullOrEmpty(itemName))
            {
                // Possibly fetch actual scriptable objects from an ItemDatabase
            }
        }
    }
}
