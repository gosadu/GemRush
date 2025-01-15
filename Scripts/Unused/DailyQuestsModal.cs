using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[System.Serializable]
public class QuestData
{
    public string description;
    public int current;
    public int goal;

    public QuestData(string desc, int cur, int gl)
    {
        description = desc;
        current = cur;
        goal = gl;
    }
}

public class DailyQuestsModal : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text questsText;
    [SerializeField] private Button closeButton;

    public void Init(List<QuestData> quests, Action onClose)
    {
        if (titleText != null) titleText.text = "Daily Quests";
        if (questsText != null)
        {
            questsText.text = "";
            foreach (var q in quests)
            {
                questsText.text += q.description + " — " + q.current + "/" + q.goal + "\n";
            }
        }
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => {
                onClose?.Invoke();
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
