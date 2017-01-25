using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerShipController : MonoBehaviour
{
    public float rotateSpeed = 1f;
    public float maxSpeed = 2f;

    public bool LogsEnabled;

    Vector3 destinationPosition;
    Tweener currentMoveTween;

    Inventory inventory;
    GameController gameController;

    public void Start()
    {
        inventory = GetComponent<Inventory>();
        inventory.Setup();
        inventory.AddGold(50);
        inventory.AddItems("Tobacco", 3);
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    public void SailToPosition(Vector3 pos)
    {
        if(LogsEnabled)
            Debug.Log("Moving to pos " + pos);
        if(currentMoveTween != null)
            currentMoveTween.Kill();

        destinationPosition = new Vector3(pos.x, transform.position.y, pos.z);
        var offsetPos = destinationPosition - transform.position;
        var newDirection = offsetPos.normalized;
        if(LogsEnabled)
            Debug.Log("new direction = " + newDirection);

        currentMoveTween = transform.DOMove(destinationPosition, offsetPos.magnitude / maxSpeed);
        var angle = -Mathf.Atan2(newDirection.z, newDirection.x) * Mathf.Rad2Deg +90;
        if(LogsEnabled)
            Debug.Log("Angle set to " + angle);

        transform.DORotate(new Vector3(0, angle, 0), rotateSpeed);
    }

    void StopShip()
    {
        if (currentMoveTween != null)
            currentMoveTween.Kill();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("WOLOLO");
        var city = other.GetComponent<City>();

        if (city)
            StartTradingWith(city);
    }

    void StartTradingWith(City city)
    {
        StopShip();
        gameController.ShopScreen.OpenShop(inventory, city);
    }
}
