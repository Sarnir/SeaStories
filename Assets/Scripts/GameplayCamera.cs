using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameplayCamera : MonoBehaviour
{
    public float ScrollFactor;
    public float PanSensitivity;
    public float CameraSpeed;

    public bool IsFollowingPlayer;

    GameController gameController;
    Camera cam;

    Vector3 lookAtCentered;
    
    Vector3 mouseOrigin;
    bool isLMBDown;
    bool isCameraPanning;

    void Awake ()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        cam = GetComponent<Camera>();
        isCameraPanning = false;
    }

    void Update ()
    {
        ProcessInput();

        if (IsFollowingPlayer)
            FollowPlayer();
    }

    void FollowPlayer()
    {
        var playerPos = gameController.Player.transform.position;
        var distance = (playerPos - transform.position);
        distance.y = 0f;

        // get inverted position here xd

       // cam.transform.DOMove(playerPos, distance / CameraSpeed);
    }

    /// <summary>
    /// Note: Projected here means cast on a plane with y = 0
    /// </summary>
    public Vector3 GetProjectedPosition(Ray ray)
    {
        float distance = 0f;
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        plane.Raycast(ray, out distance);

        return ray.GetPoint(distance);
    }

    /// <summary>
    /// Note: Projected here means cast on a plane with y = 0
    /// </summary>
    public Bounds GetProjectedBounds()
    {
        var x1Ray = cam.ViewportPointToRay(new Vector3(0f, 0f, cam.nearClipPlane));
        var x2Ray = cam.ViewportPointToRay(new Vector3(1f, 0f, cam.nearClipPlane));
        var y1Ray = cam.ViewportPointToRay(new Vector3(0f, 1f, cam.nearClipPlane));
        var y2Ray = cam.ViewportPointToRay(new Vector3(1f, 1f, cam.nearClipPlane));
        var centerRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, cam.nearClipPlane));

        var x1 = GetProjectedPosition(x1Ray);
        var x2 = GetProjectedPosition(x2Ray);
        var y1 = GetProjectedPosition(y1Ray);
        var y2 = GetProjectedPosition(y2Ray);
        var center = GetProjectedPosition(centerRay);

        x1.x = y1.x;
        x2.x = y2.x;

        Debug.Log("center = " + center + " x1 = " + x1 + " x2 = " + x2 + " y1 = " + y1 + " y2 = " + y2);

        var tmp = new Bounds();
        tmp.center = center;
        tmp.min = x1;
        tmp.max = y2;

        return tmp;

        return new Bounds(lookAtCentered, new Vector3((x2 - x1).magnitude, 0f, (y2 - y1).magnitude ));
    }

    /*void OnCameraForwardChanged()
    {
        if (cam == null)
            cam = Camera.main;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        offsetPosition = GetProjectedPosition(ray) - cam.transform.position;
        offsetPosition.x -= Width * 0.5f;
        offsetPosition.y = 0f;
        offsetPosition.z -= Length * 0.5f;
    }*/

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            var camPoints = GetProjectedBounds();
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(camPoints.min, 1f);
            Gizmos.DrawSphere(camPoints.max, 1f);
        }
    }

    private void ProcessInput()
    {
        if (gameController.UIController.ShopWindow.IsOpened())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            IsFollowingPlayer = false;
            mouseOrigin = Input.mousePosition;
            isLMBDown = true;
        }

        if (Input.GetMouseButton(0))
        {
            var mouseDeltaPos = (mouseOrigin - Input.mousePosition);

            if (!isCameraPanning)
            {
                if (mouseDeltaPos.magnitude > 7f)
                    isCameraPanning = true;
            }

            if (isCameraPanning)
            {
                cam.transform.Translate(new Vector3(mouseDeltaPos.x, 0f, mouseDeltaPos.y) * PanSensitivity, Space.World);
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
        cam.transform.DOMove(mouseDeltaPos, 1.0f);
    }

    private void HandleScrollWheel(float scrollValue)
    {
        cam.transform.Translate(new Vector3(0f, 0f, scrollValue) * ScrollFactor, Space.Self);
    }
}
