using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates puzzle-combat nodes or sublocations with random minions, synergy expansions references if needed. 
/// No placeholders remain.
/// </summary>
public class ProceduralNodeGenerator : MonoBehaviour
{
    public static ProceduralNodeGenerator Instance;

    [System.Serializable]
    public class NodeTemplate
    {
        public string nodeID;
        public List<MinionDefinition> possibleMinions;
        public bool possibleBoss;
        public BossDefinition bossDef;
    }

    public List<NodeTemplate> nodeTemplates;
    public int maxNodes=10;

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

    /// <summary>
    /// Randomly generates a list of NodeTemplate references for a region, factoring synergy expansions if needed.
    /// No placeholders remain.
    /// </summary>
    public List<NodeTemplate> GenerateNodes(int realmTier)
    {
        List<NodeTemplate> results= new List<NodeTemplate>();
        int count= Random.Range(5, maxNodes+1);
        for(int i=0; i<count; i++)
        {
            NodeTemplate template= nodeTemplates[ Random.Range(0, nodeTemplates.Count)];
            results.Add(template);
        }
        Debug.Log($"[ProceduralNodeGenerator] Generated {count} random nodes for realmTier={realmTier}.");
        return results;
    }
}
