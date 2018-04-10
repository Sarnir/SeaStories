using UnityEngine;

// for now only active skills are made
public class Skill : ScriptableObject
{
    public Sprite icon;

    public float baseCooldown; // in seconds

    public virtual void Activate(ShipController actor)
    {
        Debug.Log("Action is activated");
    }
}
