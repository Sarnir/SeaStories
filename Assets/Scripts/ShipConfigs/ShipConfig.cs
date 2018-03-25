using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShipConfig : ScriptableObject
{
    [System.Serializable]
    public class ShipConfigData
    {
        // max angle for given PoS will be (180 / enum length) * index
        public PointOfSail pointOfSail;

        [Range(0f, 2f)]
        public float coefficient;
    }

    public RigType RigType;
    //public ShipConfigData[] PointsOfSail = new ShipConfigData[(int)PointOfSail.Length];
    public AnimationCurve SailCoefficientCurve;

    public float GetSailForce(float angleToWind)
    {
        // angle is supposed to be <0;360> at this point
        if (angleToWind > 180)
            angleToWind = 360f - angleToWind;

        return SailCoefficientCurve.Evaluate(angleToWind / 180f);
    }
}


public class ShipConfigCreator
{
    [MenuItem("Assets/Create/ShipConfig")]
    public static void CreateAsset()
    {
        ShipConfig shipConfig = ScriptableObject.CreateInstance<ShipConfig>();

        AssetDatabase.CreateAsset(shipConfig, "Assets/ShipConfigs/newShipConfig.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = shipConfig;
    }
}
