using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    ObjectPool<InventoryItem> inventoryItemsPool;

    // to winno być zrobione na eventach
    public ItemInfoView ItemInfoView;
    public InventoryItem ItemPrefab;
    
    Inventory inventory;

    bool isInit = false;

    void Start()
    {
        if (!isInit)
            Init();
    }

    void Init()
    {
        isInit = true;
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
        if (!isInit)
            Init();

        inventoryItemsPool.SetAllElementsInactive();

        foreach (var item in inventory.GetAllItems())
        {
            CreateInventoryItem(item.Key, item.Value);
        }

        var items = inventoryItemsPool.GetAllActiveElements();

        if (items.Count > 0)
        {
            ItemInfoView.gameObject.SetActive(true);
            ItemInfoView.SetItemInfo(items[0].Definition);
        }
        else
        {
            ItemInfoView.gameObject.SetActive(false);
        }
    }

	void CreateInventoryItem(ItemName itemName, uint quantity)
    {
        // we don't want to display gold as item in slot
        if (itemName == ItemName.Gold)
            return;

        InventoryItem item = inventoryItemsPool.GetElementFromPool();

        var itemDefinition = ItemDatabase.GetItemDefinition(itemName);

        item.SetItem(itemDefinition, (int)quantity);
        item.button.onClick.AddListener(() => ItemInfoView.SetItemInfo(itemDefinition));
    }
}
