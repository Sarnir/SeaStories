using UnityEngine;

public class Action : ScriptableObject
{
    public Sprite icon;

    public float baseCooldown; // in seconds
    public float duration = 0f; // leave 0 if doesn't have duration

    public virtual void Activate(ShipController actor)
    {
        Debug.Log("Action is activated");
    }

    public virtual void OnDurationEnded()
    {
        Debug.Log("Action duration ended");
    }
}
