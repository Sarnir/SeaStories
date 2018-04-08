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

    public Action action; // current action

    float coolDown;

    private void Start()
    {
        button = GetComponent<Button>();
        playerShip = GameController.Instance.Player;

        if (action != null)
            SetNewAction(action);
    }

    private void Update()
    {
        if (action == null)
            return;

        if(coolDown > 0f)
        {
            coolDown -= Time.deltaTime;

            CooldownFill.fillAmount = coolDown / action.baseCooldown;

            if (coolDown <= 0f)
                ActionReady();
        }
    }

    void SetNewAction(Action newAction)
    {
        action = newAction;
        ActionIcon.sprite = action.icon;
        ResetAction();
    }

    void ResetAction()
    {
        if(action != null)
        {
            button.interactable = false;
            coolDown = action.baseCooldown;
            CooldownFill.fillAmount = 1f;
        }
    }

    void ClearSlot()
    {
        action = null;
        ActionIcon.sprite = null;
    }

    public void ActivateCurrentAction()
    {
        if (action != null)
        {
            action.Activate(playerShip);
            ResetAction();
        }
    }

    void ActionReady()
    {
        button.interactable = true;
        CooldownFill.fillAmount = 0f;
    }
}
