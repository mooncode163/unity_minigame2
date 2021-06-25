using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using DG.Tweening;

public class ShopSystem : MonoBehaviour
{
    public ScrollSnap scrollSnap;
    int currentPage=0;
    public Transform listItems;

    public GameObject itemPrefab;
    public Transform applyMenu;

    public PlayerCustomize customize;

    ShopItem selectedItem = null;

    public GameObject blockScroll;

    public GameManager gameManager;



    [Header("Make sure that the -id- are UNIQUE")]
    [SerializeField] public List<ShopItem> items;

    

    

    private void Start()
    {
        scrollSnap.onPageChange += PageChanger;

        SpawnItems();

        SetShirtFromData();
        //currentPage = scrollSnap.CurrentPage();
        //ScaleUpItem(listItems.GetChild(currentPage).GetChild(0));
    }

    public void SpawnItems()
    {
        //clean item list
        foreach (Transform child in listItems)
        {
            Destroy(child.gameObject);
        }

        //Spawn
        for (int i = 0; i < items.Count; i++)
        {
            ItemCard item = Instantiate(itemPrefab, listItems).GetComponent<ItemCard>();
            
            if (items[i].unlocked)
            {
                PlayerPrefsExtra.SetBool(items[i].id.ToString(), true);
            }
            else
            {
                items[i].unlocked = PlayerPrefsExtra.GetBool(items[i].id.ToString());
            }

            item.SetShopItem(items[i]);
        }
    }

    public void PageChanger(int page)
    {
        if (currentPage != page)
        {
            //Debug.Log("PAGE: " + page);
            ScaleNormalItem(listItems.GetChild(currentPage).GetChild(0));
            currentPage = page;
            ScaleUpItem(listItems.GetChild(currentPage).GetChild(0));
            gameManager.audioManager.PlayHighlite();
        }
        
    }


    public void ScaleUpItem(Transform item)
    {
        item.DOScale(Vector3.one * 1.65f, .2f);
    }
    public void ScaleNormalItem(Transform item)
    {
        item.DOScale(Vector3.one, .2f);
    }

    public void DisplayShopPanel()
    {
        //scrollSnap.ChangePage(currentPage);
        scrollSnap.ChangePage(currentPage + 1);
        scrollSnap.ChangePage(currentPage - 1);
    }

    public void DisplayApplyMenu(Transform pos,ShopItem item)
    {
        applyMenu.position = pos.position;
        applyMenu.DOScale(Vector3.one, .2f);
        blockScroll.SetActive(true);
        
        selectedItem = item;
    }

    public void HideApplyMenu()
    {
        applyMenu.DOScale(Vector3.zero, .15f);
        selectedItem = null;
        blockScroll.SetActive(false);
    }

    public void ApplyForLeft()
    {
        customize.SetShirtToLeftHand(selectedItem.material);
        PlayerPrefs.SetString("LeftShirt", selectedItem.id.ToString());
        HideApplyMenu();
    }
    public void ApplyForRight()
    {
        customize.SetShirtToRightHand(selectedItem.material);
        PlayerPrefs.SetString("RightShirt", selectedItem.id.ToString());
        HideApplyMenu();
    }
    public void ApplyForBoth()
    {
        customize.SetShirtToBothHands(selectedItem.material);
        PlayerPrefs.SetString("LeftShirt", selectedItem.id.ToString());
        PlayerPrefs.SetString("RightShirt", selectedItem.id.ToString());
        HideApplyMenu();
    }

    public void SetShirtFromData()
    {
        string lShirt = PlayerPrefs.GetString("LeftShirt");
        string rShirt = PlayerPrefs.GetString("RightShirt");
        foreach (ShopItem item in items)
        {
            if (item.id.Equals(lShirt))
            {
                selectedItem = item;
                ApplyForLeft();
            }
            if (item.id.Equals(rShirt))
            {
                selectedItem = item;
                ApplyForRight();
            }
        }
        selectedItem = null;
    }
}

[System.Serializable]
public class ShopItem
{
    public string id;
    public bool unlocked = false;
    public Sprite image;
    public Material material;
}
