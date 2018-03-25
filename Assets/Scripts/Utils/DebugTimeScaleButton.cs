using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTimeScaleButton : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Time -"))
            Time.timeScale *= 0.5f;

        if (GUI.Button(new Rect(70, 70, 50, 30), "Time +"))
            Time.timeScale *= 2f;
    }
}
