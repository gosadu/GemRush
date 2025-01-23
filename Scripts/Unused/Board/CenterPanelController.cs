/*************************************************************
 * CenterPanelController.cs
 * Attach this script to your "CenterPanel" GameObject.
 * Responsible for:
 *   - Hosting the gem board (EnhancedBoardManager).
 *   - Toggling the WildcardInfoPanel when "?" is clicked.
 * Design: arcane/fantasy styling, with references to ornate frames or
 * tattered parchment for the wildcard panel.
 *************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class CenterPanelController : MonoBehaviour
{
    [Header("Match-3 Board Container")]
    [SerializeField] private RectTransform gemBoardContainer;
    
    // The “?” button in the bottom-left corner
    [Header("Wildcard Info UI")]
    [SerializeField] private Button wildcardInfoButton;

    // The panel that appears with arcane info on wildcards
    [SerializeField] private GameObject wildcardInfoPanel;

    // If you want to reference your match-3 logic:
    [Header("Reference to Enhanced Board Manager")]
    [SerializeField] private EnhancedBoardManager boardManager;

    private void Awake()
    {
        // Hide the wildcard panel initially
        if (wildcardInfoPanel) 
            wildcardInfoPanel.SetActive(false);

        // Hook up the button click
        if (wildcardInfoButton)
            wildcardInfoButton.onClick.AddListener(OnWildcardInfoButtonClicked);
    }

    private void Start()
    {
        // Optionally place EnhancedBoardManager as a child of gemBoardContainer
        // if not already placed in the scene.
        if (boardManager && gemBoardContainer)
        {
            // This sets the BoardManager's transform as a child under gemBoardContainer
            // preserving local scale and position. 
            boardManager.transform.SetParent(gemBoardContainer, false);

            // Or if you want to call boardManager.InitBoard() here:
            // boardManager.InitBoard();
        }
    }

    // Toggles the wildcard info overlay on/off when “?” is clicked
    private void OnWildcardInfoButtonClicked()
    {
        if (!wildcardInfoPanel) return;
        bool isActive = wildcardInfoPanel.activeSelf;
        wildcardInfoPanel.SetActive(!isActive);
    }

    // Optional public method to show/hide the panel from other scripts:
    public void ShowWildcardInfoPanel(bool show)
    {
        if (wildcardInfoPanel)
        {
            wildcardInfoPanel.SetActive(show);
        }
    }
}
