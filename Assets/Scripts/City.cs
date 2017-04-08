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
        coll.isTrigger = true;
        coll.center = new Vector3(0f, 0.5f, 0f);
        coll.size = new Vector3(1.5f * parent.localScale.x, 1f, 1.5f * parent.localScale.z);

        var city = cityObject.AddComponent<City>();
		city.inventory = new Inventory ();
        city.inventory.Setup();
		city.inventory.AddItems(ItemName.Spices, Random.Range(1, 100));
        city.inventory.AddGold(Random.Range(1, 100));

        city.itemsToBuy = new ItemsCollection();
		city.itemsToBuy.AddItems(ItemName.Tobacco, Random.Range(1, 10));

        city.itemsToSell = new ItemsCollection();
		city.itemsToSell.AddItems(ItemName.Spices, Random.Range(1, 10));

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

        if(other.isTrigger && other.gameObject.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();

            player.EnterCity(this);
        }
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
