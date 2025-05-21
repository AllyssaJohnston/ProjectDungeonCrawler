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

    public enum SlotMode
    {
        Chest,
        Inventory
    }
    private SlotMode slotMode;

    public void Setup(Item item, PlayerInventory inventory, List<Item> contents, int index, SlotMode slotMode)
    {
        this.inventory = inventory;
        this.chestContents = contents;
        this.slotMode = slotMode;
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
                storedItem.flavor,
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

        TooltipManager.HideTooltip();

        // If the item is used while in a chest.
        if (slotMode == SlotMode.Chest)
        {
            inventory.AddItem(storedItem);
            chestContents.RemoveAt(itemIndex);
            Destroy(gameObject);
        }
        // If the item is used while in your inventory.
        else if (slotMode == SlotMode.Inventory)
        {
            storedItem.Use();
            inventory.RemoveItem(storedItem);
            Destroy(gameObject);
        }
        
    }
}
