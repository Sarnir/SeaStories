using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Pickup : MonoBehaviour {

    GameController gameController;
    
    Inventory inventory;

    MeshRenderer meshRenderer;
    int emissionId;

	Color highlightColor = new Color (0.3f, 0.3f, 0.18f);

	// Use this for initialization
	void Start ()
    {
        emissionId = Shader.PropertyToID("_EmissionColor");
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        meshRenderer = GetComponent<MeshRenderer>();

        // give some random gold and items
        inventory = new Inventory();
        inventory.Setup(1000);

        int itemsCount = Random.Range(1, 4);
        for(int i = 0; i < itemsCount; i++)
        {
            inventory.AddItems((ItemName)Random.Range(0, (int)ItemName.ItemName_Max), (uint)Random.Range(1, 100));
        }
    }
	
	public void OnPickup(Inventory taker)
    {
        Debug.Log("Picked up: ");
        var inventoryString = inventory.Print();
        inventory.TransferTo(taker);

        gameController.UIController.WorldUI.SpawnText(GetCenterPos(), inventoryString);

        Destroy(gameObject);
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
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().TakePickup(this);
        }
    }

    public Vector3 GetCenterPos()
    {
        return meshRenderer.bounds.center;
    }
}
