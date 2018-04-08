using UnityEditor;
using UnityEngine;

public class FireCannonAction : Action
{
    public float baseDmg;

    public override void Activate(ShipController actor)
    {
        Debug.Log("BOOM");

        actor.GetWeapons().FireCannons();
    }

    [MenuItem("Assets/Create/Actions/FireCannonAction")]
    public static void CreateAsset()
    {
        FireCannonAction action = ScriptableObject.CreateInstance<FireCannonAction>();

        AssetDatabase.CreateAsset(action, "Assets/ScriptableObjects/Actions/newFireCannonAction.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = action;
    }
}
