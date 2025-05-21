using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public GameObject chestPanel;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;
    private PlayerInventory inventory;

    void Start()
    {
        if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
	}

    public void OpenChest(List<Item> contents)
    {
		    
        chestPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		// Clear existing slots
		foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        for (int i = 0; i < contents.Count; i++)
        {
            var item = contents[i];
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            slot.GetComponent<ItemSlot>().Setup(item, inventory, contents, i, ItemSlot.SlotMode.Chest);
        }
    }

    public void CloseChest()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		chestPanel.SetActive(false);
    }
}
