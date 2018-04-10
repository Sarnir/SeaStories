using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviorHost : MonoBehaviour
{
    public static MonoBehaviorHost Instance;

    private void Start()
    {
        Instance = this;
    }
}
