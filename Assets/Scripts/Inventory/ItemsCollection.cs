using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Obsolete()]
public class ItemsCollection
{
	ItemsDictionary dictionary;

    public ItemsCollection()
    {
		dictionary = new ItemsDictionary();
    }

	public ItemsDictionary.Enumerator GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

	public uint GetItemQuantity(ItemName name)
    {
        if (dictionary.ContainsKey(name))
            return dictionary[name];
        else
            return 0;
    }

    public void Clear()
    {
        dictionary.Clear();
    }

	public bool ContainsItem(ItemName name)
    {
        return dictionary.ContainsKey(name);
    }

	public void SetItemQuantity(ItemName name, uint quantity)
    {
        if (dictionary.ContainsKey(name))
            dictionary[name] = quantity;
        else
            dictionary.Add(name, quantity);
    }

	public void SetItemQuantity(ItemName name, int quantity)
    {
        if (quantity < 1)
            return;

        SetItemQuantity(name, (uint)quantity);
    }

	public void AddItem(ItemName name)
    {
        AddItems(name, 1);
    }

	public void AddItems(ItemName name, int quantity)
    {
        if (quantity < 1)
            return;

        AddItems(name, (uint)quantity);
    }

	public void AddItems(ItemName name, uint quantity)
    {
        if (dictionary.ContainsKey(name))
            dictionary[name] += quantity;
        else
            dictionary.Add(name, quantity);
    }

	public void RemoveItem(ItemName name, int quantity)
    {
        if (quantity < 1)
            return;

        RemoveItem(name, (uint)quantity);
    }

	public void RemoveItem(ItemName name, uint quantity)
    {
        if(dictionary.ContainsKey(name))
        {
            var currentQuantity = dictionary[name];

            // we have an exception for gold -
            // even if its zero don't remove it from the collection
            if (currentQuantity <= quantity && name != ItemName.Gold)
                dictionary.Remove(name);
            else
                dictionary[name] -= quantity;
        }
    }
}
