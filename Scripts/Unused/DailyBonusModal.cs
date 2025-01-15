using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyBonusModal : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button collectButton;

    public void Init(int streakCount, string streakMsg, Action onCollect)
    {
        if (titleText != null) titleText.text = "Daily Bonus!";
        if (!string.IsNullOrEmpty(streakMsg))
        {
            if (messageText != null)
                messageText.text = "Day " + streakCount + " in a row!\n" + streakMsg;
        }
        else
        {
            if (messageText != null)
                messageText.text = "Thanks for playing! Enjoy some currency.";
        }

        if (collectButton != null)
        {
            collectButton.onClick.RemoveAllListeners();
            collectButton.onClick.AddListener(() => {
                onCollect?.Invoke();
            });
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
