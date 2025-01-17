using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Text pauseTitle;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button questsButton;
    [SerializeField] private Button leaderboardsButton;
    [SerializeField] private Button shopButton;

    public void Init(
        Action onResume,
        Action onCancel,
        Action onShowQuests,
        Action onShowLeaderboards,
        Action onShowShop)
    {
        if (pauseTitle != null) pauseTitle.text = "Paused";

        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(() => onResume?.Invoke());
        }
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() => onCancel?.Invoke());
        }
        if (questsButton != null)
        {
            questsButton.onClick.RemoveAllListeners();
            questsButton.onClick.AddListener(() => onShowQuests?.Invoke());
        }
        if (leaderboardsButton != null)
        {
            leaderboardsButton.onClick.RemoveAllListeners();
            leaderboardsButton.onClick.AddListener(() => onShowLeaderboards?.Invoke());
        }
        if (shopButton != null)
        {
            shopButton.onClick.RemoveAllListeners();
            shopButton.onClick.AddListener(() => onShowShop?.Invoke());
        }
    }

    public void Show(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
