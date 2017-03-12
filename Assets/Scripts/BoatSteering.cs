using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteering : MonoBehaviour
{
    Rudder rudder;
    Sails sails;
    ShipWeapons weapons;

	void Start ()
    {
        rudder = GetComponentInChildren<Rudder>();
        sails = GetComponentInChildren<Sails>();
        weapons = GetComponent<ShipWeapons>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rudder.SteerLeft();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rudder.SteerRight();
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // sails up
            sails.SpreadSails();
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // sails down
            sails.ReefSails();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            // fire!
            weapons.FireCannons();
        }
    }
}
