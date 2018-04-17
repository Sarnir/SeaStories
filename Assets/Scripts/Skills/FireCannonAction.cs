using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/Skills/FireCannonSkill", menuName = "Skills/FireCannonSkill")]
public class FireCannonSkill : Skill
{
    public float baseDmg;

    public override void Activate(ShipController actor)
    {
        Debug.Log("BOOM");

        actor.GetWeapons().FireCannons();
    }
}
