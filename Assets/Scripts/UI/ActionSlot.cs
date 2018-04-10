using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour
{
    public Image ActionIcon;
    public Image CooldownFill;
    Button button;

    ShipController playerShip;

    public Skill skill; // skill assigned to slot

    float coolDown;

    private void Start()
    {
        button = GetComponent<Button>();
        playerShip = GameController.Instance.Player;

        if (skill != null)
            SetNewAction(skill);
    }

    private void Update()
    {
        if (skill == null)
            return;

        if(coolDown > 0f)
        {
            coolDown -= Time.deltaTime;

            CooldownFill.fillAmount = coolDown / skill.baseCooldown;

            if (coolDown <= 0f)
                ActionReady();
        }
    }

    void SetNewAction(Skill newSkill)
    {
        skill = newSkill;
        ActionIcon.sprite = skill.icon;
        ResetAction();
    }

    void ResetAction()
    {
        if(skill != null)
        {
            button.interactable = false;
            coolDown = skill.baseCooldown;
            CooldownFill.fillAmount = 1f;
        }
    }

    void ClearSlot()
    {
        skill = null;
        ActionIcon.sprite = null;
    }

    public void ActivateCurrentAction()
    {
        if (skill != null)
        {
            skill.Activate(playerShip);
            ResetAction();
        }
    }

    void ActionReady()
    {
        button.interactable = true;
        CooldownFill.fillAmount = 0f;
    }
}
