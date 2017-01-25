using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text priceText;
    [SerializeField]
    Text quantityText;
    [SerializeField]
    Image itemImage;

    ItemDefinition _definition;
    public ItemDefinition Definition
    {
        get
        {
            return _definition;
        }
    }

    int _price;
    public int Price
    {
        get
        {
            return _price;
        }
    }

    int _quantity;
    public int Quantity
    {
        get
        {
            return _quantity;
        }
    }

    Button _button;
    public Button button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();

            return _button;
        }
    }
    
    public void SetItem(ItemDefinition itemDefinition, int quantity, int price)
    {
        _definition = itemDefinition;
        _price = price;
        _quantity = quantity;
        nameText.text = itemDefinition.Name;
        priceText.text = price + " geld";
        quantityText.text = "x" + quantity;
        itemImage.sprite = itemDefinition.Icon;
    }

    public void SetTradeable(bool tradeable)
    {
        button.interactable = tradeable;
        priceText.color = tradeable ? Color.black : Color.red;
    }
}
