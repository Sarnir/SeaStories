using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject CannonballPrefab;
    public float Force;
    ParticleSystem smokeFX;

    void Start()
    {
        smokeFX = GetComponentInChildren<ParticleSystem>();
    }

    public void Fire()
    {
        Debug.Log("KABOOM");

        smokeFX.Play();

        Invoke("SpawnCannonball", 0.2f);
    }

    void SpawnCannonball()
    {

        var ball = GameObject.Instantiate(CannonballPrefab, transform.position, Quaternion.identity);
        var f = transform.rotation.eulerAngles.normalized;
        ball.GetComponent<Rigidbody>().AddForce(transform.TransformVector(new Vector3(1f, 0f, 0f)) * Force, ForceMode.Impulse);

        Destroy(ball, 3f);
    }
}
