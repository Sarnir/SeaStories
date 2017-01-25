using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour {

    public PlayerShipController ActivePlayerShip;
    public float ScrollFactor;
    public float PanSensitivity;
    public ShopScreen ShopScreen;

    Camera mainCamera;
    Vector3 mouseOrigin;
    bool isCameraPanning;
    bool isLMBDown;

    // Use this for initialization
    void Start () {
        mainCamera = Camera.main;
        isCameraPanning = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if (ShopScreen.IsOpened())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            mouseOrigin = Input.mousePosition;
            isLMBDown = true;
        }

        if(Input.GetMouseButton(0))
        {
            var mouseDeltaPos = (mouseOrigin - Input.mousePosition);

            if (!isCameraPanning)
            {
                if (mouseDeltaPos.magnitude > 7f)
                    isCameraPanning = true;
            }

            if (isCameraPanning)
            {
                mainCamera.transform.Translate(mouseDeltaPos * PanSensitivity);
                mouseOrigin = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isLMBDown && !isCameraPanning)
                HandleLeftClick();

            isCameraPanning = false;
            isLMBDown = false;
        }

        HandleScrollWheel(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void PanCamera(Vector3 mouseDeltaPos)
    {
        isCameraPanning = true;
        mainCamera.transform.DOMove(mouseDeltaPos, 1.0f);
    }

    private void HandleScrollWheel(float scrollValue)
    {
        mainCamera.orthographicSize += scrollValue * ScrollFactor;
    }

    private void HandleLeftClick()
    {
        if(ActivePlayerShip != null)
        {
            var hitInfo = new RaycastHit();
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                ActivePlayerShip.SailToPosition(hitInfo.point);
            }
        }
    }
}
