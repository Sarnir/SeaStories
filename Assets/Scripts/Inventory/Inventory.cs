using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsDictionary : Dictionary<ItemName, uint> { }

public class Inventory
{
    uint maxCapacity;

    ItemsDictionary itemsDictionary;

    public void Setup(int _maxCapacity)
    {
        Setup((uint)_maxCapacity);
    }

    public void Setup(uint _maxCapacity)
    {
        maxCapacity = _maxCapacity;

        if(itemsDictionary == null)
        {
            itemsDictionary = new ItemsDictionary();
        }
    }

    public void Setup(int _maxCapacity, ItemsDictionary startingItems)
    {
        Setup((uint)_maxCapacity, startingItems);
    }

    public void Setup(uint _maxCapacity, ItemsDictionary startingItems)
    {
        Setup(_maxCapacity);

		foreach (KeyValuePair<ItemName, uint> item in startingItems)
        {
            AddItems(item.Key, item.Value);
        }
    }

    public ItemsDictionary.Enumerator GetEnumerator()
    {
        return itemsDictionary.GetEnumerator();
    }

    public uint GetQuantity(ItemName name)
    {
        if (itemsDictionary.ContainsKey(name))
            return itemsDictionary[name];
        else
            return 0;
    }

    public uint GetCurrentCapacity()
    {
        uint current = 0;

        foreach(var item in itemsDictionary)
        {
            if(item.Key != ItemName.Gold)
            {
                current += item.Value;
            }
        }

        return current;
    }

    public uint GetMaxCapacity()
    {
        return maxCapacity;
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

    public void AddItems(ItemName name, uint quantity)
    {
        if (quantity == 0)
            return;

        if (itemsDictionary.ContainsKey(name))
            itemsDictionary[name] += quantity;
        else
            itemsDictionary.Add(name, quantity);
    }

	public void AddItems(ItemsDictionary items)
	{
		foreach (var item in items)
		{
			AddItems (item.Key, item.Value);
		}
	}

	public void RemoveItem(ItemName itemName)
    {
        RemoveItems(itemName, 1);
    }

    public void RemoveItems(ItemName name, int quantity)
    {
        if (quantity < 1)
            return;

        RemoveItems(name, (uint)quantity);
    }

	public void RemoveItems(ItemName name, uint quantity)
    {
        if (quantity <= 0)
            return;

        if (itemsDictionary.ContainsKey(name))
        {
            var currentQuantity = itemsDictionary[name];

            // we have an exception for gold -
            // even if its zero don't remove it from the collection
            if (currentQuantity <= quantity && name != ItemName.Gold)
                itemsDictionary.Remove(name);
            else
                itemsDictionary[name] -= quantity;
        }
    }

    public int GetGold()
    {
        return (int)GetQuantity(ItemName.Gold);
    }

    public void AddGold(int quantity)
    {
        AddItems(ItemName.Gold, quantity);
    }

    public void RemoveGold(int quantity)
    {
        RemoveItems(ItemName.Gold, quantity);
    }

	public bool ContainsItem(ItemName itemName)
    {
        if (itemsDictionary.ContainsKey(itemName))
            return itemsDictionary[itemName] > 0;
        else
            return false;
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

    public void TransferTo(Inventory taker)
    {
        foreach(var item in itemsDictionary)
        {
            taker.AddItems(item.Key, item.Value);
        }

        itemsDictionary.Clear();
    }

    public ItemsDictionary GetAllItems()
    {
        return itemsDictionary;
    }

    public string Print()
    {
        string inventoryString = "";
        foreach (var item in itemsDictionary)
        {
            inventoryString += item.Value + " " + item.Key + "\n";
            Debug.Log(item.Value + " " + item.Key);
        }

        return inventoryString;
    }
}
