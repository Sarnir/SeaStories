using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{

    protected void Start ()
    {
        Setup();
    }
	
    protected virtual void Setup()
    {

    }

    public bool IsOpened()
    {
        return gameObject.activeInHierarchy;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        OnOpened();
    }

    protected virtual void OnOpened()
    {
    }
}
