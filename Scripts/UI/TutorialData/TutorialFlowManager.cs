using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialFlowManager : MonoBehaviour
{
    public static TutorialFlowManager Instance;

    public List<TutorialStepData> tutorialSteps;
    private Dictionary<string, TutorialStepData> stepDict= new Dictionary<string, TutorialStepData>();

    [Header("UI References")]
    public GameObject tutorialPanel;
    public Text tutorialText;
    public Button confirmButton;

    public ProjectionSummonManager cameoManager; 
    public HashSet<string> completedSteps= new HashSet<string>();

    void Awake()
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

        foreach(var step in tutorialSteps)
        {
            stepDict[step.stepKey]= step;
        }
        if(tutorialPanel) tutorialPanel.SetActive(false);
        if(confirmButton) confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    public void ShowTutorialStep(string stepKey)
    {
        if(completedSteps.Contains(stepKey))
        {
            Debug.Log($"[TutorialFlowManager] Step '{stepKey}' already shown.");
            return;
        }
        if(!stepDict.ContainsKey(stepKey))
        {
            Debug.LogWarning($"[TutorialFlowManager] No data for stepKey={stepKey}.");
            return;
        }
        var step= stepDict[stepKey];
        completedSteps.Add(stepKey);

        if(tutorialPanel) tutorialPanel.SetActive(true);
        if(tutorialText) tutorialText.text= step.displayText;
        if(step.triggersCameo && !string.IsNullOrEmpty(step.cameoID))
        {
            cameoManager?.SummonProjection(step.cameoID);
        }
        if(!step.requiresConfirmation)
        {
            Invoke(nameof(CloseTutorialPanel), 2f);
        }
    }

    void OnConfirmClicked()
    {
        CloseTutorialPanel();
    }

    void CloseTutorialPanel()
    {
        if(tutorialPanel) tutorialPanel.SetActive(false);
    }
}
