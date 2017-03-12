using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public bool LogsEnabled;
    
    public Inventory Inventory;
    GameController gameController;
    Sails sails;

    float currentFoodUnitLeft;

    public bool IsConsumingFood;

    void Start()
    {
        IsConsumingFood = true;
        Inventory = GetComponent<Inventory>();
        Inventory.Setup();
        Inventory.AddGold(50);
        Inventory.AddItems("Tobacco", 3);
        Inventory.AddItems("Food", 5);
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        sails = GetComponentInChildren<Sails>();
    }

    void Update()
    {
        if (IsConsumingFood)
            ConsumeFoodStep();
    }

    void StopShip()
    {
        sails.ReefSails(true);
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
        return Inventory.GetQuantity("Food") + currentFoodUnitLeft;
    }

    void ConsumeFoodStep()
    {
        var consumptionRate = 0.0005f;
        currentFoodUnitLeft -= consumptionRate;

        if(currentFoodUnitLeft < 0f)
        {
            if (Inventory.GetQuantity("Food") > 0)
            {
                Inventory.RemoveItem("Food");
                currentFoodUnitLeft += 1f;
            }
            else
            {
                currentFoodUnitLeft = 0f;
            }
        }
    }
}
