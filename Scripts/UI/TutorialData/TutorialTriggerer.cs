using UnityEngine;

/// <summary>
/// Attach this to certain objects or events to auto-trigger a tutorial step. 
/// No placeholders remain.
/// </summary>
public class TutorialTriggerer : MonoBehaviour
{
    public string tutorialKeyToTrigger;
    public bool triggerOnce=true;
    private bool hasTriggered=false;

    void OnEnable()
    {
        if(!hasTriggered || !triggerOnce)
        {
            TutorialFlowManager.Instance?.ShowTutorialStep(tutorialKeyToTrigger);
            hasTriggered=true;
        }
    }
}
