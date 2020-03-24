using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public InventoryContent Content;
    public Text PlayerGoldText;
    public Text CapacityText;

    Inventory playerInventory;
    
    ScrollRect scrollView;

	protected void Setup ()
    {
        scrollView = GetComponentInChildren<ScrollRect>();
	}

    public void RefreshInventory(Inventory _inventory)
    {
        playerInventory = _inventory;

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
