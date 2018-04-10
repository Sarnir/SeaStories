using UnityEditor;
using UnityEngine;

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

    [MenuItem("Assets/Create/Skills/SpeedUpSkill")]
    public static void CreateAsset()
    {
        SpeedUpSkill action = ScriptableObject.CreateInstance<SpeedUpSkill>();

        AssetDatabase.CreateAsset(action, "Assets/ScriptableObjects/Skills/newSpeedUpSkill.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = action;
    }
}
