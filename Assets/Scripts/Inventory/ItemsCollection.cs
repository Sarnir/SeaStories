using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollection
{
    Dictionary<string, uint> dictionary;

    public ItemsCollection()
    {
        dictionary = new Dictionary<string, uint>();
    }

    public Dictionary<string, uint>.Enumerator GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    public uint GetItemQuantity(string name)
    {
        if (dictionary.ContainsKey(name))
            return dictionary[name];
        else
            return 0;
    }

    public bool ContainsItem(string name)
    {
        return dictionary.ContainsKey(name);
    }

    public void SetItemQuantity(string name, uint quantity)
    {
        if (dictionary.ContainsKey(name))
            dictionary[name] = quantity;
        else
            dictionary.Add(name, quantity);
    }

    public void SetItemQuantity(string name, int quantity)
    {
        if (quantity < 1)
            return;

        SetItemQuantity(name, (uint)quantity);
    }

    public void AddItem(string name)
    {
        AddItems(name, 1);
    }

    public void AddItems(string name, int quantity)
    {
        if (quantity < 1)
            return;

        AddItems(name, (uint)quantity);
    }

    public void AddItems(string name, uint quantity)
    {
        if (dictionary.ContainsKey(name))
            dictionary[name] += quantity;
        else
            dictionary.Add(name, quantity);
    }

    public void RemoveItem(string name, int quantity)
    {
        if (quantity < 1)
            return;

        RemoveItem(name, (uint)quantity);
    }

    public void RemoveItem(string name, uint quantity)
    {
        if(dictionary.ContainsKey(name))
        {
            var currentQuantity = dictionary[name];

            if (currentQuantity <= quantity)
                dictionary.Remove(name);
            else
                dictionary[name] -= quantity;
        }
    }
}
