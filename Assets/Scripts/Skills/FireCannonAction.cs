using UnityEditor;
using UnityEngine;

public class FireCannonSkill : Skill
{
    public float baseDmg;

    public override void Activate(ShipController actor)
    {
        Debug.Log("BOOM");

        actor.GetWeapons().FireCannons();
    }

    [MenuItem("Assets/Create/Skills/FireCannonSkill")]
    public static void CreateAsset()
    {
        FireCannonSkill skill = ScriptableObject.CreateInstance<FireCannonSkill>();

        AssetDatabase.CreateAsset(skill, "Assets/ScriptableObjects/Skills/newFireCannonSkill.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = skill;
    }
}
