using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public GameObject chestPanel;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;
    public PlayerInventory inventory;

    void Start()
    {
        if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void OpenChest(List<Item> contents)
    {
        chestPanel.SetActive(true);

        // Clear existing slots
        foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        foreach (var item in contents)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            slot.GetComponent<ItemSlot>().Setup(item, inventory);
        }
    }

    public void CloseChest()
    {
        chestPanel.SetActive(false);
    }
}
