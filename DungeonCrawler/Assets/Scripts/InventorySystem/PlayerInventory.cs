using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public InventoryUI inventoryUI;

    private bool isOpen = false;

    void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = FindFirstObjectByType<InventoryUI>();
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log($"Picked up: {item.itemName}");
    }

    public void RemoveItem(Item item)
    {

        items.Remove(item);
        Debug.Log($"Used item: {item.itemName}");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!isOpen)
            {
                inventoryUI.OpenInventory(items);
                isOpen = true;
            }
            else
            {
                inventoryUI.CloseInventory();
                isOpen = false;
            }
        }
    }
}
