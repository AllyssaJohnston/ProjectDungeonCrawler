using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private PlayerInventory inventory;
    public Image iconImage;
    private Item storedItem;

    public void Setup(Item item, PlayerInventory inventory)
    {
        this.inventory = inventory;
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
    }
}
