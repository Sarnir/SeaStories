using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Pickup : MonoBehaviour {

    GameController gameController;

    MeshRenderer meshRenderer;
    int emissionId;

	Color highlightColor = new Color (0.3f, 0.3f, 0.18f);

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

    public void OnCursorEnter()
    {
        Debug.Log("Pickup hovered on!");
		meshRenderer.material.DOColor (highlightColor, "_EmissionColor", 0.2f);
        //gameController.Player.SetDestination(transform.position);
    }

    public void OnCursorExit()
    {
		meshRenderer.material.DOColor (Color.black, "_EmissionColor", 0.2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            other.GetComponent<PlayerController>().TakePickup(this);
        }
    }
}
