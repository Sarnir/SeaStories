using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public uint Gold
    {
        get
        {
            return _gold;
        }

        private set
        {
            _gold = value;
        }
    }

    private uint _gold;

    ItemsCollection inventory;

    public void Setup()
    {
        if(inventory == null)
        {
            inventory = new ItemsCollection();
        }
    }

	public void Setup(uint startingGold, ItemsDictionary startingItems)
    {
        Gold = startingGold;

		foreach (KeyValuePair<ItemName, uint> item in startingItems)
        {
            AddItems(item.Key, item.Value);
        }
    }

	public uint GetQuantity(ItemName itemName)
    {
        return inventory.GetItemQuantity(itemName);
    }

	public void AddItem(ItemName itemName)
    {
        AddItems(itemName, 1);
    }

    public void AddItem(ItemDefinition item)
    {
        AddItems(item, 1);
    }

    public void AddItems(ItemDefinition item, uint quantity)
    {
        AddItems(item.Name, quantity);
    }

	public void AddItems(ItemName itemName, int quantity)
    {
        if (quantity < 1)
            return;

        AddItems(itemName, (uint)quantity);
    }

	public void AddItems(ItemName itemName, uint quantity)
    {
        if (quantity == 0)
            return;

        inventory.AddItems(itemName, quantity);
    }

	public void AddItems(ItemsDictionary items)
	{
		foreach (var item in items)
		{
			inventory.AddItems (item.Key, item.Value);
		}
	}

	public void RemoveItem(ItemName itemName)
    {
        RemoveItems(itemName, 1);
    }

	public void RemoveItems(ItemName itemName, int quantity)
    {
        if (quantity <= 0)
            return;

        inventory.RemoveItem(itemName, quantity);
    }

    public int GetGold()
    {
        return (int)Gold;
    }

    public void SetGold(int quantity)
    {
        Gold = (uint)quantity;
    }

    public void AddGold(int quantity)
    {
        Gold += (uint)quantity;
    }

    public void RemoveGold(int quantity)
    {
        Gold -= (uint)quantity;
    }

	public bool ContainsItem(ItemName itemName)
    {
        if (!inventory.ContainsItem(itemName))
            return false;

        return inventory.GetItemQuantity(itemName) > 0;
    }

    public bool SellItem(ShopItem item, Inventory buyersInventory)
    {
        if (buyersInventory.GetGold() < item.Price || !ContainsItem(item.Definition.Name))
            return false;

        buyersInventory.RemoveGold(item.Price);
        RemoveItem(item.Definition.Name);
        buyersInventory.AddItem(item.Definition.Name);
        AddGold(item.Price);

        return true;
    }

    public ItemsCollection GetAllItems()
    {
        return inventory;
    }
}
