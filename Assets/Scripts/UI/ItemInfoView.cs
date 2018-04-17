using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoView : MonoBehaviour
{
    public Text NameText;
    public Image Icon;
    public Text TypeText;
    public Text ValueText;
    public Text DescriptionText;
    
    public void SetItemInfo(ItemDefinition item)
    {
        NameText.text = item.NameString;
        Icon.sprite = item.Icon;
        DescriptionText.text = item.Description;
    }
}
