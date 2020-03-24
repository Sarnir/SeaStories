using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : MonoBehaviour
{
    public Image HeroAvatar;
    public Text HeroName;
    public Text HeroClass;
    public Text HeroDescription;
    public CrewContent crewContent;
    public CrewInfoView CrewInfoPrefab;

    CrewContent skillsContent;
    Crew crew;

    private void OnEnable()
    {
        if(skillsContent == null)
        {
            skillsContent = GetComponentInChildren<CrewContent>();
        }

        crew = GameController.Instance.Player.GetComponent<Crew>();
        Refresh();
    }

    private void Refresh()
    {
        skillsContent.SetCrew(crew);
        skillsContent.Refresh();

        SetupListeners();
    }

    private void SetupListeners()
    {
        foreach (var element in crewContent.GetAllElements())
        {
            element.OnClick -= SelectHero;
            element.OnClick += SelectHero;
        }
    }

    public void SelectHero(UIClickableElement<CrewMember> hero)
    {
        HeroAvatar.sprite = hero.Data.Avatar;
        HeroName.text = hero.Data.Name;
        HeroClass.text = hero.Data.Class;
        HeroDescription.text = hero.Data.Description;
    }
}
