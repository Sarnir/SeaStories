using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    const string GOLD_NAME_STRING = "Gold";

    public uint Gold
    {
        get
        {
            return inventory.GetItemQuantity(GOLD_NAME_STRING);
        }

        private set
        {
            inventory.SetItemQuantity(GOLD_NAME_STRING, value);
        }
    }

    

    ItemsCollection inventory;
	void Start ()
    {
        Setup();
	}

    public void Setup()
    {
        if(inventory == null)
        {
            inventory = new ItemsCollection();
            inventory.AddItem(GOLD_NAME_STRING);
        }
    }

    public void Setup(uint startingGold, Dictionary<string, uint> startingItems)
    {
        Gold = startingGold;

        foreach (KeyValuePair<string, uint> item in startingItems)
        {
            AddItems(item.Key, item.Value);
        }
    }

    void PrintInventory()
    {
        Debug.Log("Items in inventory of " + name + ":");
        foreach(var item in inventory)
        {
            Debug.Log(item.Key + " - " + item.Value);
        }
    }

    public uint GetQuantity(string itemName)
    {
        return inventory.GetItemQuantity(itemName);
    }

    public void AddItem(string itemName)
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

    public void AddItems(string itemName, int quantity)
    {
        if (quantity < 1)
            return;

        AddItems(itemName, (uint)quantity);
    }

    public void AddItems(string itemName, uint quantity)
    {
        if (quantity == 0)
            return;

        inventory.AddItems(itemName, quantity);
    }

    public void RemoveItem(string itemName)
    {
        RemoveItems(itemName, 1);
    }

    public void RemoveItems(string itemName, int quantity)
    {
        if (quantity <= 0)
            return;

        inventory.RemoveItem(itemName, quantity);
    }

    public int GetGold()
    {
        return (int)inventory.GetItemQuantity(GOLD_NAME_STRING);
    }

    public void SetGold(int quantity)
    {
        inventory.SetItemQuantity(GOLD_NAME_STRING, quantity);
    }

    public void AddGold(int quantity)
    {
        inventory.AddItems(GOLD_NAME_STRING, quantity);
    }

    public void RemoveGold(int quantity)
    {
        RemoveItems(GOLD_NAME_STRING, quantity);
    }

    public bool ContainsItem(string itemName)
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

        PrintInventory();

        return true;
    }
}
