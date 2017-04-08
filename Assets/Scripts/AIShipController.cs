using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIShipController : ShipController
{
	public Vector3 destination;

	public void SailTo(Vector3 pos)
	{
		// keep close to desired angle
		// desired angles are based on ship specs

		sails.SpreadSails (true);
		destination = pos;
	}

	void Update()
	{
		sails.SpreadSails (true);
		if (destination != transform.position)
		{
			// rotate towards target
			// should be transform.forward here...
			var angle = Utils.Math.AngleSigned(transform.up, destination - transform.position, Vector3.up);
			if (angle > 1f)
				rudder.SteerRight ();
			else if (angle < -1f)
				rudder.SteerLeft ();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawLine (transform.position, destination);
	}
}
