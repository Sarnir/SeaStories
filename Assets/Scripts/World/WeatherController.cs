using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Vector3 CurrentWind;
    public static WeatherController Instance;
    
    void Start ()
    {
        Instance = this;
	}

    void Update()
    {
    }

    public Vector3 GetTrueWind()
    {
        return CurrentWind;
    }
}
