using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrewInfoView : UIClickableElement<CrewMember>
{
    public Image Avatar;
    public Text NameText;
    public Text ClassText;
    
    public void SetCrewMember(CrewMember crewMember)
    {
        Data = crewMember;

        Avatar.sprite = crewMember.Avatar;
        NameText.text = crewMember.Name;
        ClassText.text = crewMember.Class;
    }
}
