using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewView : MonoBehaviour
{
    public CrewInfoView CrewInfoPrefab;

    CrewContent crewContent;
    Crew crew;

    private void OnEnable()
    {
        if(crewContent == null)
        {
            crewContent = GetComponentInChildren<CrewContent>();
        }

        crew = GameController.Instance.Player.GetComponent<Crew>();
        Refresh();
    }

    private void Refresh()
    {
        crewContent.SetCrew(crew);
        crewContent.Refresh();
    }

    public void SelectCrewMember(CrewMember crewMember)
    {

    }
}
