using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    GameController gameController;

    MeshRenderer meshRenderer;
    int emissionId;

	// Use this for initialization
	void Start ()
    {
        emissionId = Shader.PropertyToID("_EmissionColor");
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnMouseEnter()
    {
        Debug.Log("Pickup hovered on!");
        meshRenderer.material.SetColor(emissionId, Color.gray);
        //gameController.Player.SetDestination(transform.position);
    }

    private void OnMouseExit()
    {
        meshRenderer.material.SetColor(emissionId, Color.black);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            other.GetComponent<PlayerController>().TakePickup(this);
        }
    }
}
