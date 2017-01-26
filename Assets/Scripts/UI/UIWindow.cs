using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    Canvas canvas;
    // Use this for initialization
    protected void Start () {
        canvas = GetComponent<Canvas>();

        Setup();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void Setup()
    {

    }

    public bool IsOpened()
    {
        return canvas.enabled;
    }

    public void Close()
    {
        canvas.enabled = false;
    }

    public void Open()
    {
        canvas.enabled = true;
        OnOpened();
    }

    protected virtual void OnOpened()
    {
    }
}
