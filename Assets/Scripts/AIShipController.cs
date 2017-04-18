using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIShipController : ShipController
{
	public Vector3 destination;
    bool isDestinationUpwind;

    // angle between ship and target on wind axis 
    // at which the ship will come about while tacking
    public float ComeAboutAngle = 30f;

    // TODO: this should be dependent on ship type
    public float MaxDeadAngle = 30f;

    float desiredAngle;

    public void SailTo(Vector3 pos)
	{
		// keep close to desired angle
		// desired angles are based on ship specs

		sails.SpreadSails (true);
		destination = pos;
        isDestinationUpwind = true;
	}

    void Awake()
    {
        isDestinationUpwind = true;
        StartTacking();
    }

	void Update()
	{
		sails.SpreadSails (true);
		if (destination != transform.position)
		{
            if (isDestinationUpwind)
                Tack();
            else
                SailDirectly();
		}
	}

    void StartTacking()
    {
        desiredAngle = Utils.Random.Sign() * MaxDeadAngle;
        Debug.Log("Desired angle = " + desiredAngle);
    }

    void Tack()
    {
        var angleOfAttack = physics.GetAngleOfAttackSigned();
        var angleToTarget = Utils.Math.AngleSigned(sails.GetTrueWind(), transform.position - destination, Vector3.up);

        Debug.Log("Angle of attack =  " + angleOfAttack + ", angle to target = " + angleToTarget);

        if (angleOfAttack < desiredAngle)
            rudder.SteerLeft();
        else if (angleOfAttack > desiredAngle)
            rudder.SteerRight();

        if (Mathf.Abs(angleToTarget) > ComeAboutAngle && Mathf.Sign(angleOfAttack) == Mathf.Sign(desiredAngle))
        {
            // come about!
            desiredAngle = -desiredAngle;
            Debug.Log("Desired angle = " + desiredAngle);
        }
    }

    void SailDirectly()
    {
        // rotate towards target
        // should be transform.forward here...
        var angle = Utils.Math.AngleSigned(transform.up, destination - transform.position, Vector3.up);
        if (angle > 1f)
            rudder.SteerRight();
        else if (angle < -1f)
            rudder.SteerLeft();
    }

	void OnDrawGizmos()
	{
		Gizmos.DrawLine (transform.position, destination);
    }
}
