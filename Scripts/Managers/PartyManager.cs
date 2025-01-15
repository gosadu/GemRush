using UnityEngine;
using System.Collections.Generic;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private List<CharacterData> allCharacters; // All possible characters
    [SerializeField] private List<CharacterData> activeParty;   // Up to 4

    private const int MAX_PARTY_SIZE = 4;

    public void InitPartySystem()
    {
        Debug.Log("[PartyManager] Initializing Party System...");
        if (activeParty.Count == 0 && allCharacters.Count > 0)
        {
            AddToParty(allCharacters[0]);
        }
    }

    public bool AddToParty(CharacterData character)
    {
        if (activeParty.Count >= MAX_PARTY_SIZE)
        {
            Debug.LogWarning("Party is full! Can't add more than " + MAX_PARTY_SIZE);
            return false;
        }
        if (!activeParty.Contains(character))
        {
            activeParty.Add(character);
            Debug.Log("Added character: " + character.characterName);
            return true;
        }
        Debug.LogWarning("Character already in party: " + character.characterName);
        return false;
    }

    public bool RemoveFromParty(CharacterData character)
    {
        if (activeParty.Contains(character))
        {
            activeParty.Remove(character);
            Debug.Log("Removed character: " + character.characterName);
            return true;
        }
        return false;
    }

    public float GetPartySynergy()
    {
        float synergy = 1f;
        foreach (var c in activeParty)
        {
            synergy += c.synergyBonus;
        }
        return synergy;
    }

    public void AwardXPToParty(int xpAmount)
    {
        Debug.Log("Awarding " + xpAmount + " XP to each party member.");
        foreach (var c in activeParty)
        {
            c.AddXP(xpAmount);
        }
    }

    public List<CharacterData> GetAllCharacters() => allCharacters;
    public List<CharacterData> GetActiveParty() => activeParty;
}
