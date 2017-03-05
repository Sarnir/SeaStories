using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    ParticleSystem particle;

    void Start ()
    {
        particle = GetComponent<ParticleSystem>();
	}
	
	void Update ()
    {
        if (!particle.IsAlive())
            Destroy(gameObject);
	}
}
