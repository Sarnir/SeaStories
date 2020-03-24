using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : UIWindow
{
    public InventoryView InventoryView;
    
    public void ToggleMenu(Inventory inventory)
    {
        InventoryView.RefreshInventory(inventory);
        if (IsOpened())
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
