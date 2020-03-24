using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    Vector3 steeringForce;
    float turningForce;
    Vector3 desiredVelocity;
    Vector3 desiredPosition;

    public float MaxSpeed;
    public float MaxTurnSpeed;

    public float TurnSpeed;

    public float SlowingRadius;

    public BoxCollider boxCollider;
    Rigidbody body;

    Vector3 desiredForward;

    void Start ()
    {
        body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        steeringForce = Vector3.zero;
        turningForce = 0f;

        _Arrive(desiredPosition, SlowingRadius);
        
        steeringForce.y = 0f;

        body.AddForce(steeringForce);
        body.AddTorque(new Vector3(0f, turningForce, 0f));

        //Debug.DrawRay(transform.position, steeringForce, Color.blue);
        //Debug.DrawRay(transform.position, desiredVelocity, Color.gray);
        //Debug.DrawRay(transform.position, body.velocity, Color.green);

        //Debug.DrawRay(transform.position, transform.up * 5f, Color.green);
        //Debug.DrawRay(transform.position, body.velocity * 5f, Color.blue);
        //Debug.DrawRay(transform.position, desiredForward * 5f, Color.red);
        //Debug.DrawRay(transform.position, steeringForce * 5f, Color.yellow);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, SlowingRadius);
    }

    public void Seek(Vector3 target)
    {
        desiredPosition = target;
    }

    void _Seek(Vector3 target)
    {
        desiredForward = target - transform.position;
        desiredForward.Normalize();

        float speed = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * Utils.Math.AngleSigned(transform.up, desiredForward)));

        turningForce = Mathf.Clamp(Utils.Math.AngleSigned(transform.up, desiredForward) * TurnSpeed,
            -MaxTurnSpeed, MaxTurnSpeed);

        steeringForce = transform.up * speed * MaxSpeed;
    }

    void _Arrive(Vector3 target, float radius)
    {
        var distanceFactor = Mathf.Clamp01(((target - transform.position).magnitude / radius));

        desiredForward = target - transform.position;
        desiredForward.Normalize();

        float speed = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * Utils.Math.AngleSigned(transform.up, desiredForward)));

        if (IsPointInsideObject(target))
        {
            speed = 0f;
            turningForce = 0f;
        }
        else
        {
            speed = speed * distanceFactor;
            turningForce = Mathf.Clamp(
                Utils.Math.AngleSigned(transform.up, desiredForward) * TurnSpeed,
                -MaxTurnSpeed, MaxTurnSpeed);
        }

        steeringForce = transform.up * speed * MaxSpeed;
    }

    bool IsPointInsideObject(Vector3 point)
    {
        if (boxCollider == null)
            return (transform.position - point).magnitude < 0.1f;

        return boxCollider.bounds.Contains(point);
    }
}
