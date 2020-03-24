using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    ObjectPool<ShopItem> shopItemsPool;

    public ShopItem ShopItemPrefab;

    public Action<ShopItem> OnShopItemClick;

    Inventory inventory;
    Inventory buyerInventory;
    ItemsDictionary itemsWithPricesFilter;

    void Awake()
    {
        shopItemsPool = new ObjectPool<ShopItem>(ShopItemPrefab, transform);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void SetActive(bool val)
    {
        gameObject.SetActive(val);
    }

    public void GenerateItemList(Inventory _inventory, Inventory _buyersInventory, ItemsDictionary tradeItemsWithPrices)
    {
        inventory = _inventory;
        buyerInventory = _buyersInventory;
        itemsWithPricesFilter = tradeItemsWithPrices;

        Refresh();
    }

    public void Refresh()
    {
        shopItemsPool.SetAllElementsInactive();

        foreach (var itemWithPrice in itemsWithPricesFilter)
        {
            if (inventory.ContainsItem(itemWithPrice.Key))
            {
                var item = CreateShopItem(itemWithPrice.Key, inventory.GetQuantity(itemWithPrice.Key), itemWithPrice.Value, transform);
                item.SetTradeable(buyerInventory.GetGold() >= itemWithPrice.Value);
            }
        }
    }

	ShopItem CreateShopItem(ItemName itemName, uint quantity, uint price, Transform parent)
    {
        ShopItem item = shopItemsPool.GetElementFromPool();

        item.SetItem(ItemDatabase.GetItemDefinition(itemName), (int)quantity, (int)price);
        item.SetOnClick(() => ShopItemClicked(item));
        return item;
    }

    void ShopItemClicked(ShopItem item)
    {
        OnShopItemClick(item);
    }
}
