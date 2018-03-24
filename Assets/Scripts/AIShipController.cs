using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class AIShipController : ShipController
{
	public Vector3 destination;
	public Vector3 destinationAcc;

    // angle between ship and target on wind axis 
    // at which the ship will come about while tacking
    // TODO: this should be dependent on ship type
    public float ComeAboutAngle = 20f;

    // what is the highest angle still considered dead for this vessel?
    // TODO: this should be dependent on ship type
    public float MaxDeadAngle = 25f;

    // how close does it needs to be to assume destination is met
    public float destinationPrecision = 1f;

    float desiredAngle;
    bool readyToComeAbout;

    Vector3 distanceToDestination
    {
        get { return transform.position - destination; }
    }

    float angleToTarget
    {
        get { return Utils.Math.AngleSigned(WeatherController.Instance.GetTrueWind(), transform.position - destination, Vector3.up); }
    }

    float angleOfAttack
    {
        get { return physics.GetAngleOfAttackSigned(); }
    }

    bool isInDeadZone
    {
        get { return Mathf.Abs(angleOfAttack) <= MaxDeadAngle; }
    }

    enum SailingStrategy
    {
        Anchor,
        SailStraight,
        Tack,
		AttackTarget
    }

    SailingStrategy currentSailStrategy;

    public void SetSail(Vector3 pos)
	{
		sails.SpreadSailsFully ();
		destination = pos;
        currentSailStrategy = SailingStrategy.SailStraight;
    }

	public void Attack()
	{
		sails.SpreadSailsFully ();
		currentSailStrategy = SailingStrategy.AttackTarget;
	}
    
    void Start()
    {
        SetSail(destination);
    }

    void Update()
    {
		// testing fleeing target
		destination += destinationAcc;

        if(currentSailStrategy != SailingStrategy.Tack && isInDeadZone)
        {
            Debug.Log("Starting tacking!");
            currentSailStrategy = SailingStrategy.Tack;
            desiredAngle = MaxDeadAngle + UnityEngine.Random.Range(10f, 20f);
            readyToComeAbout = true;
        }

        if(currentSailStrategy == SailingStrategy.Tack)
        {
            if (readyToComeAbout && Mathf.Abs(angleToTarget - (ComeAboutAngle * Mathf.Sign(desiredAngle))) < 1f)
            {
                readyToComeAbout = false;
                desiredAngle *= -1;
            }

            if (!readyToComeAbout && (angleOfAttack - desiredAngle) < 1f)
                readyToComeAbout = true;

            SailAtAngleToWind(desiredAngle);
        }
        else if(currentSailStrategy == SailingStrategy.SailStraight)
        {
            desiredAngle = 0f;
            SailDirectly(destination);
        }
		else if(currentSailStrategy == SailingStrategy.AttackTarget)
		{
			SailAtAngleToTarget (90f, destination);
		}
			
	}

    void StartTacking()
    {
        desiredAngle = Utils.Random.Sign() * MaxDeadAngle;
        Debug.Log("Desired angle = " + desiredAngle);
    }

    void Tack()
    {
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

    void SailDirectly(Vector3 target)
    {
        SailAtAngleToTarget(0f, target);
    }

    void SailAtAngleToWind(float targetAngle)
    {
        var angle = Utils.Math.AngleSigned(transform.up, WeatherController.Instance.GetTrueWind(), Vector3.up);
        angle = angle - (180f * Mathf.Sign(angle));

        Debug.Log("Angle to wind = " + angle);

        if (targetAngle - angle < -2f)
            rudder.SteerRight();
        else if (targetAngle - angle > 2f)
            rudder.SteerLeft();
    }

    void SailAtAngleToTarget(float targetAngle, Vector3 targetPos)
    {
        var angle = Utils.Math.AngleSigned(transform.up, targetPos - transform.position, Vector3.up);
        
        if (targetAngle - angle < -5f)
            rudder.SteerRight();
        else if (targetAngle - angle > 5f)
            rudder.SteerLeft();
    }

    void OnDrawGizmos()
	{
		Gizmos.DrawLine (transform.position, destination);
    }
}
