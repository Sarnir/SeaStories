using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/Skills/SpeedUpSkill", menuName = "Skills/SpeedUpSkill")]
public class SpeedUpSkill : Skill
{
    public float modifier;
    
    [SerializeField]
    SkillDuration skillDuration;

    public override void Activate(ShipController actor)
    {
        Debug.Log("SPEED UUUUUP");

        actor.speedModifier = modifier;
        skillDuration.Start(() => { actor.speedModifier = 1f; });
    }
}
