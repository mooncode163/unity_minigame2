using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIOptionImageSelect : UIViewPop, ITableViewDataSource
{
    public const string STR_KEYNAME_VIEWALERT_NOAD = "STR_KEYNAME_VIEWALERT_NOAD";
    public const string STR_KEYNAME_VIEWALERT_LOADING = "STR_KEYNAME_VIEWALERT_LOADING";
    public const string STR_KEYNAME_VIEWALERT_UPDATE_VERSION = "STR_KEYNAME_VIEWALERT_UPDATE_VERSION";
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public GameObject objTableViewTemplate;
    public TableView tableView;
    public UIText textTitle;
    public UIImage imageBar;
    public UIButton btnBack;
    // public Image imageBg;
    public int numRows;
    private int numInstancesCreated = 0;


    public List<object> listItem;

    int oneCellNum;
    int heightCell;
    int totalItem;
    public void Awake()
    {
        heightCell = 320;
        LoadPrefab();

        UpdateItem();
        oneCellNum = 3;
        if(Device.isLandscape)
        {
            oneCellNum = 4;
        }
        totalItem = listItem.Count;
        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }
        tableView.dataSource = this;




    }

    // Use this for initialization
    public void Start()
    {
        if (this.controller != null)
        {
            if (this.controller.naviController != null)
            {
                btnBack.gameObject.SetActive(false);
            }
        }
        LayOut();

        OnUIDidFinish(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }


    }


    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

    }

    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.LoadByKey("UIOptionImageSelectCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }


    public void OnClickBtnBack()
    {
        // PopViewController pop = (PopViewController)this.controller;
        // if (pop != null)
        // {
        //     pop.Close();
        // }
        Close();
        UIGameMerge.main.gameStatus = UIGameMerge.Status.Play;
    }
    public void OnUILanguageDidClose()
    {
        int tag = PlayerPrefs.GetInt(AppString.STR_KEY_LANGUAGE);
        SystemLanguage lan = (SystemLanguage)tag;
        Language.main.SetLanguage(lan, true);

        {
            string str = Language.main.GetString(AppString.STR_SETTING);
            textTitle.text = str;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            RectTransform rctran = imageBar.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + fontsize + oft * 2;
            rctran.sizeDelta = sizeDelta;
        }
        UpdateItem();
        tableView.ReloadData();

    }
    public virtual void UpdateItem()
    {
        listItem = GameLevelParse.main.listGameItemDefault;

    }

    public void OnCellItemDidClick(UICellItemBase item)
    {

    }

    #region ITableViewDataSource

    //Will be called by the TableView to know how many rows are in this table
    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        return numRows;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
        return heightCell;
        //return (cellPrefab.transform as RectTransform).rect.height;
    }

    //Will be called by the TableView when a cell needs to be created for display
    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        UICellBase cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as UICellBase;
        if (cell == null)
        {
            cell = (UICellBase)GameObject.Instantiate(cellPrefab);
            cell.name = "UICellBase" + (++numInstancesCreated).ToString();
            Rect rccell = (cellPrefab.transform as RectTransform).rect;
            Rect rctable = (tableView.transform as RectTransform).rect;
            Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
            Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
            Vector2 sizeCellNew = sizeCell;
            sizeCellNew.x = rctable.width;

            //int i =0;
            for (int i = 0; i < oneCellNum; i++)
            {
                int itemIndex = row * oneCellNum + i;
                float cell_space = 10;
                UICellItemBase item = (UICellItemBase)GameObject.Instantiate(cellItemPrefab);
                //item.itemDelegate = this;
                Rect rcItem = (item.transform as RectTransform).rect;
                item.width = (rctable.width - cell_space * (oneCellNum - 1)) / oneCellNum;
                item.height = heightCell;
                item.transform.SetParent(cell.transform, false);
                item.index = itemIndex;
                item.totalItem = totalItem;
                item.callbackClick = OnCellItemDidClick;
                cell.AddItem(item);

            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        cell.UpdateItem(listItem);
        return cell;
    }

    #endregion 
    #region Table View event handlers

    //Will be called by the TableView when a cell's visibility changed
    public void TableViewCellVisibilityChanged(int row, bool isVisible)
    {
        //Debug.Log(string.Format("Row {0} visibility changed to {1}", row, isVisible));
        if (isVisible)
        {

        }
    }

    #endregion

    public void IAPCallBack(string str)
    {
        Debug.Log("IAPCallBack::" + str);
        IAP.main.IAPCallBackBase(str);
    }

    #region UIViewAlert
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {
        if (STR_KEYNAME_VIEWALERT_NOAD == alert.keyName)
        {
            if (isYes)
            {
                DoBtnNoADIAP();

            }
        }
        if (STR_KEYNAME_VIEWALERT_UPDATE_VERSION == alert.keyName)
        {
            if (isYes)
            {

                if (AppVersion.main.appNeedUpdate)
                {
                    string url = AppVersion.main.strUrlAppstore;
                    if (!Common.BlankString(url))
                    {
                        Application.OpenURL(url);
                    }
                }

            }
        }




    }



    #endregion

    public void OnClickBtnCustom()
    {
        GameData.main.isCustom = true;
        OnClickBtnBack();
        GameManager.main.GotoPlayAgain();
    }

    public void OnClickBtnDefault()
    {
        GameData.main.isCustom = false;
    }

    public void OnClickBtnNoADIAP()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseNoAd;
        }
        else
        {
            DoBtnNoAdAlert();
        }
    }


    public void DoBtnNoAdAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_NOAD");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_NOAD");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_NOAD");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_NOAD");

        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_NOAD, OnUIViewAlertFinished);
    }
    public void DoBtnNoADIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack");

        // viewAlert.ShowBtnNo(false);
        // viewAlert.keyName = STR_KEYNAME_VIEWALERT_LOADING;
        // viewAlert.callback = OnUIViewAlertFinished;
        // string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_START_BUY);
        // string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_START_BUY);
        // string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
        // string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
        // viewAlert.SetText(title, msg, yes, no);
        // viewAlert.Show();
        IAP.main.StartBuy(IAP.productIdNoAD, false);

    }

    public void OnClickBtnRestoreIAP()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseRestoreIAP;
        }
        else
        {
            DoBtnRestoreIAP();
        }

    }

    //恢复内购
    public void DoBtnRestoreIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack");

        string product = IAP.productIdNoAD;
        if (Config.main.isNoIDFASDK && Common.isiOS)
        {
            product = Common.GetAppPackage() + "." + IAPConfig.main.GetIdByKey("unlocklevel");
        }
        IAP.main.RestoreBuy(product);

    }

    public void OnUIParentGateDidCloseNoAd(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnNoAdAlert();
        }
    }

    public void OnUIParentGateDidCloseRestoreIAP(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnRestoreIAP();
        }
    }
}
