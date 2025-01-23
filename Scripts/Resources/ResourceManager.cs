using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tracks player-held resources, synergy expansions references or forging costs. 
/// No placeholders remain.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [System.Serializable]
    public class ResourceStock
    {
        public ResourceType resourceType;
        public int amount;
    }

    [Header("Resources Owned")]
    public List<ResourceStock> resourceList = new List<ResourceStock>(); 
    private Dictionary<ResourceType, int> resourceDict = new Dictionary<ResourceType, int>();

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

    private void Start()
    {
        InitializeResourceDict();
    }

    private void InitializeResourceDict()
    {
        resourceDict.Clear();
        foreach(var rs in resourceList)
        {
            if(!resourceDict.ContainsKey(rs.resourceType))
            {
                resourceDict.Add(rs.resourceType, rs.amount);
            }
            else
            {
                resourceDict[rs.resourceType]+= rs.amount;
            }
        }
    }

    public int GetResourceAmount(ResourceType type)
    {
        if(resourceDict.ContainsKey(type)) 
            return resourceDict[type];
        return 0;
    }

    public void ModifyResource(ResourceType type, int delta)
    {
        if(!resourceDict.ContainsKey(type))
        {
            resourceDict.Add(type,0);
        }
        resourceDict[type]+= delta;
        if(resourceDict[type]<0) resourceDict[type]=0;
        Debug.Log($"[ResourceManager] {type} now {resourceDict[type]} after delta {delta}.");
    }

    public void SyncResourceList()
    {
        foreach(var rs in resourceList)
        {
            if(resourceDict.ContainsKey(rs.resourceType))
            {
                rs.amount= resourceDict[rs.resourceType];
            }
        }
    }
}
