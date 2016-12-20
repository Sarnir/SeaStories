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
        if(currentMoveTween != null)
            currentMoveTween.Kill();

        destinationPosition = new Vector3(pos.x, transform.position.y, pos.y);
        var offsetPos = destinationPosition - transform.position;

        currentMoveTween = transform.DOMove(destinationPosition, offsetPos.magnitude / maxSpeed);
        var angle = Mathf.Atan2(offsetPos.y, offsetPos.x) * Mathf.Rad2Deg + 90;

        transform.DORotate(new Vector3(0, angle, 0), rotateSpeed);
    }
}
