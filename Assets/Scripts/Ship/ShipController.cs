using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipController : MonoBehaviour
{
    public ShipConfig shipConfig;

    protected Inventory inventory;
    protected Sails sails;
	protected Rudder rudder;
    protected BoatPhysics physics;
    protected ShipWeapons weapons;

	protected Vector3 destination;
	protected Rigidbody rigidBody;

    public float speedModifier;

    void Awake()
    {
        sails = GetComponentInChildren<Sails> ();
		rudder = GetComponentInChildren<Rudder> ();
        physics = GetComponentInChildren<BoatPhysics>();
		rigidBody = GetComponent<Rigidbody> ();
        weapons = GetComponentInChildren<ShipWeapons>();

		destination = transform.position;

        speedModifier = 1f;
    }

	void FixedUpdate()
	{
		ApplyRudderForces ();
		Sail ();
	}

    public ShipWeapons GetWeapons()
    {
        return weapons;
    }

	public void SetupInventory(int _maxSpace, ItemsDictionary startingItems)
	{
		inventory = new Inventory ();
		inventory.Setup(_maxSpace, startingItems);
	}

	protected void Sail ()
	{
        if (Vector3.Distance(destination, transform.position) > 1.5f)
        {
            var angle = Utils.Math.AngleSigned(transform.up, transform.position - destination);

            if (angle < 5f && angle > -5f && (rudder.Angle > 20f || rudder.Angle < -20f))
                ;// don't turn anymore, let it gooooooo
            else if (angle < -1f)
                rudder.SteerRight();
            else if (angle > 1f)
                rudder.SteerLeft();

            // calculate force pushing the boat
            // it should depend on the wind and boat params
            var forwardForce = (10f - rigidBody.velocity.magnitude) * speedModifier
                * transform.up * shipConfig.GetSailForce(GetAngleOfAttack());
            forwardForce.y = 0f;

            rigidBody.AddForce(forwardForce);
            //Debug.Log("velocity: " + rigidBody.velocity.magnitude);
        }
        else
            DestinationReached();
	}

    public float GetAngleOfAttack()
    {
        // TODO: ogarnąć, żeby używać tutaj poprawnego wektora kierunku :D
        var angleOfAttack = 180f + Utils.Math.AngleSigned(transform.up, GetApparentWindForce(), Vector3.up); // Vector3.Angle (transform.up, apparentWindForce);
        //myLog = "Angle of attack: " + angleOfAttack;

        return angleOfAttack;
    }

    public Vector3 GetApparentWindForce()
    {
        return WeatherController.Instance.GetTrueWind() - rigidBody.velocity;
    }

    // copied from BoatPhysics
    void ApplyRudderForces()
	{
		var vel = rigidBody.velocity;
		vel.y = 0f;
		var velocityParam = Mathf.Clamp01(vel.magnitude);

		//Debug.Log("Velocity param = " + velocityParam);

		float force = velocityParam * rudder.RudderCoefficient * Mathf.Sin(Mathf.Deg2Rad * -rudder.Angle);

		rigidBody.AddTorque(0f, force, 0f);
	}

    protected void StopShip()
    {
        sails.ReefSailsFully();
		rudder.Reset ();
    }

	public virtual void SetDestination (Vector3 _destination)
	{
		destination = _destination;
	}

    protected virtual void DestinationReached()
    {
        destination = transform.position;
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
