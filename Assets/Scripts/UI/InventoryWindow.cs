using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : UIWindow
{
    public InventoryContent Content;
    public Text PlayerGoldText;

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
    }

    void SetPlayerGold()
    {
        PlayerGoldText.text = "Player geld: " + playerInventory.Gold;
    }
}
