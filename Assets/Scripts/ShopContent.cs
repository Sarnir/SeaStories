using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    List<ShopItem> shopItemsPool;

    public ShopItem ShopItemPrefab;

    public Action<ShopItem> OnShopItemClick;

    Inventory inventory;
    ItemsCollection itemsWithPricesFilter;

    void Start()
    {
        shopItemsPool = new List<ShopItem>();
    }

    ShopItem GetElementFromPool()
    {
        for (int i = 0; i < shopItemsPool.Count; i++)
        {
            if (!shopItemsPool[i].gameObject.activeSelf)
            {
                shopItemsPool[i].gameObject.SetActive(true);
                return shopItemsPool[i];
            }
        }
        ShopItem item;
        item = Instantiate(ShopItemPrefab, transform, false);
        item.gameObject.SetActive(true);
        item.button.onClick.AddListener(() => ShopItemClicked(item));
        shopItemsPool.Add(item);

        return item;
    }

    void SetAllElementsInactive()
    {
        for (int i = 0; i < shopItemsPool.Count; i++)
        {
            shopItemsPool[i].gameObject.SetActive(false);
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void SetActive(bool val)
    {
        gameObject.SetActive(val);
    }

    public void GenerateItemList(Inventory _inventory, ItemsCollection tradeItemsWithPrices = null)
    {
        inventory = _inventory;
        itemsWithPricesFilter = tradeItemsWithPrices;

        Refresh();
    }

    public void Refresh()
    {
        SetAllElementsInactive();

        foreach (var itemWithPrice in itemsWithPricesFilter)
        {
            if (inventory.ContainsItem(itemWithPrice.Key))
            {
                CreateShopItem(itemWithPrice.Key, inventory.GetQuantity(itemWithPrice.Key), itemWithPrice.Value, transform);
            }
        }
    }

    void CreateShopItem(string itemName, uint quantity, uint price, Transform parent)
    {
        ShopItem item = GetElementFromPool();

        item.SetItem(ItemDatabase.GetItemDefinition(itemName), (int)quantity, (int)price);
    }

    void ShopItemClicked(ShopItem item)
    {
        OnShopItemClick(item);
    }
}
