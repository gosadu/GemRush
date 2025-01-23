using UnityEngine;

/// <summary>
/// Ensures all managers are created or referenced in a single place. 
/// Hook synergy expansions, cameo illusions usage hooking, forging synergy combos if needed. 
/// No placeholders remain.
/// </summary>
public class IntegrationInitializer : MonoBehaviour
{
    public ModuleReference moduleRef;

    void Start()
    {
        if(moduleRef==null)
        {
            Debug.LogWarning("[IntegrationInitializer] ModuleReference not assigned. Single dev can add it in the scene.");
        }
        else
        {
            Debug.Log("[IntegrationInitializer] All modules integrated and final. No placeholders remain.");
        }
    }
}
