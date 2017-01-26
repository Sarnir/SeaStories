using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : UIWindow
{
    public ShopContent BuyContent;
    public ShopContent SellContent;
    public Text PlayerGoldText;
    public Text CityGoldText;

    Inventory playerInventory;
    Inventory shopInventory;

    ShopContent currentContent;

    ScrollRect scrollView;

	protected override void Setup ()
    {
        scrollView = GetComponentInChildren<ScrollRect>();

        BuyContent.OnShopItemClick += BuyItemFromShop;
        SellContent.OnShopItemClick += SellItemToShop;
	}

    void OnDestroy()
    {
        BuyContent.OnShopItemClick -= BuyItemFromShop;
        SellContent.OnShopItemClick -= SellItemToShop;
    }

    public void OpenShop(Inventory _playerInventory, City shop)
    {
        playerInventory = _playerInventory;
        shopInventory = shop.GetInventory();

        SetItemsToBuy(shop.GetItemsForSale());
        SetItemsToSell(shop.GetItemsToBuy());

        SetPlayerGold();
        SetCityGold();

        Open();
        SwitchToBuyContent(true);
    }

    public void SwitchToSellContent(bool force = false)
    {
        if (force || !SellContent.IsActive())
        {
            currentContent = SellContent;
            SellContent.SetActive(true);
            BuyContent.SetActive(false);
            RefreshContent();
        }
    }

    public void SwitchToBuyContent(bool force = false)
    {
        if (force || !BuyContent.IsActive())
        {
            currentContent = BuyContent;
            SellContent.SetActive(false);
            BuyContent.SetActive(true);
            RefreshContent();
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
        BuyContent.GenerateItemList(shopInventory, playerInventory, itemsToBuy);
    }

    void SetItemsToSell(ItemsCollection sellableItemsWithPrices)
    {
        SellContent.GenerateItemList(playerInventory, shopInventory, sellableItemsWithPrices);
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
