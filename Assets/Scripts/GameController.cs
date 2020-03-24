using System;
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

    // lepiej zrób na evencie?
    public void SetCrossmark(Vector3 point)
    {
        EnableCrossmark(true);
        point = new Vector3(point.x, 0f, point.z);
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

    public void ToggleMenu()
    {
        UIController.MenuWindow.ToggleMenu(Player.GetInventory());
        TogglePauseWorld();
    }

    public void TogglePauseWorld()
    {
        Player.IsConsumingFood = !Player.IsConsumingFood;
    }
}
