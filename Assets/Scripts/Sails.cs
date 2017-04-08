using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AngleCoefficient
{
	public float Angle;
	public float Value;
}

public class Sails : MonoBehaviour
{
	[SerializeField]
	AngleCoefficient[] dragCoefficient;
	[SerializeField]
	AngleCoefficient[] liftCoefficient;

    Cloth sailCloth;

    float sailArea;

	// Use this for initialization
	void Start ()
	{
		sailCloth = GetComponent<Cloth> ();
	}
	
    void Update()
    {
        sailCloth.externalAcceleration = WeatherController.Instance.GetWindVector();
    }

	public float GetArea()
    {
        return sailArea;
    }

    public Vector3 GetCenter()
    {
        return transform.position;
    }

	public void SpreadSails(bool max = false)
    {
		sailArea += 0.25f;

		if (sailArea >= 1f || max)
        {
            sailArea = 1f;
        }
    }

    public void ReefSails(bool max = false)
    {
		sailArea -= 0.25f;

        if(sailArea <= 0f || max)
        {
            sailArea = 0f;
        }
    }

	public float GetDragCoefficient(float angle)
	{
        angle = Utils.Math.ClampAngleBetween0360(angle);
        return GetAngleCoefficient (dragCoefficient, angle);
	}

	public float GetLiftCoefficient(float angle)
	{
        angle = Utils.Math.ClampAngleBetween0360(angle);
		return Mathf.Sign(180f - angle) * GetAngleCoefficient (liftCoefficient, angle);
	}

	protected float GetAngleCoefficient(AngleCoefficient[] coefficient, float angle)
	{
		if (coefficient == null || coefficient.Length < 2)
			return float.NaN;

        // angle is supposed to be <0;360> at this point
        if (angle > 180)
            angle = 360f - angle;

        AngleCoefficient last;
		AngleCoefficient current = coefficient[1];
        
		for (int i = 1; i < coefficient.Length; i++)
		{
			last = coefficient [i - 1];
			current = coefficient [i];
			if (angle >= last.Angle && angle <= current.Angle)
			{
				return Mathf.Lerp (last.Value, current.Value, (angle - last.Angle)/(current.Angle - last.Angle));
			}
		}

		return current.Value;
	}

    public Vector3 GetTrueWind()
    {
        return sailCloth.externalAcceleration;
    }
}
