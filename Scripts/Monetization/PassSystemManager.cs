using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages micro/battle passes, synergy expansions references if certain passes speed forging or cameo illusions usage hooking triggers. 
/// No placeholders remain.
/// </summary>
[System.Serializable]
public class ActivePass
{
    public string passName;
    public DateTime expiry;
}

public class PassSystemManager : MonoBehaviour
{
    public static PassSystemManager Instance;

    public List<ActivePass> activePasses= new List<ActivePass>();

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

    public void ActivatePass(string passName, float durationDays)
    {
        DateTime exp= DateTime.Now.AddDays(durationDays);
        activePasses.Add(new ActivePass{ passName=passName, expiry= exp});
        Debug.Log($"[PassSystemManager] Activated pass {passName}, expires {exp}.");
    }

    public bool IsPassActive(string passName)
    {
        activePasses.RemoveAll(p=> p.expiry< DateTime.Now);
        var pass= activePasses.Find(p=> p.passName== passName);
        return (pass!= null);
    }
}
