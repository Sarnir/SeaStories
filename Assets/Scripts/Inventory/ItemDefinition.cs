using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDefinition
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public Sprite Icon;
    // also some stats this item gives or whatever
}
