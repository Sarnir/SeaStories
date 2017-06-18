using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AngleCoefficient
{
	public float Angle;
	public float Value;
}

[System.Serializable]
public enum PointOfSail
{
    InIrons,
    CloseHauled,
    CloseReach,
    BeamReach,
    BroadReach,
    Running,
    Length // used to check for number of elements in enum
}

[System.Serializable]
public enum RigType
{
    ForeAndAft,
    Square
}

public class Sails : MonoBehaviour
{
	[SerializeField]
	AngleCoefficient[] dragCoefficient;
	[SerializeField]
	AngleCoefficient[] liftCoefficient;
    [SerializeField]
    float sailAreaFactor;

    Cloth sailCloth;
    
    float sailArea;

    [SerializeField]
    Vector3 SailCenter;

    [SerializeField]
    ShipConfig shipConfig;

    public bool IsUsingConfig
    {
        get; private set;
    }

	// Use this for initialization
	void Awake ()
	{
        IsUsingConfig = (shipConfig != null);

		sailCloth = GetComponent<Cloth> ();
	}
	
    void Update()
    {
        sailCloth.externalAcceleration = WeatherController.Instance.GetTrueWind();
    }

	public float GetArea()
    {
        return sailArea * sailAreaFactor;
    }

    public Vector3 GetCenter()
    {
        return transform.position + SailCenter;
    }

    public void SpreadSailsFully()
    {
        sailArea = 1f;
    }

    public void SpreadSails()
    {
		sailArea += 0.25f;

		if (sailArea > 1f)
        {
            sailArea = 1f;
        }
    }

    public void ReefSailsFully()
    {
        sailArea = 0f;
    }

    public void ReefSails()
    {
		sailArea -= 0.25f;

        if(sailArea < 0f)
        {
            sailArea = 0f;
        }
    }

    public float GetSailForce(float angle)
    {
        // angle is supposed to be <0;360> at this point
        if (angle > 180)
            angle = 360f - angle;
        
        return GetArea() * shipConfig.SailCoefficientCurve.Evaluate(angle / 180f);
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

        if (angle < coefficient[0].Angle)
            return coefficient[0].Value;

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
