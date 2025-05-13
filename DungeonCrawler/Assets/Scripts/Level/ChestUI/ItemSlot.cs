using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image iconImage;
    private Item storedItem;

    public void Setup(Item item)
    {
        storedItem = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public void OnClick()
    {
        
        FindFirstObjectByType<PlayerInventory>()?.AddItem(storedItem);

        // remove this UI element after picking it up
        Destroy(gameObject);
    }
}
