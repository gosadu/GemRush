using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ShopModal : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text currencyText;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Button closeButton;

    private Action<ShopItem> onBuyItem;
    private Action onClose;

    public void Init(int currency, List<ShopItem> shopItems, Action<ShopItem> onBuy, Action onCloseAction)
    {
        onBuyItem = onBuy;
        onClose = onCloseAction;

        if (titleText != null) titleText.text = "Item Shop";
        if (currencyText != null) currencyText.text = "Your Currency: " + currency;

        // Clear old items
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new item buttons
        foreach (var item in shopItems)
        {
            var buttonGO = Instantiate(itemButtonPrefab, itemsContainer);
            var textComp = buttonGO.GetComponentInChildren<Text>();
            if (textComp != null)
            {
                textComp.text = item.name + " — Cost: " + item.cost;
            }
            var btn = buttonGO.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                onBuyItem?.Invoke(item);
            });
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
