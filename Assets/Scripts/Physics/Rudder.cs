using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : MonoBehaviour
{
    public float RudderCoefficient;
    public float TurningSpeed;
    public float MaxAngle;

    float Angle
    {
        get { return transform.localRotation.eulerAngles.z; }
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
        //if(Angle < MaxAngle)
            transform.Rotate(new Vector3(0f, 0f, TurningSpeed));
    }

    public void SteerRight()
    {
        //if(Angle > -MaxAngle)
            transform.Rotate(new Vector3(0f, 0f, -TurningSpeed));
    }

    void Update()
    {
        // dodać metodę na clampowanie stopni do określonej wartości do Math biblioteki
        //Angle = Mathf.SmoothStep(Angle, 0f, 1f -Mathf.Abs(Angle)/MaxAngle);
    }
}
