using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    public PlayerController ActivePlayer;
    public float ScrollFactor;
    public float PanSensitivity;
    public UIController UIController;

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
        if (UIController.ShopWindow.IsOpened())
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
                mainCamera.transform.Translate(new Vector3(mouseDeltaPos.x, 0f, mouseDeltaPos.y) * PanSensitivity, Space.World);
                mouseOrigin = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
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
        mainCamera.transform.Translate(new Vector3(0f, 0f, scrollValue) * ScrollFactor, Space.Self);
    }

    public void OpenShop(City city)
    {
        UIController.ShopWindow.OpenShop(ActivePlayer.Inventory, city);
    }

    public void CloseShop()
    {
        UIController.ShopWindow.Close();
        ActivePlayer.LeaveCity();
    }

    public void ToggleInventory()
    {
        UIController.InventoryWindow.ToggleInventory(ActivePlayer.Inventory);
        ActivePlayer.IsConsumingFood = !ActivePlayer.IsConsumingFood;
    }
}
