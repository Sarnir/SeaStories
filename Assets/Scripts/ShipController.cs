using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipController : MonoBehaviour
{
    protected Inventory inventory;
    protected Sails sails;
	protected Rudder rudder;
    protected BoatPhysics physics;

	protected Vector3 destination;
	protected Rigidbody rigidBody;

    void Awake()
    {
        sails = GetComponentInChildren<Sails> ();
		rudder = GetComponentInChildren<Rudder> ();
        physics = GetComponentInChildren<BoatPhysics>();
		rigidBody = GetComponent<Rigidbody> ();

		destination = transform.position;
    }

	void FixedUpdate()
	{
		ApplyRudderForces ();
		Sail ();
	}

	public void SetupInventory(ItemsDictionary startingItems)
	{
		inventory = new Inventory ();
		inventory.Setup();
		inventory.AddItems (startingItems);
	}

	protected void Sail ()
	{
		if (Vector3.Distance (destination, transform.position) > 1f)
		{
			rudder.SteerLeft ();

			// calculate force pushing the boat
			// it should depend on the wind and boat params
			var forwardForce = (5f - rigidBody.velocity.magnitude) * transform.up;
			forwardForce.y = 0f;

			rigidBody.AddForce (forwardForce);
		}
	}


	// copied from BoatPhysics
	void ApplyRudderForces()
	{
		var vel = rigidBody.velocity;
		vel.y = 0f;
		var velocityParam = Mathf.Clamp01(vel.magnitude);

		//Debug.Log("Velocity param = " + velocityParam);

		float force = velocityParam * rudder.RudderCoefficient * Mathf.Sin(Mathf.Deg2Rad * -rudder.GetAngle());

		rigidBody.AddTorque(0f, force, 0f);
	}

    protected void StopShip()
    {
        sails.ReefSailsFully();
		rudder.Reset ();
    }

	public void SetDestination (Vector3 _destination)
	{
		destination = _destination;
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
