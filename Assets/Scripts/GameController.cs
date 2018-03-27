﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerController Player;
    public UIController UIController;
	public Transform Crossmark;

	static GameController _instance;
    public static GameController Instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindWithTag("GameController").GetComponent<GameController>();

			return _instance;
		}
	}

    public void OpenShop(City city)
    {
		UIController.ShopWindow.OpenShop(Player.GetInventory(), city);
    }

    public void SetCrossmark(Vector3 point)
    {
        EnableCrossmark(true);
        Crossmark.transform.position = point;
    }

    public void EnableCrossmark(bool enable)
    {
        Crossmark.gameObject.SetActive(enable);
    }

    public void CloseShop()
    {
        UIController.ShopWindow.Close();
        Player.LeaveCity();
    }

    public void ToggleInventory()
    {
		UIController.InventoryWindow.ToggleInventory(Player.GetInventory());
        Player.IsConsumingFood = !Player.IsConsumingFood;
    }
}
