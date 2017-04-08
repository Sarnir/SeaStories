﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : MonoBehaviour
{
    public float RudderCoefficient;
    public float TurningSpeed;
    public float MaxAngle;
	public float MinTurningForce;

	Quaternion originRotation;

    float Angle
    {
        get
        {
            var angle = transform.localRotation.eulerAngles.z;
            if (angle > 180f)
                angle -= 360f;
            return angle;
        }
        set
        {
            transform.localRotation = Quaternion.Euler(
                transform.localRotation.eulerAngles.x,
                transform.localRotation.eulerAngles.y,
                value); 
        }
    }

    public float GetAngle()
    {
        return Angle;
    }

    public void SteerLeft()
    {
        if(Angle < MaxAngle)
            transform.Rotate(new Vector3(0f, 0f, TurningSpeed));
    }

    public void SteerRight()
    {
        if(Angle > -MaxAngle)
            transform.Rotate(new Vector3(0f, 0f, -TurningSpeed));
    }

	public void Reset()
	{
		transform.rotation = originRotation;
	}

	void Start()
	{
		originRotation = transform.rotation;
	}

    void Update()
    {
        // dodać metodę na clampowanie stopni do określonej wartości do Math biblioteki
        if (Mathf.Abs(Angle) != 0f)
        {
            Angle -= (Angle / MaxAngle) * TurningSpeed;
        }
    }
}