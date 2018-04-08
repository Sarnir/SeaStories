using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InputController : MonoBehaviour
{
	PlayerController player;
	UIController UIController;

	Vector3 mouseOrigin;
	bool isRMBDown;
	bool isLeftClickCanceled;

	public float ScrollFactor = 2f;
	public float PanSensitivity = 0.05f;
	bool isCameraPanning;
	Camera mainCamera;

	Pickup activePickup;

	void Start ()
	{
		activePickup = null;

		player = GameController.Instance.Player;
		UIController = GameController.Instance.UIController;
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

        if(EventSystem.current.IsPointerOverGameObject())
        {
            HandleHoverOn(new RaycastHit[0]);
            return;
        }


        var hits = Physics.RaycastAll (Camera.main.ScreenPointToRay (Input.mousePosition),
			100);

		HandleHoverOn (hits);

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
                HandleLeftClick(hits);
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

    private void HandleLeftClick(RaycastHit[] hits)
    {
        foreach(var hit in hits)
        {
            // pickups have higher priority
            if (hit.transform.gameObject.tag == "Pickup")
            {
                player.TakePickup(hit.transform.GetComponent<Pickup>());
                return;
            }
        }

        if(hits.Length > 0)
            player.SetDestination(hits[0].point);
    }

	private void HandleHoverOn(RaycastHit[] hits)
	{
		bool wasPickupHit = false;
		foreach (var hit in hits)
		{
			if (hit.transform.gameObject.tag == "Pickup")
			{
				wasPickupHit = true;
				if (activePickup != null)
				{
					if (!activePickup.gameObject.Equals (hit.transform.gameObject))
					{
						activePickup.OnCursorExit ();
						activePickup = hit.transform.GetComponent<Pickup> ();
						activePickup.OnCursorEnter ();
					}
				}
				else
				{
					activePickup = hit.transform.GetComponent<Pickup> ();
					activePickup.OnCursorEnter ();
				}
			}
		}

		if(activePickup != null && wasPickupHit == false)
		{
			activePickup.OnCursorExit ();
			activePickup = null;
		}
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
}
