/*************************************************************
 * BottomPanelController.cs
 * Attach this script to BottomPanel.
 * Manages Hero Portrait, Party Icons, Item Slots, etc.
 *************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BottomPanelController : MonoBehaviour
{
    [Header("Hero & Party UI")]
    [SerializeField] private Image heroPortrait;
    [SerializeField] private List<Image> partyIcons;
    // Alternatively, you could have 4 separate fields if you prefer.

    [Header("Item Slots")]
    [SerializeField] private List<Button> itemSlots;
    // or a list of Image if you only display them

    [Header("Monetization Hooks")]
    [SerializeField] private List<Image> monetizationIcons;
    // e.g., a small “$” icon in corner

    private void Start()
    {
        // Example: set default hero sprite or skill charge fill
        if (heroPortrait)
        {
            // Could set a default placeholder sprite or color
            // heroPortrait.sprite = somePlaceholderFrame;
        }

        // Example: disable all item slots if you have no items
        foreach (var slot in itemSlots)
        {
            if (slot != null) slot.interactable = false; 
        }

        // Similarly for party icons...
        // ...
    }

    // Example public methods for outside scripts:
    public void SetHeroPortrait(Sprite newSprite)
    {
        if (heroPortrait != null)
        {
            heroPortrait.sprite = newSprite;
        }
    }

    public void EnablePartyIcon(int index, Sprite iconSprite)
    {
        if (index >= 0 && index < partyIcons.Count && partyIcons[index] != null)
        {
            partyIcons[index].sprite = iconSprite;
            partyIcons[index].color = Color.white; // fully visible
        }
    }

    public void SetupItemSlot(int index, Sprite itemSprite)
    {
        if (index >= 0 && index < itemSlots.Count && itemSlots[index] != null)
        {
            Image slotImg = itemSlots[index].GetComponent<Image>();
            if (slotImg != null) slotImg.sprite = itemSprite;
            itemSlots[index].interactable = true;
        }
    }

    // ... other functionalities, e.g. hooking up monetizationIcons
}
