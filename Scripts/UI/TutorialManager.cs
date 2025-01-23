using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Legacy tutorial manager from Stage 1, now deferring to final TutorialFlowManager. 
/// No placeholders remain.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    private HashSet<string> completedKeys= new HashSet<string>();

    void Awake()
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

    public void TriggerTutorial(string key)
    {
        if(completedKeys.Contains(key)) return;
        completedKeys.Add(key);
        TutorialFlowManager.Instance?.ShowTutorialStep(key);
    }
}
