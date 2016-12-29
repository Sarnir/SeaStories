using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerShipController ActivePlayerShip;
    public float ScrollFactor;
    float cameraZOffset;
    Camera mainCamera;

    // Use this for initialization
    void Start () {
        mainCamera = Camera.main;
        cameraZOffset = mainCamera.transform.position.z;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            HandleLeftClick();
        }

        HandleScrollWheel(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void HandleScrollWheel(float scrollValue)
    {
        mainCamera.orthographicSize += scrollValue * ScrollFactor;
    }

    private void HandleLeftClick()
    {
        if(ActivePlayerShip != null)
        {
            /*var mousePos = Input.mousePosition;
            mousePos.z = 11.35f;
            var pos = Camera.main.ScreenToWorldPoint(mousePos);
            ActivePlayerShip.SailToPosition(pos);*/

            var hitInfo = new RaycastHit();
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                Debug.Log("HIT at " + hitInfo.point);
                ActivePlayerShip.SailToPosition(hitInfo.point);
            }
        }
    }
}
