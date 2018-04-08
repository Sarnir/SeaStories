using UnityEditor;
using UnityEngine;

public class SpeedUpAction : Action
{
    public float modifier;

    public override void Activate(ShipController actor)
    {
        Debug.Log("SPEED UUUUUP");

        actor.speedModifier = modifier;
    }

    [MenuItem("Assets/Create/Actions/SpeedUpAction")]
    public static void CreateAsset()
    {
        SpeedUpAction action = ScriptableObject.CreateInstance<SpeedUpAction>();

        AssetDatabase.CreateAsset(action, "Assets/ScriptableObjects/Actions/newSpeedUpAction.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = action;
    }
}
