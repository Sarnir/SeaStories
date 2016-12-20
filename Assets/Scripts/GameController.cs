using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerShipController ActivePlayerShip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if(Input.GetMouseButtonUp(0))
        {
            HandleLeftClick();
        }
    }

    private void HandleLeftClick()
    {
        if(ActivePlayerShip != null)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ActivePlayerShip.SailToPosition(pos);
        }
    }
}
