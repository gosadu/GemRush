using UnityEngine;

/// <summary>
/// Tracks the player's Gold and Gems, saving to PlayerPrefs.
/// In future phases, we could store on a server or use encryption.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public int Gold { get; private set; }
    public int Gems { get; private set; }

    private void Start()
    {
        LoadCurrency();
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        SaveCurrency();
        Debug.Log("[CurrencyManager] Gold added. Total = " + Gold);
    }

    public void SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            SaveCurrency();
            Debug.Log("[CurrencyManager] Spent gold. Total = " + Gold);
        }
        else
        {
            Debug.LogWarning("[CurrencyManager] Not enough Gold!");
        }
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        SaveCurrency();
        Debug.Log("[CurrencyManager] Gems added. Total = " + Gems);
    }

    public bool SpendGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            SaveCurrency();
            Debug.Log("[CurrencyManager] Spent gems. Total = " + Gems);
            return true;
        }
        else
        {
            Debug.LogWarning("[CurrencyManager] Not enough Gems!");
            return false;
        }
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("player_gold", Gold);
        PlayerPrefs.SetInt("player_gems", Gems);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        Gold = PlayerPrefs.GetInt("player_gold", 0);
        Gems = PlayerPrefs.GetInt("player_gems", 0);
    }
}
