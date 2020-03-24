using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/Characters/newCrewMember", menuName = "Characters/CrewMember")]
public class CrewMember : ScriptableObject
{
    public Sprite Avatar;
    public string Name;
    public string Class;
    [TextArea]
    public string Description;
}
