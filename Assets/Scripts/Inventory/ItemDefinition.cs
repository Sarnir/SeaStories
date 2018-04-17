using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/Items/defaultItem", menuName = "Items/ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    [SerializeField]
	public ItemName Name;
    [SerializeField]
    public Sprite Icon;
    [SerializeField, TextArea]
    public string Description;

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
    ItemName_Max,
}