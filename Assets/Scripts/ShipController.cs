using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipController : MonoBehaviour
{
    protected Inventory inventory;
    protected Sails sails;
	protected Rudder rudder;

    void Start()
    {
        sails = GetComponentInChildren<Sails> ();
		rudder = GetComponentInChildren<Rudder> ();
    }

	public void SetupInventory(ItemsDictionary startingItems)
	{
		inventory = new Inventory ();
		inventory.Setup();
		inventory.AddItems (startingItems);
	}

    protected void StopShip()
    {
        sails.ReefSails(true);
		rudder.Reset ();
    }

	public Inventory GetInventory()
	{
		return inventory;
	}

    public virtual void EnterCity(City city)
    {
        StopShip();
    }

    public virtual void LeaveCity()
    {
        transform.Rotate(0f, 0f, 180f);
    }
}
