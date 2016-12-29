using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerShipController : MonoBehaviour
{
    public float rotateSpeed = 1f;
    public float maxSpeed = 2f;

    Vector3 destinationPosition;
    Tweener currentMoveTween;
   
    public void SailToPosition(Vector3 pos)
    {
        Debug.Log("Moving to pos " + pos);
        if(currentMoveTween != null)
            currentMoveTween.Kill();

        destinationPosition = new Vector3(pos.x, transform.position.y, pos.z);
        var offsetPos = destinationPosition - transform.position;
        var newDirection = offsetPos.normalized;
        Debug.Log("new direction = " + newDirection);
        // angle to direction
        // direction to angle
        // substract vectors before atan?

        currentMoveTween = transform.DOMove(destinationPosition, offsetPos.magnitude / maxSpeed);
        var angle = -Mathf.Atan2(newDirection.z, newDirection.x) * Mathf.Rad2Deg +90;
        Debug.Log("Angle set to " + angle);

        transform.DORotate(new Vector3(0, angle, 0), rotateSpeed);
    }
}
