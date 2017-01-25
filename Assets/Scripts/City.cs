using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    Inventory inventory;
    ItemsCollection itemsToBuy;
    ItemsCollection itemsToSell;

    public static City Create(string name, Transform parent, Vector3 position)
    {
        var cityObject = new GameObject(name);
        cityObject.transform.parent = parent;
        cityObject.transform.localPosition = position;
        var coll = cityObject.AddComponent<BoxCollider>();
        coll.center = new Vector3(0f, 0.5f, 0f);
        coll.size = new Vector3(1.5f, 1f, 1.5f);

        var city = cityObject.AddComponent<City>();
        city.inventory = cityObject.AddComponent<Inventory>();
        city.inventory.Setup();
        city.inventory.AddItems("Spices", Random.Range(1, 100));
        city.inventory.AddGold(Random.Range(1, 100));

        city.itemsToBuy = new ItemsCollection();
        city.itemsToBuy.AddItems("Tobacco", Random.Range(1, 10));

        city.itemsToSell = new ItemsCollection();
        city.itemsToSell.AddItems("Spices", Random.Range(1, 10));

        return city;
    }

    void OnCollisionEnter(Collision collision)
    {
        string message = string.Format("OnCollisionEnter: Object {0} collided with city named {1}", collision.gameObject.name, name);
        Debug.Log(message);
    }

    void OnTriggerEnter(Collider other)
    {
        string message = string.Format("OnTriggerEnter: Object {0} collided with city named {1}", other.gameObject.name, name);
        Debug.Log(message);
    }

    public ItemsCollection GetItemsForSale()
    {
        return itemsToSell;
    }

    public ItemsCollection GetItemsToBuy()
    {
        return itemsToBuy;
    }

    public int GetGold()
    {
        return inventory.GetGold();
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}
