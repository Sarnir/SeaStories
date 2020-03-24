using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewContent : UIContent<CrewInfoView, CrewMember>
{
    Crew crew;

    public void SetCrew(Crew _crew)
    {
        crew = _crew;
    }

    public override IEnumerable<CrewMember> GetAllDefinitions()
    {
        return crew.crewMembers;
    }

    protected override void SetupElement(CrewInfoView element, CrewMember definition)
    {
        element.SetCrewMember(definition);
    }
}
