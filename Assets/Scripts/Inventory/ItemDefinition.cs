using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDefinition
{
    [SerializeField]
	public ItemName Name;
    [SerializeField]
    public Sprite Icon;

	public string NameString { get { return Name.ToString (); } }
    // also some stats this item gives or whatever
}

[System.Serializable]
public enum ItemName
{
	Gold,
	Food,
	Spices,
	Fish,
	Tobacco,
}