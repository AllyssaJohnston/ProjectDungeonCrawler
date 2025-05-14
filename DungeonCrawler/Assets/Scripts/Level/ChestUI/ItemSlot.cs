using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    private Item storedItem;

    public void Setup(Item item)
    {
        storedItem = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (storedItem != null)
        {
            // Show the tooltip offset from the current mouse position
            Vector3 offset = new Vector3(120f, -15f); // Adjust as needed
            TooltipManager.instance.ShowTooltip(
                storedItem.description,
                Input.mousePosition + offset
            );
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.instance.HideTooltip();
    }

    public void OnClick()
    {
        
        FindFirstObjectByType<PlayerInventory>()?.AddItem(storedItem);
        TooltipManager.instance.HideTooltip();
        // remove this UI element after picking it up
        Destroy(gameObject);
    }
}
