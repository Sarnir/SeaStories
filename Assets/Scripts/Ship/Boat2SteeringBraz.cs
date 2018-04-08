using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat2SteeringBraz : MonoBehaviour
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
        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            // fire!
			if(weapons != null)
            	weapons.FireCannons();
        }
    }
}
