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

	// Use this for initialization
	void Start ()
	{
		sailCloth = GetComponent<Cloth> ();
	}
	
	public float GetArea()
    {
        return 1f;
    }

	public float GetDragCoefficient(float angle)
	{
		return GetAngleCoefficient (dragCoefficient, angle);
	}

	public float GetLiftCoefficient(float angle)
	{
		return GetAngleCoefficient (liftCoefficient, angle);
	}

	protected float GetAngleCoefficient(AngleCoefficient[] coefficient, float angle)
	{
		if (coefficient == null || coefficient.Length < 2)
			return float.NaN;

		while (angle < 0)
			angle += 360f;

		while (angle > 360)
			angle -= 360f;

		// angle is supposed to be 0-360 at this point
		AngleCoefficient last;
		AngleCoefficient current = coefficient[1];
		for (int i = 1; i < coefficient.Length; i++)
		{
			last = coefficient [i - 1];
			current = coefficient [i];
			if (angle >= last.Angle && angle <= current.Angle)
			{
				return Mathf.Lerp (last.Value, current.Value, angle/current.Angle);
			}
		}

		return current.Value;
	}

    public Vector3 GetTrueWind()
    {
        return sailCloth.externalAcceleration;
    }
}
