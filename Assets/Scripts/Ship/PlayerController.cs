﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct ItemsQuantities
{
	public ItemName name;
	public uint quantity;
}

public class PlayerController : ShipController
{
    public bool LogsEnabled;
	public bool IsConsumingFood;

	public ItemsQuantities[] StartingItems;
    GameController gameController;

    float currentFoodUnitLeft;

    void Start()
    {
        IsConsumingFood = true;

		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		sails = GetComponentInChildren<Sails> ();
		rudder = GetComponentInChildren<Rudder> ();

		ItemsDictionary dict = new ItemsDictionary ();

		for (int i = 0; i < StartingItems.Length; i++)
		{
			dict.Add (StartingItems [i].name, StartingItems [i].quantity);
		}

		SetupInventory (dict);
    }

    void Update()
    {
        if (IsConsumingFood)
            ConsumeFoodStep();
    }

    /*void OnTriggerEnter(Collider other)
    {
        Debug.Log("WOLOLO");
        var city = other.GetComponent<City>();

        if (city)
            EnterCity(city);
    }*/

    public void EnterCity(City city)
    {
        StopShip();
        IsConsumingFood = false;
        gameController.OpenShop(city);
    }

    public void LeaveCity()
    {
        IsConsumingFood = true;
        transform.Rotate(0f, 0f, 180f);
    }

    public float GetCurrentFood()
    {
		return inventory.GetQuantity(ItemName.Food) + currentFoodUnitLeft;
    }

    void ConsumeFoodStep()
    {
        var consumptionRate = 0.0005f;
        currentFoodUnitLeft -= consumptionRate;

        if(currentFoodUnitLeft < 0f)
        {
			if (inventory.GetQuantity(ItemName.Food) > 0)
            {
				inventory.RemoveItem(ItemName.Food);
                currentFoodUnitLeft += 1f;
            }
            else
            {
                currentFoodUnitLeft = 0f;
            }
        }
    }
}