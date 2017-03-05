using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapons : MonoBehaviour
{
    Cannon[] Cannons;
    public float ReloadCooldown;

    float currentCooldown;

	// Use this for initialization
	void Start ()
    {
        currentCooldown = 0f;

        Cannons = GetComponentsInChildren<Cannon>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentCooldown -= Time.deltaTime;
	}

    public void FireCannons()
    {
        if (currentCooldown > 0f)
            return;
        
        for (int i = 0; i < Cannons.Length; i++)
        {
            Cannons[i].Fire(Random.value);
        }

        currentCooldown = ReloadCooldown;
    }
}
