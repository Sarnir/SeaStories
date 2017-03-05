using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField]
    float Damage;
    float lowestVertHeight;

    bool destroy;

    public ParticleSystem SplashFXPrefab;

	void Start ()
    {
        destroy = false;

        var meshFilter = GetComponent<MeshFilter>();

        lowestVertHeight = (meshFilter.mesh.vertices[0]).magnitude * transform.localScale.y;
	}
	
	void Update ()
    {
        // check collision with water
        if((transform.position.y - lowestVertHeight) < WaterController.Instance.GetWaterYPos(transform.position))
        {
            Instantiate(SplashFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }  
    }

    public float GiveDamageAndDestroy()
    {
        Destroy(gameObject);

        return Damage;
    }
}
