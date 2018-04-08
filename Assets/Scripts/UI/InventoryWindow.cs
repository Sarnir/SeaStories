using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : UIWindow
{
    public InventoryContent Content;
    public Text PlayerGoldText;
    public Text CapacityText;

    Inventory playerInventory;
    
    ScrollRect scrollView;

	protected override void Setup ()
    {
        scrollView = GetComponentInChildren<ScrollRect>();
	}

    public void ToggleInventory(Inventory _inventory)
    {
        playerInventory = _inventory;
        if (IsOpened())
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    protected override void OnOpened()
    {
        Content.GenerateItemList(playerInventory);

        SetPlayerGold();
        SetCapacityText();
    }

    void SetPlayerGold()
    {
        PlayerGoldText.text = "Player geld: " + playerInventory.GetGold();
    }

    void SetCapacityText()
    {
        CapacityText.text = "Capacity: " + playerInventory.GetCurrentCapacity() +
            " / " + playerInventory.GetMaxCapacity();
    }
}
