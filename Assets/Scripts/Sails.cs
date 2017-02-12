using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sails : MonoBehaviour
{
	public WindZone Wind;

	Cloth sailCloth;

	// Use this for initialization
	void Start ()
	{
		sailCloth = GetComponent<Cloth> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
