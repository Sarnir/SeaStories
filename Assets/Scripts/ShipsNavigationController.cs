using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages routing of every AI ship that is currently "spawned".
/// Vessels that are in the world, but are not seen by player are moved around by this class.
/// </summary>
public class ShipsNavigationController : MonoBehaviour
{
    static public ShipsNavigationController Instance;

    List<AIShipController> allShips;

	void Start ()
    {
        Instance = this;
        allShips = new List<AIShipController>();
	}
	
	void Update ()
    {
		// lista wszystkich statków
        // ich cele
        // ich aktualna pozycja?
        // ich parametry?
	}
}
