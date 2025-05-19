using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private PlayerInventory inventory;
    public Image iconImage;
    private Item storedItem;

    private List<Item> chestContents;

    private int itemIndex;

    public void Setup(Item item, PlayerInventory inventory, List<Item> contents, int index)
    {
        this.inventory = inventory;
        this.chestContents = contents;
        itemIndex = index;
        storedItem = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("on pointer enter");
        if (storedItem != null)
        {
            // Show the tooltip offset from the current mouse position
            Vector3 offset = new Vector3(120f, -15f); // Adjust as needed
            TooltipManager.ShowTooltip(
                storedItem.description,
                Input.mousePosition + offset
            );
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.HideTooltip();
    }

    public void OnClick()
    {

        inventory.AddItem(storedItem);
        TooltipManager.HideTooltip();
        // remove this UI element after picking it up
        Destroy(gameObject);
        // and remove the item from the chests list
        chestContents.RemoveAt(itemIndex);
        
    }
}
