using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInputHandler : MonoBehaviour
{
    [SerializeField]
    Slider speedSlider;
    [SerializeField]
    Slider scaleSlider;
    [SerializeField]
    Slider heightSlider;

    [SerializeField]
    Transform cube;

    [SerializeField]
    float cameraSpeed = 0.01f;

    float speedTarget;
    float scaleTarget;
    float heightTarget;

    WaterController wc;

    [SerializeField]
    Transform cameraTransform;

    void Start()
    {
        wc = FindObjectOfType<WaterController>();

        speedTarget = wc.waveSpeed;
        scaleTarget = wc.waveScale;
        heightTarget = wc.waveHeight;

        speedSlider.onValueChanged.AddListener(value => speedTarget = value);
        scaleSlider.onValueChanged.AddListener(value => scaleTarget = value);
        heightSlider.onValueChanged.AddListener(value => heightTarget = value);
    }

    void Update()
    {
        wc.waveSpeed = speedTarget;
        wc.waveScale = scaleTarget;
        wc.waveHeight = heightTarget;

        if (Input.GetKeyUp(KeyCode.Y))
            cube.position += new Vector3(0, 5, 0);

        if (Input.GetKey(KeyCode.UpArrow))
            cameraTransform.position += new Vector3(0f, cameraSpeed, 0f);
        if (Input.GetKey(KeyCode.DownArrow))
            cameraTransform.position += new Vector3(0f, -cameraSpeed, 0f);
        if (Input.GetKey(KeyCode.LeftArrow))
            cameraTransform.position += new Vector3(-cameraSpeed, 0f, 0f);
        if (Input.GetKey(KeyCode.RightArrow))
            cameraTransform.position += new Vector3(cameraSpeed, 0f, 0f);

        if (Input.GetKey(KeyCode.I))
            cameraTransform.Rotate(new Vector3(cameraSpeed * 5f, 0f, 0f));
        if (Input.GetKey(KeyCode.K))
            cameraTransform.Rotate(new Vector3(-cameraSpeed * 5f, 0f, 0f));
        if (Input.GetKey(KeyCode.L))
            cameraTransform.Rotate(new Vector3(0f, cameraSpeed * 5f, 0f));
        if (Input.GetKey(KeyCode.J))
            cameraTransform.Rotate(new Vector3(0f, -cameraSpeed * 5f, 0f));
    }

    private void FixedUpdate()
    {
        
    }
}
