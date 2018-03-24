using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Cannonball CannonballPrefab;
    public float Force;
    ParticleSystem smokeFX;

    void Start()
    {
        smokeFX = GetComponentInChildren<ParticleSystem>();
    }

    public void Fire(float delay)
    {
        Debug.Log("KABOOM");

        Invoke("SpawnCannonball", delay);
    }

    void SpawnCannonball()
    {
        smokeFX.Play();
        Cannonball ball = Instantiate(CannonballPrefab, transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(transform.TransformVector(new Vector3(1f, 0f, 0f)) * Force, ForceMode.Impulse);

        Destroy(ball, 3f);
    }
}
