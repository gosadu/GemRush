using UnityEngine;

[CreateAssetMenu(fileName="TutorialStepData", menuName="PuzzleRPG/TutorialStepData")]
public class TutorialStepData : ScriptableObject
{
    public string stepKey;            
    public string displayText;        
    public bool requiresConfirmation;
    public bool triggersCameo;        
    public string cameoID;           
}
