using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    ObjectPool<InventoryItem> inventoryItemsPool;

    public InventoryItem ItemPrefab;
    
    Inventory inventory;

    void Start()
    {
        inventoryItemsPool = new ObjectPool<InventoryItem>(ItemPrefab, transform);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void SetActive(bool val)
    {
        gameObject.SetActive(val);
    }

    public void GenerateItemList(Inventory _inventory)
    {
        inventory = _inventory;

        Refresh();
    }

    public void Refresh()
    {
        inventoryItemsPool.SetAllElementsInactive();

        foreach (var item in inventory.GetAllItems())
        {
            CreateInventoryItem(item.Key, item.Value, transform);
        }
    }

	void CreateInventoryItem(ItemName itemName, uint quantity, Transform parent)
    {
        InventoryItem item = inventoryItemsPool.GetElementFromPool();

        item.SetItem(ItemDatabase.GetItemDefinition(itemName), (int)quantity);
    }
}
