using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : UIItem
{
    [SerializeField]
    Text priceText;
    
    int _price;
    public int Price
    {
        get
        {
            return _price;
        }
    }
    
    public void SetItem(ItemDefinition itemDefinition, int quantity, int price)
    {
        base.SetItem(itemDefinition, quantity);
        _price = price;
        priceText.text = price + " geld";
    }

    public void SetTradeable(bool tradeable)
    {
        button.interactable = tradeable;
        priceText.color = tradeable ? Color.black : Color.red;
    }

    public void SetOnClick(UnityEngine.Events.UnityAction func)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(func);
    }
}
