using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    public PlayerController Player;
    public float ScrollFactor;
    public float PanSensitivity;
    public UIController UIController;
	public Transform Crossmark;

    Camera mainCamera;
    Vector3 mouseOrigin;
    bool isCameraPanning;
    bool isRMBDown;
	bool isLeftClickCanceled;

    static GameController _instance;
    public static GameController GetInstance()
    {
        if (_instance == null)
            _instance = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        return _instance;
    }

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
			isLeftClickCanceled = false;
		}
		
		if(Input.GetMouseButton(0))
		{
			var mouseDeltaPos = (mouseOrigin - Input.mousePosition);

			if (mouseDeltaPos.magnitude > 7f)
					isLeftClickCanceled = true;
		}
		
		if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			if(!isLeftClickCanceled)
			{
				RaycastHit hit;
				
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                    if (hit.transform.gameObject.tag == "Pickup")
                        Player.TakePickup(hit.transform.GetComponent<Pickup>());
                    else
                        Player.SetDestination(hit.point);
				}
			}
		}

		if (Input.GetMouseButtonDown(1))
        {
            mouseOrigin = Input.mousePosition;
            isRMBDown = true;
        }

        if(Input.GetMouseButton(1))
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

        if (Input.GetMouseButtonUp(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            isCameraPanning = false;
            isRMBDown = false;
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
