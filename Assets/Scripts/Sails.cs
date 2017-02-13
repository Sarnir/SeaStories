using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sails : MonoBehaviour
{
    public float DragCoefficient;
    public float LiftCoefficient;
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

    public Vector3 GetTrueWind()
    {
        return sailCloth.externalAcceleration;
    }
}
