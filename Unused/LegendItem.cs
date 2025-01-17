using UnityEngine;
using UnityEngine.UI;
using System;

public class LegendItem : MonoBehaviour
{
    [SerializeField] private Image gemImage;
    [SerializeField] private Text titleText;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text infoTitle;
    [SerializeField] private Text infoDesc;
    private string desc;

    public void Init(Sprite gemSprite, string title, string description)
    {
        if (gemImage != null) gemImage.sprite = gemSprite;
        if (titleText != null) titleText.text = title;
        desc = description;
    }

    public void OnLongPress()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            if (infoTitle != null) infoTitle.text = titleText.text;
            if (infoDesc != null) infoDesc.text = desc;
        }
    }

    public void CloseInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }
}
