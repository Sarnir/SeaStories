using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WorldGenerator worldGenerator = (WorldGenerator)target;

        if(DrawDefaultInspector() && worldGenerator.AutoUpdateInEditor)
        {
            worldGenerator.CreateWorldFromEditor();
        }

        if(GUILayout.Button("Create"))
        {
            worldGenerator.CreateWorldFromEditor();
        }
    }
}
