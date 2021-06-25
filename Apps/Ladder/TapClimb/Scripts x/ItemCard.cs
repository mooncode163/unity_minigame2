using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    ShopItem shopItem;
    public Image imageContainer;
    public Button button;
    public GameObject lockObj;
    public Transform applyPos;


    public void SetShopItem(ShopItem shopItem)
    {
        this.shopItem = shopItem;

        RefreshItemData();
    }

    public void RefreshItemData()
    {
        if (shopItem.unlocked)
        {
            //button.interactable = true;
            lockObj.SetActive(false);
        }
        else
        {
            //LOCK
            //button.interactable = false;
            lockObj.SetActive(true);
        }

        imageContainer.sprite = shopItem.image;
    }

    public void SetShirt()
    {
        if (shopItem.unlocked)
        {
            FindObjectOfType<ShopSystem>().DisplayApplyMenu(applyPos, shopItem);
            FindObjectOfType<GameManager>().audioManager.PlayClick1();
        }
        else
        {
            WatchAdToUnlock();
        }
    }

    public void WatchAdToUnlock()
    {
        ADManager ad = FindObjectOfType<ADManager>();

        ad.itemToUnlock = this;

        ad.Display_RewardVideoAd();
    }

    public void Unlock()
    {
        PlayerPrefsExtra.SetBool(shopItem.id.ToString(), true);
        shopItem.unlocked = true;
        RefreshItemData();
        SetShirt();
    }
}
