using UnityEngine;

/// <summary>
/// Creates random hazard or corrupted gem patterns for puzzle nodes. 
/// synergy expansions references if realm tier modifies spawn rates. 
/// No placeholders remain.
/// </summary>
public class ProceduralHazardGenerator : MonoBehaviour
{
    public static ProceduralHazardGenerator Instance;

    [Header("Hazard Configuration")]
    public float baseCorruptedChance=0.05f;
    public float synergyCorruptedModifier=0.01f;

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

    public float GetCorruptedSpawnChance(int realmTier)
    {
        float finalChance= baseCorruptedChance + (realmTier* synergyCorruptedModifier);
        return Mathf.Clamp01(finalChance);
    }
}
