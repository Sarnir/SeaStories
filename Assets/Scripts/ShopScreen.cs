using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : MonoBehaviour
{
    public ShopContent BuyContent;
    public ShopContent SellContent;
    public Text PlayerGoldText;
    public Text CityGoldText;

    Inventory playerInventory;
    Inventory shopInventory;

    ShopContent currentContent;

    Canvas canvas;
    ScrollRect scrollView;
	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
        scrollView = GetComponentInChildren<ScrollRect>();

        BuyContent.OnShopItemClick += BuyItemFromShop;
        SellContent.OnShopItemClick += SellItemToShop;
	}

    void OnDestroy()
    {
        BuyContent.OnShopItemClick -= BuyItemFromShop;
        SellContent.OnShopItemClick -= SellItemToShop;
    }

    public void CloseShop()
    {
        canvas.enabled = false;
    }

    public void OpenShop(Inventory _playerInventory, City shop)
    {
        playerInventory = _playerInventory;
        shopInventory = shop.GetInventory();

        SetItemsToBuy(shop.GetItemsForSale());
        SetItemsToSell(shop.GetItemsToBuy());

        SetPlayerGold();
        SetCityGold();

        canvas.enabled = true;
        SwitchToBuyContent(true);
    }

    public bool IsOpened()
    {
        return canvas.enabled;
    }

    public void SwitchToSellContent(bool force = false)
    {
        if (force || !SellContent.IsActive())
        {
            currentContent = SellContent;
            SellContent.SetActive(true);
            BuyContent.SetActive(false);
        }
    }

    public void SwitchToBuyContent(bool force = false)
    {
        if (force || !BuyContent.IsActive())
        {
            currentContent = BuyContent;
            SellContent.SetActive(false);
            BuyContent.SetActive(true);
        }
    }

    void SetPlayerGold()
    {
        PlayerGoldText.text = "Player geld: " + playerInventory.Gold;
    }

    void SetCityGold()
    {
        CityGoldText.text = "City geld: " + shopInventory.Gold;
    }

    public void SetItemsToBuy(ItemsCollection itemsToBuy)
    {
        BuyContent.GenerateItemList(shopInventory, itemsToBuy);
    }

    void SetItemsToSell(ItemsCollection sellableItemsWithPrices)
    {
        SellContent.GenerateItemList(playerInventory, sellableItemsWithPrices);
    }

    void BuyItemFromShop(ShopItem item)
    {
        shopInventory.SellItem(item, playerInventory);
        RefreshContent();
    }

    void SellItemToShop(ShopItem item)
    {
        playerInventory.SellItem(item, shopInventory);
        RefreshContent();
    }

    void RefreshContent()
    {
        currentContent.Refresh();
        SetPlayerGold();
        SetCityGold();
    }
}
