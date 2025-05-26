using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;
    public TextMeshProUGUI inventoryIndicatorText;
    private PlayerInventory inventory;

    void Start()
    {
        if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void OpenInventory(List<Item> contents)
    {
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
        inventoryPanel.SetActive(true);
        inventoryIndicatorText.gameObject.SetActive(true);
        ClearSlots();

        // Create new slots
        for (int i = 0; i < contents.Count; i++)
        {
            var item = contents[i];
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            slot.GetComponent<ItemSlot>().Setup(item, inventory, contents, i, ItemSlot.SlotMode.Inventory);
        }
    }

    public void CloseInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventoryPanel.SetActive(false);
        inventoryIndicatorText.gameObject.SetActive(false);
    }

    private void ClearSlots()
    {
        foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }
    }
}
