using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDamage : MonoBehaviour
{
    public float MaxHP;
    float CurrentHP;

    public ParticleSystem FireFXPrefab;
    public ParticleSystem Smoke1FX;
    public ParticleSystem Smoke2FX;

    void Start ()
    {
        CurrentHP = MaxHP;
	}

	void Update ()
    {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Cannonball cannonball = collision.gameObject.GetComponent<Cannonball>();

        if(cannonball != null)
        {
            HandleCannonballCollision(cannonball);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Cannonball cannonball = collider.gameObject.GetComponent<Cannonball>();

        if (cannonball != null)
        {
            HandleCannonballCollision(cannonball);
        }
    }

    void HandleCannonballCollision(Cannonball cannonball)
    {
        ApplyDamage(cannonball.GiveDamageAndDestroy(), cannonball.transform.position);
    }

    void ApplyDamage(float damage, Vector3 damagePos)
    {
        CurrentHP -= damage;
        Debug.Log("Current HP of " + gameObject.name + " is " + CurrentHP);

        var fireFX = Instantiate(FireFXPrefab, damagePos, Quaternion.identity);
        fireFX.Play();

        var hpRatio = GetHPRatio();
        if (hpRatio <= 0.66f && !Smoke1FX.IsAlive())
        {
            Smoke1FX.Play();
        }
        else if (hpRatio < 0.33f && !Smoke2FX.IsAlive())
        {
            Smoke1FX.Stop();
            Smoke2FX.Play();
        }
        else if (hpRatio <= 0f)
        {
            Smoke2FX.Stop();
            Sink();
        }
    }

    public float GetHPRatio()
    {
        return CurrentHP / MaxHP;
    }

    void Sink()
    {
        var rb = GetComponent<Rigidbody>();

        DG.Tweening.DOTween.To(() => rb.mass, x => rb.mass = x, 15f, 7f);
        var boatph = GetComponent<BoatPhysics>();
        boatph.CenterOfMass = new Vector3((Random.value - 1) * 2f, (Random.value - 1) * 2f, (Random.value - 1) * 2f);

        Destroy(gameObject, 15f);
    }
}
