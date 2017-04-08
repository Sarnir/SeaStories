using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public ItemDefinition[] Items;

    static ItemDefinition[] items;

    void Start()
    {
        items = Items;
    }

	public static ItemDefinition GetItemDefinition(ItemName name)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].Name == name)
                return items[i];
        }

        return null;
    }
}
