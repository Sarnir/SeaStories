using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField]
    Text nameText;
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
    
    public void SetItem(ItemDefinition itemDefinition, int quantity)
    {
        _definition = itemDefinition;
        _quantity = quantity;
        nameText.text = itemDefinition.Name;
        quantityText.text = "x" + quantity;
        itemImage.sprite = itemDefinition.Icon;
    }
}
